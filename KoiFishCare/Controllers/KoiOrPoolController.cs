using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.KoiOrPool;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/koi-or-pool")]
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
        public async Task<IActionResult> GetKoiOrPoolOfCus()
        {
            var customer = await _userManager.GetUserAsync(this.User);

            var listKoiOrPool = await _fishOrPoolRepo.GetKoiOrPoolsOfCustomer(customer.Id);
            
            if(listKoiOrPool == null || !listKoiOrPool.Any()) return BadRequest("Create a Fish or Pool to select");

            var dto = listKoiOrPool.Select(k => new KoiOrPoolDto
            {
                KoiOrPoolID = k.KoiOrPoolID,
                Name = k.Name,
                IsPool = k.IsPool,
                Description = k.Description,
                CustomerId = k.CustomerId
            }).ToList();

            return Ok(dto);
        }
    }
}