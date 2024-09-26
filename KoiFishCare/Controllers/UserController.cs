using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using KoiFishCare.Mappers;
using KoiFishCare.Dtos.User;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ItokenService _tokenService;

        private readonly IUserRepository _userRepo;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, ItokenService tokenService, IUserRepository userRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid login");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                var customer = user as Customer;
                var userDto = new UserDto
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = _tokenService.CreateToken(customer)
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

            var result = await _userManager.CreateAsync(customer, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(customer, false);

                var userDto = new UserDto
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = _tokenService.CreateToken(customer)
                };
                return Ok(userDto);
            }
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ViewProfile()
        {
            var id = _userManager.GetUserId(this.User);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var result = user.ToUserProfileFromUser();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDTO updateDTO)
        {
            var id = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("Can not find user!");
            }

            var userDTO = updateDTO;
            if (updateDTO.UserName.IsNullOrEmpty())
            {
                userDTO.UserName = user.UserName;
            }
            if (updateDTO.FirstName.IsNullOrEmpty())
            {
                userDTO.FirstName = user.FirstName;
            }

            if (updateDTO.LastName.IsNullOrEmpty())
            {
                userDTO.LastName = user.LastName;
            }
            if (updateDTO.Gender == null)
            {
                userDTO.Gender = user.Gender;
            }
            if (updateDTO.Email.IsNullOrEmpty())
            {
                userDTO.Email = user.Email;
            }
            if (updateDTO.Address.IsNullOrEmpty())
            {
                userDTO.Address = user.Address;
            }
            if (updateDTO.ImageURL.IsNullOrEmpty())
            {
                userDTO.ImageURL = user.ImageURL;
            }
            if (updateDTO.PhoneNumber.IsNullOrEmpty())
            {
                userDTO.PhoneNumber = user.PhoneNumber;
            }
            if (updateDTO.ExperienceYears == null)
            {
                userDTO.ExperienceYears = user.ExperienceYears;
            }

            var userUpdate = await _userRepo.UpdateAsync(id, userDTO);
            return Ok(userUpdate);
        }
    }
}
