using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Vet;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/vet")]
    [ApiController]
    public class VetController : ControllerBase
    {
        private readonly IVetRepository _vetRepo;
        private readonly ISlotRepository _slotRepo;
        public VetController(IVetRepository vetRepo, ISlotRepository slotRepo)
        {
            _vetRepo = vetRepo;
            _slotRepo = slotRepo;
        }

        [HttpGet("all-vet")]
        public async Task<IActionResult> GetAllVet()
        {
            var vet = await _vetRepo.GetAllVet();
            var vetDto = vet.Select(v => v.ToVetDto()).ToList();
            return Ok(vetDto);
        }

        [HttpGet("available-vet/{slotId:int}")]
        public async Task<IActionResult> GetAvailableVetFromSlotId(int slotId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vets = await _slotRepo.GetListAvailableVet(slotId);
            if (vets == null)
                return BadRequest("There is no available vet with this slot");

            var vetDtos = vets.Select(vs => new VetDto
            {
                Id = vs.Veterinarian.Id.ToString(),
                VetName = vs.Veterinarian.UserName,
                FirstName = vs.Veterinarian.FirstName,
                LastName = vs.Veterinarian.LastName,
                Gender = vs.Veterinarian.Gender,
                ExperienceYears = vs.Veterinarian.ExperienceYears
            }).ToList();

            return Ok(vetDtos);
        }
    }
}