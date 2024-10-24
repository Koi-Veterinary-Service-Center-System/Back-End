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
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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

            if(model.UserName == null) return NotFound("Not found user name");
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid login");
            }

            if (user.isBanned)
            {
                return Unauthorized("You are banned!");
            }

            if(model.Password == null) return NotFound("Not found password");
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
            var redirectUrl = Url.Action("GoogleResponse", "User"); // No page required, this is an API endpoint
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme); // Redirects to Google for login
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (result.Succeeded && result.Principal != null)
            {
                // Extract user info from the claims
                var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
                if(email == null) return NotFound("Not found email");

                // Check if the user exists
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // User does not exist; redirect with an error message
                    return Redirect($"http://localhost:5173/?error=UserNotFound");
                }

                // Generate JWT token for the authenticated user
                var userRoles = await _userManager.GetRolesAsync(user);
                var userRole = userRoles.FirstOrDefault();
                if(userRole == null) return NotFound("Not found user role");
                var role = await _roleManager.FindByNameAsync(userRole);
                if(role == null) return NotFound("Not found role");
                var token = _tokenService.CreateToken(user, role);

                // Redirect to the frontend with token and username as query params
                return Redirect($"http://localhost:5173/?token={token}&username={user.UserName}");
            }

            // If the authentication failed, redirect to the frontend with an error
            return Redirect($"http://localhost:5173/?error=GoogleLoginFailed");
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.UserName!.Length < 3)
            {
                return BadRequest("User name length must be more than 3!");
            }

            if (!IsValidEmail(model.Email!))
            {
                return BadRequest("Invalid email!");
            }

            if (await IsEmailExistedNoId(model.Email!))
            {
                return BadRequest("This email is already existed!");
            }

            var customer = new Customer
            {
                FirstName = model.FirstName!,
                LastName = model.LastName!,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(customer, model.Password!);
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
                        Token = _tokenService.CreateToken(customer, role!)
                    };
                    return Ok(userDto);
                }
            }
            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ViewProfile()
        {
            var id = _userManager.GetUserId(this.User);
            var user = await _userManager.FindByIdAsync(id!);
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

            if (await IsUserNameExisted(id, updateDTO.UserName))
            {
                return BadRequest("This user name is already existed!");
            }

            if (!IsValidEmail(updateDTO.Email!))
            {
                return BadRequest("Invalid email!");
            }

            if (await IsEmailExisted(id, updateDTO.Email!))
            {
                return BadRequest("This email is already existed!");
            }

            if (!IsValidPhoneNumber(updateDTO.PhoneNumber))
            {
                return BadRequest("Phone number must not contain characters!");
            }

            if (await IsPhoneNumberExisted(id, updateDTO.PhoneNumber))
            {
                return BadRequest("This phone number is already existed!");
            }

            user.UserName = string.IsNullOrEmpty(updateDTO.UserName) ? user.UserName : updateDTO.UserName;
            user.FirstName = string.IsNullOrEmpty(updateDTO.FirstName) ? user.FirstName : updateDTO.FirstName;
            user.LastName = string.IsNullOrEmpty(updateDTO.LastName) ? user.LastName : updateDTO.LastName;
            user.Gender = updateDTO.Gender ?? user.Gender;
            user.Email = string.IsNullOrEmpty(updateDTO.Email) ? user.Email : updateDTO.Email;
            user.Address = string.IsNullOrEmpty(updateDTO.Address) ? user.Address : updateDTO.Address;
            user.ImageURL = string.IsNullOrEmpty(updateDTO.ImageURL) ? user.ImageURL : updateDTO.ImageURL;
            user.PhoneNumber = string.IsNullOrEmpty(updateDTO.PhoneNumber) ? user.PhoneNumber : updateDTO.PhoneNumber;
            user.ExperienceYears = updateDTO.ExperienceYears ?? updateDTO.ExperienceYears;

            var userUpdate = await _userManager.UpdateAsync(user);
            if (userUpdate.Succeeded)
            {
                return Ok(await user.ToUserProfileFromUser(_userManager));
            }
            return BadRequest("User update failed");
        }

        [HttpPost("create-user")]
        [Authorize(Roles = "Manager")]
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

            if (!IsValidEmail(userDTO.Email!))
            {
                return BadRequest("Invalid email!");
            }

            if (await IsEmailExistedNoId(userDTO.Email!))
            {
                return BadRequest("This email is already existed!");
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


        [HttpDelete("delete-user/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return Ok($"Account deleted successfully for {user.UserName}");
        }

        [HttpGet("all-user")]
        [Authorize(Roles = "Manager")]
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

        [HttpPatch("update-role-user/{id}/{role}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateRoleUser([FromRoute] string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Can not find this user!");
            }

            if (!role.Equals("Vet") && !role.Equals("Staff") && !role.Equals("Manager"))
            {
                return BadRequest("This role is not available!");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            await _userManager.AddToRoleAsync(user, role);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
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

        [HttpPatch("ban-user/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BanUser([FromRoute] string id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("Can not find user!");
            }

            existingUser.isBanned = true;
            await _userRepo.UpdateAsync(existingUser);
            return Ok("Banned successfully!");
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

        private async Task<bool> IsPhoneNumberExisted(string id, string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(x => x.Id != id && x.PhoneNumber == phoneNumber);
        }

        private async Task<bool> IsEmailExisted(string? id, string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Id != id && x.Email == email);
        }

        private async Task<bool> IsEmailExistedNoId(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> IsUserNameExisted(string id, string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.Id != id && x.UserName == userName);
        }

    }
}
