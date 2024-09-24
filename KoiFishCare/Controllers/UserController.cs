using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using KoiFishCare.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.Interfaces;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase 
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ItokenService tokenService;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, ItokenService tokenService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid login");
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {               
                var customer = user as Customer;
                var userDto = new UserDto
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = tokenService.CreateToken(customer)
                };

            return Ok(userDto);
            }
            return Unauthorized("Invalid login");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(customer, model.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(customer, false);
                
                var userDto = new UserDto
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = tokenService.CreateToken(customer)
                };
            return Ok(userDto);
            }
            return BadRequest(result.Errors);
        }
    }
}
