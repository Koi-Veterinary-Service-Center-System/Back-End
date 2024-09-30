using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.VetSlot;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/vetslot")]
    [ApiController]
    public class VetSlotController : ControllerBase
    {
        private readonly IVetSlotRepository _vetSlotRepo;
        public VetSlotController(IVetSlotRepository vetSlotRepo)
        {
            _vetSlotRepo = vetSlotRepo;
        }

        [Authorize]
        [HttpGet("vetslot-list")]
        public async Task<IActionResult> GetListVetSlot()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlotList = await _vetSlotRepo.GetAllVetSlot();
            if (vetSlotList == null || !vetSlotList.Any())
                return NotFound();

            return Ok(vetSlotList);
        }

        [Authorize]
        [HttpGet("{slotId}/{vetId}")]
        public async Task<IActionResult> GetById(int slotId, string vetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.GetVetSlot(vetId, slotId);
            if (vetSlot == null) return NotFound();

            return Ok(vetSlot);
        }

        [Authorize]
        [HttpPost("add-vetslot")]
        public async Task<IActionResult> Create([FromBody] AddVetSlotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vetSlotRepo.Create(dto.VetID, dto.SlotID, false);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update-vetslot")]
        public async Task<IActionResult> Update([FromBody] UpdateVetSlotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.Update(dto.VetID, dto.SlotID, dto.isBook);
            if (vetSlot == null) return NotFound();

            return Ok(vetSlot);
        }

        [Authorize]
        [HttpDelete("delete-vetslot/{vetId}/{slotId}")]
        public async Task<IActionResult> Delete(string vetId, int slotId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.Delete(vetId, slotId);
            if(vetSlot == null) return NotFound();

            return NoContent();
        }
    }
}