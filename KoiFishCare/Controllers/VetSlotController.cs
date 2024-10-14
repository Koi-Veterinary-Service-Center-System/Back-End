using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.VetSlot;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/vetslot")]
    [ApiController]
    public class VetSlotController : ControllerBase
    {
        private readonly IVetSlotRepository _vetSlotRepo;
        private readonly ISlotRepository _slotRepo;
        private readonly IVetRepository _vetRepo;

        private readonly UserManager<User> _userManager;
        public VetSlotController(IVetSlotRepository vetSlotRepo, ISlotRepository slotRepo, IVetRepository vetRepo, UserManager<User> userManager)
        {
            _vetSlotRepo = vetSlotRepo;
            _slotRepo = slotRepo;
            _vetRepo = vetRepo;
            _userManager = userManager;
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

            var dto = vetSlotList.Select(vs => vs.ToVetSlotDtoFromModel());

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{slotId}/{vetId}")]
        public async Task<IActionResult> GetById(int slotId, string vetId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.GetVetSlot(vetId, slotId);
            if (vetSlot == null) return NotFound();

            return Ok(vetSlot.ToVetSlotDtoFromModel());
        }

        [Authorize]
        [HttpPost("add-vetslot")]
        public async Task<IActionResult> Create([FromBody] AddVetSlotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var slot = await _slotRepo.GetSlotById(dto.SlotID);
            if (slot == null) return BadRequest("Invalid SlotId");

            var vet = await _vetRepo.GetVetById(dto.VetID);
            if (vet == null) return BadRequest("Invalid VetId");

            var result = await _vetSlotRepo.Create(dto.VetID, dto.SlotID, false);
            return Ok(result.ToVetSlotDtoFromModel());
        }

        [Authorize]
        [HttpPut("update-vetslot")]
        public async Task<IActionResult> Update([FromBody] UpdateVetSlotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.Update(dto.VetID, dto.SlotID, dto.isBook);
            if (vetSlot == null) return NotFound();

            return Ok(vetSlot.ToVetSlotDtoFromModel());
        }

        [Authorize]
        [HttpDelete("delete-vetslot/{vetId}/{slotId}")]
        public async Task<IActionResult> Delete(string vetId, int slotId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vetSlot = await _vetSlotRepo.Delete(vetId, slotId);
            if (vetSlot == null) return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Vet")]
        [HttpGet]
        public async Task<IActionResult> GetVetSlotByVetID()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return Unauthorized();
            }

            var vetSlots = await _vetSlotRepo.GetVetSlotByVetID(id);
            if (vetSlots == null || !vetSlots.Any())
            {
                return NotFound();
            }

            var result = vetSlots.Select(x => x.ToVetSlotDtoFromModel());

            return Ok(result);
        }
    }
}