using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using KoiFishCare.Dtos.BookingRecord;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KoiFishCare.Controllers
{
    [Route("api/bookingRecord")]
    [ApiController]
    public class BookingRecordController : Controller
    {
        private readonly IBookingRecordRepository _bookingRecordRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<User> _userManager;
        public BookingRecordController(IBookingRecordRepository bookingRecordRepo, IBookingRepository bookingRepo, UserManager<User> userManager)
        {
            _bookingRecordRepo = bookingRecordRepo;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("{bookingID}")]
        public async Task<IActionResult> GetBookingRecordByBookingID([FromRoute] int bookingID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookingRecord = await _bookingRecordRepo.GetByBookingIDAsync(bookingID);
            if (bookingRecord == null)
            {
                return NotFound("Can not find this booking's record!");
            }

            return Ok(bookingRecord.ToDTOFromModel());
        }

        [Authorize]
        [HttpGet("all-bookingRecord")]
        public async Task<IActionResult> GetAllBookingRecord()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookingRecords = await _bookingRecordRepo.GetAllAsync();
            if (bookingRecords == null || !bookingRecords.Any())
            {
                return NotFound("Can not find any booking's record!");
            }

            return Ok(bookingRecords.Select(x => x.ToDTOFromModel()));
        }

        [Authorize(Roles = "Vet")]
        [HttpPost("create-bookingRecord/auto-completed-or-received-money-booking")]
        public async Task<IActionResult> CreateBookingRecord([FromBody] FromCreateBookingRecordDTO createBookingRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBooking = await _bookingRepo.GetBookingByIdAsync(createBookingRecordDTO.BookingID);
            if (existingBooking == null)
            {
                return NotFound("Can not found this booking!");
            }

            var id = _userManager.GetUserId(this.User);
            if (User.IsInRole("Vet"))
            {
                if (existingBooking.Veterinarian.Id != id)
                {
                    return Unauthorized("You can only create booking's record of your own booking!");
                }
            }

            BookingRecord bookingRecord = new BookingRecord()
            {
                BookingID = createBookingRecordDTO.BookingID,
                ArisedQuantity = createBookingRecordDTO.ArisedQuantity,
                QuantityMoney = (createBookingRecordDTO.ArisedQuantity + existingBooking.Quantity) * existingBooking.Service.QuantityPrice,
                TotalAmount = ((createBookingRecordDTO.ArisedQuantity + existingBooking.Quantity) * existingBooking.Service.QuantityPrice) + existingBooking.InitAmount,
                Note = createBookingRecordDTO.Note,
            };

            var result = await _bookingRecordRepo.CreateAsync(bookingRecord);
            if (existingBooking.isPaid == true && bookingRecord.QuantityMoney == 0)
            {
                existingBooking.BookingStatus = BookingStatus.Received_Money;
            }
            else
            {
                existingBooking.BookingStatus = BookingStatus.Completed;
            }
            await _bookingRepo.UpdateBooking(existingBooking);
            return Ok(result.ToDTOFromModel());
        }

        [Authorize(Roles = "Vet")]
        [HttpPut("update-bookingRecord/{id}/{bookingID}")]
        public async Task<IActionResult> UpdateBookingRecord([FromRoute] int id, int bookingID, [FromBody] FromUpdateBookingRecordDTO updateBookingRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBooking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (existingBooking == null)
            {
                return BadRequest("Can not find this booking!");
            }

            var existingBookingRecord = await _bookingRecordRepo.GetByIDAsync(id);
            if (existingBookingRecord == null)
            {
                return NotFound("Can not find this booking's record!");
            }

            existingBookingRecord.BookingID = bookingID;
            existingBookingRecord.ArisedQuantity = updateBookingRecordDTO.ArisedQuantity;
            existingBookingRecord.QuantityMoney = (updateBookingRecordDTO.ArisedQuantity + existingBooking.Quantity) * existingBooking.Service.QuantityPrice;
            existingBookingRecord.TotalAmount = existingBooking.InitAmount + ((updateBookingRecordDTO.ArisedQuantity + existingBooking.Quantity) * existingBooking.Service.QuantityPrice);
            existingBookingRecord.Note = existingBooking.Note;

            var updateBookingRecord = await _bookingRecordRepo.UpdateAsync(existingBookingRecord);

            return Ok(updateBookingRecord.ToDTOFromModel());
        }

        // [Authorize(Roles = "Vet")]
        // [HttpDelete("delete-bookingRecord/{id}")]
        // public async Task<IActionResult> DeleteBookingRecord([FromRoute] int id)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var bookingRecord = await _bookingRecordRepo.GetByIDAsync(id);
        //     if (bookingRecord == null)
        //     {
        //         return NotFound("Can not find this booking's record!");
        //     }

        //     await _bookingRecordRepo.DeleteAsync(bookingRecord);

        //     return Ok("Deleted successfully!");
        // }

    }
}