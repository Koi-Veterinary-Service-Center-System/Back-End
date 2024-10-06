using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.PrescriptionRecord;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/pres-rec")]
    public class PrescriptionRecordController : ControllerBase
    {
        
        private readonly IPrescriptionRecordRepository _preRecRepo;
        private readonly UserManager<User> _userManager;
        private readonly IBookingRepository _bookingRepo;
        public PrescriptionRecordController(IPrescriptionRecordRepository preRecRepo, UserManager<User> userManager,
        IBookingRepository bookingRepo)
        {
            _preRecRepo = preRecRepo;
            _userManager = userManager;
            _bookingRepo = bookingRepo;
        }

        [HttpGet("{presRecId:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int presRecId)
        {
            var presRec = await _preRecRepo.GetById(presRecId);
            if(presRec == null) return NotFound();
            return Ok(presRec.ToPresRecDtoFromModel());
        }

        [HttpGet("list-preRec-CusName/{cusName}")]
        public async Task<IActionResult> GetListByCusname([FromRoute] string cusName)
        {
            var list = await _preRecRepo.GetListByCusUsername(cusName);
            if(!list.Any()) return NotFound("There is no prescription record of this customer!");
            var dto = list.Select(p => p.ToPresRecDtoFromModel());
            return Ok(dto);
        }

        [HttpPost("create-presRec")]
        [Authorize(Roles = "Staff, Vet")]
        public async Task<IActionResult> Create([FromBody] CreatePresRecordDto createDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _bookingRepo.GetBookingByIdAsync(createDto.BookingID);

            //check only the vet of the booking or staff can create
            if(User.IsInRole("Vet"))
            {
                var user = await _userManager.GetUserAsync(this.User);
                if(user.UserName != booking.VetID) return Unauthorized("This booking is not yours, you can not create record for this!");
            }

            var presRec = await _preRecRepo.Create(createDto.ToModelFromCreate());
            if(presRec == null) return NotFound("Can not find BookingId");

            return Ok(presRec.ToPresRecDtoFromModel());
        }

        [HttpPut("update-presRec/{presRecId:int}")]
        [Authorize(Roles = "Staff, Vet")]
        public async Task<IActionResult> Update([FromRoute] int presRecId, [FromBody] UpdatePresRecordDto updateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _bookingRepo.GetBookingByIdAsync(updateDto.BookingID);

            //check only the vet of the booking or staff can create
            if(User.IsInRole("Vet"))
            {
                var user = await _userManager.GetUserAsync(this.User);
                if(user.Id != booking.VetID) return Unauthorized("This booking is not yours, you can not update record for this!");
            }

            var presRec = await _preRecRepo.Update(presRecId, updateDto.ToModelFromUpdate());
            if(presRec == null) return NotFound();

            return Ok(presRec);
        }

        [HttpDelete("delete-presRec/{presRec:int}/{bookingId:int}")]
        [Authorize(Roles = "Staff, Vet")]
        public async Task<IActionResult> Delete(int presRecId, int bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);

            if(User.IsInRole("Vet"))
            {
                var user = await _userManager.GetUserAsync(this.User);
                if(user.UserName != booking.VetID) return Unauthorized("This booking is not yours, you can not delete record for this!");
            }

            var presRec = await _preRecRepo.Delete(presRecId);
            if(presRec == null) return NotFound();

            return NoContent();
        }

    }
}