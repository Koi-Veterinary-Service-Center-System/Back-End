using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.BookingRecord;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Authorization;
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
        public BookingRecordController(IBookingRecordRepository bookingRecordRepo, IBookingRepository bookingRepo)
        {
            _bookingRecordRepo = bookingRecordRepo;
            _bookingRepo = bookingRepo;
        }

        // [Authorize(Roles = "Vet, Staff, Manager")]
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

        // [Authorize(Roles = "Vet, Staff, Manager")]
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

        // [Authorize(Roles = "Vet, Staff, Manager")]
        [HttpPost("create-bookingRecord")]
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
            var bookingRecord = await _bookingRecordRepo.CreateAsync(createBookingRecordDTO.ToModelFromDTO());

            return Ok(bookingRecord.ToDTOFromModel());
        }

        // [Authorize(Roles = "Vet, Staff, Manager")]
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

            existingBookingRecord.TotalAmount = updateBookingRecordDTO.TotalAmount;
            existingBookingRecord.ArisedMoney = updateBookingRecordDTO.ArisedMoney;
            existingBookingRecord.BookingID = id;

            var updateBookingRecord = await _bookingRecordRepo.UpdateAsync(existingBookingRecord);

            return Ok(updateBookingRecord.ToDTOFromModel());
        }

        // [Authorize(Roles = "Vet, Staff, Manager")]
        [HttpDelete("delete-bookingRecord/{id}")]
        public async Task<IActionResult> DeleteBookingRecord([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookingRecord = await _bookingRecordRepo.GetByIDAsync(id);
            if (bookingRecord == null)
            {
                return NotFound("Can not find this booking's record!");
            }

            await _bookingRecordRepo.DeleteAsync(bookingRecord);

            return Ok("Deleted successfully!");
        }

    }
}