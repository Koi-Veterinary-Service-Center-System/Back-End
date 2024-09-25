using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Service;
using KoiFishCare.Interfaces;
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
        public async Task<IActionResult> GetAllService()
        {
            var services = await _serviceRepo.GetAllService();
            if(services == null || !services.Any()) return BadRequest("Can not find any service");

            var serviceDto = services.Select(s => new ServiceDto
            {
                ServiceID = s.ServiceID,
                ServiceName = s.ServiceName,
                Description = s.Description,
                Price = s.Price,
                EstimatedDuration = s.EstimatedDuration
            }).ToList();
            
            return Ok(serviceDto);
        }
    }
}