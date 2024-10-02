using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.Service;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;
        public ServiceController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet("all-service")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllService()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var services = await _serviceRepo.GetAllService();
            if (services == null || !services.Any()) return NotFound("Can not find any service");

            var serviceDto = services.Select(s => s.ToServiceDto()).ToList();

            return Ok(serviceDto);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = await _serviceRepo.GetServiceById(id);
            if (service == null) return NotFound("Can not find any service");

            return Ok((service.ToServiceDto()));
        }

        [HttpPost("add-service")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AddService([FromBody] AddUpdateServiceDTO serviceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (serviceDto.Price <= 0) return BadRequest("Price must bigger than 0");
            if (serviceDto.EstimatedDuration <= 0) return BadRequest("Duration must bigger than 0");

            var serviceModel = serviceDto.ToServiceFromAddUpdateDto();
            await _serviceRepo.CreateService(serviceModel);
            return CreatedAtAction(nameof(GetServiceById), new { id = serviceModel.ServiceID }, serviceModel.ToServiceDto());
        }

        [HttpPut("update-service/{id:int}")]
        public async Task<IActionResult> UpdateService([FromRoute] int id, [FromBody] AddUpdateServiceDTO serviceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (serviceDto.Price <= 0) return BadRequest("Price must bigger than 0");
            if (serviceDto.EstimatedDuration <= 0) return BadRequest("Duration must bigger than 0");

            var serviceModel = await _serviceRepo.UpdateService(id, serviceDto);

            if (serviceModel == null) return NotFound("Can not find service");

            return Ok(serviceModel.ToServiceDto());
        }

        [HttpDelete("delete-service/{id:int}")]
        public async Task<IActionResult> DeleteService([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var serviceModel = await _serviceRepo.DeleteService(id);

            if (serviceModel == null) return NotFound("Can not find service");

            return NoContent();
        }
    }
}