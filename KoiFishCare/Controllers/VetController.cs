using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public VetController(IVetRepository vetRepo)
        {
            _vetRepo = vetRepo;
        }   

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllVet()
        {
            var vet = await _vetRepo.GetAllVet();
            vet.Select(v => v.ToVetDto());
            return Ok(vet);
        }
    }
}