using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using KoiFishCare.Mappers;
using KoiFishCare.DTOs.User;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ItokenService _tokenService;

        private readonly IUserRepository _userRepo;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager,
        ItokenService tokenService, IUserRepository userRepo, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
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
                var userRoles = await _userManager.GetRolesAsync(user);
                var userRole = userRoles.FirstOrDefault();
                var role = await _roleManager.FindByNameAsync(userRole!);
                var customer = user as Customer;
                var userDto = new UserDTO
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = _tokenService.CreateToken(customer, role)
                };

                return Ok(userDto);
            }
            return Unauthorized("Invalid login");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
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
                var roleResult = await _userManager.AddToRoleAsync(customer, "Customer");
                if (roleResult.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync("Customer");
                    var userDto = new UserDTO
                    {
                        UserName = customer.UserName,
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Token = _tokenService.CreateToken(customer, role)
                    };
                    return Ok(userDto);
                }
            }
            return BadRequest(result.Errors);
        }

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
