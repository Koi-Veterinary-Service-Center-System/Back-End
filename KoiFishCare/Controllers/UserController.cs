using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KoiFishCare.Models;
using KoiFishCare.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Authorization;
using KoiFishCare.Mappers;
using KoiFishCare.DTOs.User;
using KoiFishCare.Dtos.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

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
        private readonly IStaffRepository _staffRepo;
        private readonly IVetRepository _vetRepo;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager,
        ItokenService tokenService, IUserRepository userRepo, RoleManager<IdentityRole> roleManager, IStaffRepository staffRepo, IVetRepository vetRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _roleManager = roleManager;
            _staffRepo = staffRepo;
            _vetRepo = vetRepo;
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
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid login");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            if (userRole == null)
            {
                return Unauthorized("User has no assigned roles");
            }
            var role = await _roleManager.FindByNameAsync(userRole);
            if (role == null)
            {
                return BadRequest("Role not found");
            }

            var token = _tokenService.CreateToken(user, role);
            var userDto = new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };

            var welcomeMessage = $"Welcome {user.UserName} ({userRole})";

            return Ok(new { Token = userDto.Token, Message = welcomeMessage });
        }

[HttpGet("google-login")]
public IActionResult GoogleLogin()
{
    var redirectUrl = Url.Action("GoogleResponse", "User");
    var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
}

[HttpGet("google-response")]
public async Task<IActionResult> GoogleResponse()
{
    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

    if (result.Succeeded && result.Principal != null)
    {
        // Extract user info from the claims
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

        // Check if the user exists, if not, create them
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = result.Principal.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = result.Principal.FindFirst(ClaimTypes.Surname)?.Value
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "Customer"); // Assign role if necessary
        }

        // Generate JWT token
        var userRoles = await _userManager.GetRolesAsync(user);
        var userRole = userRoles.FirstOrDefault();
        var role = await _roleManager.FindByNameAsync(userRole);
        var token = _tokenService.CreateToken(user, role);

        var userDto = new UserDTO
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token // Return the JWT token
        };

        return Ok(new { Token = token, Message = $"Welcome {user.UserName}" }); // Return the user information and token
    }

    return BadRequest("Google login failed.");
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

            var dto = await user.ToUserProfileFromUser(_userManager);
            return Ok(dto);
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

            if (updateDTO.UserName.Length < 3)
            {
                return BadRequest("User name length must be more than 3!");
            }

            if (!IsValidEmail(updateDTO.Email))
            {
                return BadRequest("Invalid email!");
            }

            if (!IsValidPhoneNumber(updateDTO.PhoneNumber))
            {
                return BadRequest("Phone number must not contain characters!");
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
            var dto = await userUpdate.ToUserProfileFromUser(_userManager);
            return Ok(dto);
        }



        [HttpPost("create-user")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userDTO.UserName.Length < 3)
            {
                return BadRequest("User name length must be more than 3!");
            }

            if (!IsValidEmail(userDTO.Email))
            {
                return BadRequest("Invalid email!");
            }

            if (!userDTO.Role.Equals("Vet") && !userDTO.Role.Equals("Staff") && !userDTO.Role.Equals("Manager"))
            {
                return BadRequest("This role is not available!");
            }

            if (userDTO.Role.Equals("Staff"))
            {
                var user = new Staff
                {
                    UserName = userDTO.UserName,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Gender = userDTO.Gender,
                    ManagerID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    IsManager = userDTO.Role.Equals("Manager"),
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var role = await _userManager.AddToRoleAsync(user, userDTO.Role);
                if (!role.Succeeded)
                {
                    return BadRequest(role.Errors);
                }
            }

            else if (userDTO.Role.Equals("Manager"))
            {
                var user = new Staff
                {
                    UserName = userDTO.UserName,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Gender = userDTO.Gender,
                    IsManager = userDTO.Role.Equals("Manager"),
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var role = await _userManager.AddToRoleAsync(user, userDTO.Role);
                if (!role.Succeeded)
                {
                    return BadRequest(role.Errors);
                }
            }

            else
            {
                var user = new Veterinarian
                {
                    UserName = userDTO.UserName,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Gender = userDTO.Gender,
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                var role = await _userManager.AddToRoleAsync(user, userDTO.Role);
                if (!role.Succeeded)
                {
                    return BadRequest(role.Errors);
                }
            }

            return Ok($"Account created successfully for {userDTO.Role}: {userDTO.UserName}");
        }


        [HttpDelete("delete-user")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return Ok($"Account deleted successfully for {user.UserName}");
        }

        [HttpGet("all-user")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userRepo.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return NotFound("There is no user!");
            }

            var userDTOs = new List<ViewUserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOs.Add(new ViewUserDTO
                {
                    UserID = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.FirstOrDefault(),
                    Gender = user.Gender,
                    UserName = user.UserName,
                    Email = user.Email,
                });
            }

            return Ok(userDTOs);
        }

        [HttpPut("update-user")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateUser(string userID, string role)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new ViewUserDTO()
            {
                UserID = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                Gender = user.Gender,
                Email = user.Email,
            };

            return Ok(userDTO);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^[0-9\-]+$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
