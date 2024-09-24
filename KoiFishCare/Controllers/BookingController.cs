using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.Mappers;

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
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] FromCreateBookingDto createBookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get logged in customer
            var username = _userManager.GetUserName(this.User);
            var userModel = await _userManager.FindByNameAsync(username);

            //check role


            // Get customer fishorpool
            var fishorpool = await _fishOrPoolRepo.GetKoiOrPoolById(createBookingDto.KoiOrPoolId);
            if (fishorpool == null) return BadRequest("Fish or Pool does not exist");
            if (fishorpool.CustomerId != userModel.Id) return BadRequest("Not your koi or pool");

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
                bookingModel.TotalAmount = service.Price;

                await _bookingRepo.CreateBooking(bookingModel);
                return Ok(bookingModel);
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
                return Ok(bookingModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooking()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var booking = await _bookingRepo.GetBookingsByUserIdAsync(user.Id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }
    }
}