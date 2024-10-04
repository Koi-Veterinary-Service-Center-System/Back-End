using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Distance;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanceController : ControllerBase
    {
        private readonly IDistanceRepository _distanceRepo;

        public DistanceController(IDistanceRepository distanceRepo)
        {
            _distanceRepo = distanceRepo;
        }

        [HttpGet("all-distance")]
        public async Task<IActionResult> GetAllDistance()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distances = await _distanceRepo.GetAllDistanceAsync();
            if (distances == null || !distances.Any())
            {
                return NotFound("Can not find any distance!");
            }

            var distanceDTO = distances.Select(x => x.ToDTOFromModel()).ToList();
            return Ok(distanceDTO);
        }

        [HttpGet("get-distance/{id:int}")]
        public async Task<IActionResult> GetDistanceById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            var distance = await _distanceRepo.GetDistanceById(id);
            if (distance == null)
            {
                return NotFound("Can not find any distance!");
            }

            return Ok(distance.ToDTOFromModel());

        }

        [HttpPost("create-distance")]
        public async Task<IActionResult> CreateDistance([FromBody] AddDistanceDTO distanceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (distanceDTO.Price < 0)
            {
                return BadRequest("Price must bigger than or equal to 0");
            }

            var distance = await _distanceRepo.CreateDistance(distanceDTO.ToModelFromDTO());
            return Ok(distance.ToDTOFromModel());
        }

        [HttpPut("update-distance/{id:int}")]
        public async Task<IActionResult> UpdateDistance([FromRoute] int id, [FromBody] AddDistanceDTO distanceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (distanceDTO.Price < 0)
            {
                return BadRequest("Price must bigger than or equal to 0");
            }

            var distance = await _distanceRepo.UpdateDistanceById(id, distanceDTO);
            if (distance == null)
            {
                return NotFound("Can not find any distance!");
            }

            return Ok(distance.ToDTOFromModel());
        }

        [HttpDelete("delete-distance/{id:int}")]
        public async Task<IActionResult> DeleteDistance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distance = await _distanceRepo.DeleteDistance(id);
            if (distance == null)
            {
                return NotFound("Can not find any distance!");
            }

            return Ok(distance.ToDTOFromModel());
        }
    }


}
