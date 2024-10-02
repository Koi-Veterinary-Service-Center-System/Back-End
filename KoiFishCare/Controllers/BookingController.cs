using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.Mappers;
using Microsoft.VisualBasic;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IFishOrPoolRepository _fishOrPoolRepo;
        private readonly ISlotRepository _slotRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IBookingRepository _bookingRepo;

        public BookingController(UserManager<User> userManager, IFishOrPoolRepository fishOrPoolRepo,
        ISlotRepository slotRepo, IServiceRepository serviceRepo, IBookingRepository bookingRepo)
        {
            _userManager = userManager;
            _fishOrPoolRepo = fishOrPoolRepo;
            _slotRepo = slotRepo;
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
        }


        [Authorize]
        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking([FromBody] FromCreateBookingDTO createBookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get logged in customer
            var userModel = await _userManager.GetUserAsync(this.User);
            if (userModel == null)
                return BadRequest("Login before booking!");

            //check role


            // Get customer fishorpool
            if (createBookingDto.KoiOrPoolId.HasValue)
            {
                var fishorpool = await _fishOrPoolRepo.GetKoiOrPoolById(createBookingDto.KoiOrPoolId.Value);
                if (fishorpool == null) return BadRequest("Fish or Pool does not exist");
                if (fishorpool.CustomerID != userModel.Id) return BadRequest("Not your koi or pool");
            }


            // Get Slot
            var slot = await _slotRepo.GetSlotById(createBookingDto.SlotId);
            if (slot == null) return BadRequest("Slot does not exist");

            // Get Service
            var service = await _serviceRepo.GetServiceById(createBookingDto.ServiceId);
            if (service == null) return BadRequest("Service does not exist");

            // Get vet
            var vetUsername = createBookingDto.VetName;
            if (vetUsername != null)
            {
                var vet = await _userManager.FindByNameAsync(vetUsername) as Veterinarian;
                if (vet == null)
                {
                    return BadRequest("Vet does not exist");
                }

                var bookingsInDateAndSlot = await _bookingRepo.GetBookingsByDateAndSlot(createBookingDto.BookingDate, slot.SlotID);
                if (bookingsInDateAndSlot.Any(b => b.VetID == vet.Id))
                    return BadRequest("The selected vet is already booked");

                var bookingModel = createBookingDto.ToBookingFromCreate();
                bookingModel.CustomerID = userModel.Id;
                bookingModel.VetID = vet.Id;
                bookingModel.ServiceID = service.ServiceID;
                bookingModel.Slot = slot;
                bookingModel.Service = service;
                bookingModel.TotalAmount = createBookingDto.TotalAmount;

                await _bookingRepo.CreateBooking(bookingModel);
                return Ok(bookingModel.ToBookingDtoFromModel());
            }
            else
            {
                var availableVet = await _slotRepo.GetAvailableVet(slot);
                if (availableVet == null)
                    return BadRequest("No available vet for the chosen slot");

                var bookingModel = createBookingDto.ToBookingFromCreate();
                bookingModel.CustomerID = userModel.Id;
                bookingModel.VetID = availableVet.VetID;
                bookingModel.ServiceID = service.ServiceID;
                bookingModel.Slot = slot;
                bookingModel.Service = service;
                bookingModel.TotalAmount = service.Price;

                await _bookingRepo.CreateBooking(bookingModel);
                return Ok(bookingModel.ToBookingDtoFromModel());
            }
        }

        [Authorize]
        [HttpGet("all-booking")]
        public async Task<IActionResult> GetBookingByID()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var isVet = await _userManager.IsInRoleAsync(user, "Vet");
            var isCus = await _userManager.IsInRoleAsync(user, "Customer");

            if (!isVet && !isCus)
            {
                return Unauthorized("You have no permission to access this feature!");
            }

            if (isVet)
            {
                var booking = await _bookingRepo.GetBookingByVetIdAsync(user.Id);
                if (booking == null || !booking.Any())
                {
                    return NotFound("There is no booking!");
                }
                return Ok(booking);
            }

            if (isCus)
            {
                var booking = await _bookingRepo.GetBookingsByCusIdAsync(user.Id);
                if (booking == null || !booking.Any())
                {
                    return NotFound("There is no booking!");
                }
                return Ok(booking);
            }

            return BadRequest("Invalid request");
        }

        [Authorize]
        [HttpPut("update-status")]
        [Authorize(Roles = "Staff, Customer, Vet")]
        public async Task<IActionResult> UpdateStatusForVet(int bookingID, BookingStatus newStatus)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Vet"))
                if (booking.BookingStatus.Equals("Ongoing") || booking.BookingStatus.Equals("Completed"))
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }

            if (User.IsInRole("Staff"))
            {
                if (booking.BookingStatus.Equals("Pending") || booking.BookingStatus.Equals("Scheduled"))
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }
            }

            if (User.IsInRole("Customer"))
            {
                if (booking.BookingStatus.Equals("Received_Money"))
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }
            }

            return Ok(booking);
        }
    }
}