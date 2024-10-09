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
using Microsoft.AspNetCore.Http.HttpResults;

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
        private readonly IVetSlotRepository _vetSlotRepo;
        private readonly IPrescriptionRecordRepository _presRecRepo;

        public BookingController(UserManager<User> userManager, IFishOrPoolRepository fishOrPoolRepo,
        ISlotRepository slotRepo, IServiceRepository serviceRepo, IBookingRepository bookingRepo,
        IVetSlotRepository vetSlotRepo, IPrescriptionRecordRepository presRecRepo)
        {
            _userManager = userManager;
            _fishOrPoolRepo = fishOrPoolRepo;
            _slotRepo = slotRepo;
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
            _vetSlotRepo = vetSlotRepo;
            _presRecRepo = presRecRepo;
        }


        [Authorize]
        [HttpPost("create-booking")]
        [Authorize(Roles = "Customer")]
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

            // Check if the booking date is at least 1 day after today
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (createBookingDto.BookingDate <= currentDate.AddDays(1))
            {
                return BadRequest("Booking date must be at least 1 day after today.");
            }

            // Get customer fishorpool if cus want to examinate
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

                var vetSlot = await _vetSlotRepo.GetVetSlot(vet.Id, slot.SlotID);
                if (vetSlot == null)
                {
                    return BadRequest("The selected vet slot does not exist.");
                }

                if (vetSlot.isBooked == true)
                {
                    return BadRequest("The selected vet is already booked");
                }

                var bookingModel = createBookingDto.ToBookingFromCreate();
                bookingModel.CustomerID = userModel.Id;
                bookingModel.VetID = vet.Id;
                bookingModel.ServiceID = service.ServiceID;
                bookingModel.Slot = slot;
                bookingModel.Service = service;
                bookingModel.TotalAmount = createBookingDto.TotalAmount;

                var result = await _bookingRepo.CreateBooking(bookingModel);

                await _vetSlotRepo.Update(vetSlot.VetID, vetSlot.SlotID, true);

                return Ok(result.ToDtoFromModel());
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

                var result = await _bookingRepo.CreateBooking(bookingModel);

                await _vetSlotRepo.Update(availableVet.VetID, availableVet.SlotID, true);

                return Ok(result.ToDtoFromModel());
            }
        }

        [Authorize]
        [HttpGet("view-booking-process")]
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
                var bookings = await _bookingRepo.GetBookingByVetIdAsync(user.Id);
                if (bookings == null || !bookings.Any())
                {
                    return NotFound("There is no booking!");
                }

                var result = bookings.Select(x => x.ToDtoFromModel());
                return Ok(result);
            }

            if (isCus)
            {
                var bookings = await _bookingRepo.GetBookingsByCusIdAsync(user.Id);
                if (bookings == null || !bookings.Any())
                {
                    return NotFound("There is no booking!");
                }

                var result = bookings.Select(x => x.ToDtoFromModel());
                return Ok(result);
            }

            return BadRequest("Invalid request");
        }

        [Authorize]
        [HttpGet("view-booking-history")]
        public async Task<IActionResult> GetBookingHistory()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Unauthorized("User is not available!");
            }

            var bookingHistorys = await _bookingRepo.GetBookingByStatusAsync(user.Id);
            if (bookingHistorys == null || !bookingHistorys.Any())
            {
                return NotFound("There is no booking!");
            }
            
            var result = bookingHistorys.Select(x => x.ToDtoFromModel());
            return Ok(result);
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
                if (booking.BookingStatus == BookingStatus.Ongoing || booking.BookingStatus == BookingStatus.Completed)
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }

            if (User.IsInRole("Staff"))
            {
                if (booking.BookingStatus == BookingStatus.Pending || booking.BookingStatus == BookingStatus.Scheduled)
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }
            }

            if (User.IsInRole("Customer"))
            {
                if (booking.BookingStatus == BookingStatus.Received_Money)
                {
                    booking.BookingStatus = newStatus;
                    _bookingRepo.UpdateBooking(booking);
                }
            }

            return Ok("Update status successfully!");
        }


        [Authorize]
        [HttpPut("cancel-booking/{bookingId:int}")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null) return Unauthorized("User is not available!");

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null) return NotFound("Booking not found!");
            if (booking.BookingStatus.Equals(BookingStatus.Cancelled) || booking.BookingStatus.Equals(BookingStatus.Refunded))
                return BadRequest("The booking is already canceled");

            if (User.IsInRole("Customer"))
                if (booking.CustomerID != user.Id)
                    return Unauthorized("You can only cancel your own bookings.");

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var daysBeforeBooking = (booking.BookingDate.ToDateTime(TimeOnly.MinValue) - currentDate.ToDateTime(TimeOnly.MinValue)).Days;

            decimal refundPercent = 0; // default no refund
            if (daysBeforeBooking > 7)
                refundPercent = 100;
            else if (daysBeforeBooking > 3)
                refundPercent = 50;


            var refundMoney = booking.TotalAmount * (refundPercent / 100);

            var presRec = new PrescriptionRecord
            {
                BookingID = booking.BookingID,
                RefundPercent = refundPercent,
                RefundMoney = refundMoney,
                CreateAt = DateTime.Now
            };

            await _presRecRepo.Create(presRec);

            booking.BookingStatus = BookingStatus.Cancelled;
            _bookingRepo.UpdateBooking(booking);

            return Ok(presRec.ToPresRecDtoFromModel());
        }

        [HttpGet("all-booking")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> GetAllBooking()
        {
            var bookings = await _bookingRepo.GetAllBooking();
            if (!bookings.Any()) return NotFound("No booking!");

            return Ok(bookings);
        }

    }
}