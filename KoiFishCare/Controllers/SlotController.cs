using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Slot;
using KoiFishCare.DTOs.Slot;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/slot")]
    [AllowAnonymous]
    public class SlotController : ControllerBase
    {
        private readonly ISlotRepository _slotRepo;
        public SlotController(ISlotRepository slotRepo)
        {
            _slotRepo = slotRepo;
        }

        [HttpGet("all-slot")]
        public async Task<IActionResult> GetAllSlot()
        {
            var slots = await _slotRepo.GetAllSlot();
            if (slots == null || !slots.Any()) return BadRequest("Can not find any slot");

            var slotsDto = slots.Select(s => s.ToSlotDto()).ToList();
            return Ok(slotsDto);
        }

        [HttpGet("available-slot/{vetId}")]
        public async Task<IActionResult> GetAvailableSlotFromVetId(string vetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var slots = await _slotRepo.GetListAvailableSlot(vetId);
            if (slots == null || !slots.Any())
                return BadRequest("There is no available slot with this vet");

            var slotsDto = slots.Select(vs => new SlotDTO
            {
                SlotID = vs?.Slot?.SlotID,
                StartTime = vs?.Slot?.StartTime,
                EndTime = vs?.Slot?.EndTime,
                WeekDate = vs?.Slot?.WeekDate.ToString()
            }
            ).ToList();

            return Ok(slotsDto);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("create-slot")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateSlotDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TimeOnly.TryParse(dto.StartTime, out var parsedStartTime))
            {
                return BadRequest("Invalid StartTime format.");
            }

            if (!TimeOnly.TryParse(dto.EndTime, out var parsedEndTime))
            {
                return BadRequest("Invalid EndTime format.");
            }

            var result = await _slotRepo.Create(dto.ToSlotModel(parsedStartTime, parsedEndTime));
            return Ok(result.ToSlotDto());
        }


        [Authorize(Roles = "Staff")]
        [HttpPut("update-slot/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateUpdateSlotDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TimeOnly.TryParse(dto.StartTime, out var parsedStartTime))
            {
                return BadRequest("Invalid StartTime format.");
            }

            if (!TimeOnly.TryParse(dto.EndTime, out var parsedEndTime))
            {
                return BadRequest("Invalid EndTime format.");
            }

            var result = await _slotRepo.UpdateSlot(id, dto.ToSlotModel(parsedStartTime, parsedEndTime));
            if (result == null) return NotFound("Slot not found");

            return Ok(result.ToSlotDto());
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("delete-slot/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _slotRepo.Delete(id);
            if (result == null) return NotFound("Slot not found");

            return NoContent();
        }

        [HttpPatch("soft-delete/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> SoftDelete([FromRoute] int id)
        {
            var existingSlot = await _slotRepo.GetSlotById(id);
            if (existingSlot == null)
            {
                return NotFound("Can not find this service!");
            }

            existingSlot.IsDeleted = true;
            await _slotRepo.Update(existingSlot);
            return Ok("Deleted successfully!");
        }
    }
}