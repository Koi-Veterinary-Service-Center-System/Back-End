using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.KoiOrPool;
using KoiFishCare.DTOs.KoiOrPool;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/koi-or-pool")]
    [ApiController]
    public class KoiOrPoolController : ControllerBase
    {
        private readonly IFishOrPoolRepository _fishOrPoolRepo;
        private readonly UserManager<User> _userManager;
        public KoiOrPoolController(IFishOrPoolRepository fishOrPoolRepo, UserManager<User> userManager)
        {
            _fishOrPoolRepo = fishOrPoolRepo;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("all-customer-koi-pool")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetKoiOrPoolOfCus()
        {
            var customer = await _userManager.GetUserAsync(this.User);
                if(customer == null) return Unauthorized();

            var listKoiOrPool = await _fishOrPoolRepo.GetKoiOrPoolsOfCustomer(customer.Id);
            
            if(listKoiOrPool == null || !listKoiOrPool.Any()) return BadRequest("Create a Fish or Pool to select");

            var dto = listKoiOrPool.Select(k => k.ToDtoFromModel()).ToList();

            return Ok(dto);
        }

        [Authorize]
        [HttpPost("create-koiorpool")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateKoiOrPoolDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _userManager.GetUserAsync(this.User);
                if(customer == null) return Unauthorized();

            var model = dto.ToModelFromCreateUpdate();
            model.CustomerID = customer.Id;
            await _fishOrPoolRepo.Create(model);
            return Ok(model.ToDtoFromModel());
            }

        [Authorize]
        [HttpPut("update-koiorpool/{id:int}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody] CreateUpdateKoiOrPoolDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _userManager.GetUserAsync(this.User);
                if(customer == null) return Unauthorized();

            var model = await _fishOrPoolRepo.GetKoiOrPoolById(id);
            if(model != null)
                if(model.CustomerID != customer.Id)
                    return BadRequest("Not your koi or pool!");

            var result = await _fishOrPoolRepo.Update(id, dto.ToModelFromCreateUpdate());
            if(result == null) return NotFound("Can not find koi or pool");

            return Ok(result.ToDtoFromModel());
        }

        [Authorize]
        [HttpDelete("delete-koiorpool/{id:int}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _userManager.GetUserAsync(this.User);
                if(customer == null) return Unauthorized();

            var model = await _fishOrPoolRepo.GetKoiOrPoolById(id);
            if(model != null)
                if(model.CustomerID != customer.Id)
                    return BadRequest("Not your koi or pool!");

            var result = await _fishOrPoolRepo.Delete(id);
            if(result == null) return NotFound("Can not find koi or pool");

            return NoContent();
        }
    }
}