using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Slot;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/slot")]
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
            if(slots == null || !slots.Any()) return BadRequest("Can not find any slot");
            
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

            var slotsDto = slots.Select(vs => new SlotDto
            {
                SlotID = vs.Slot.SlotID,
                StartTime = vs.Slot.StartTime,
                EndTime = vs.Slot.EndTime,
                WeekDate = vs.Slot.WeekDate
            }
            ).ToList();

            return Ok(slotsDto);
        }
    }
}