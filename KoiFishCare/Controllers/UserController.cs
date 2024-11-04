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
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ItokenService _tokenService;

        private readonly IUserRepository _userRepo;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStaffRepository _staffRepo;
        private readonly IVetRepository _vetRepo;
        private readonly IEmailService _emailService;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager,
        ItokenService tokenService, IUserRepository userRepo, RoleManager<IdentityRole> roleManager, IStaffRepository staffRepo, IVetRepository vetRepo, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _roleManager = roleManager;
            _staffRepo = staffRepo;
            _vetRepo = vetRepo;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.UserName == null) return NotFound("Not found user name");
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username or password. Please try again!");
            }

            if (user.IsBanned)
            {
                return Unauthorized("You are banned!");
            }

            if (model.Password == null) return NotFound("Not found password");
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password. Please try again!");
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

        // [HttpGet("google-login")]
        // public IActionResult GoogleLogin()
        // {
        //     var redirectUrl = Url.Action("GoogleResponse", "User"); // No page required, this is an API endpoint
        //     var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        //     return Challenge(properties, GoogleDefaults.AuthenticationScheme); // Redirects to Google for login
        // }

        // [HttpGet("google-response")]
        // public async Task<IActionResult> GoogleResponse()
        // {
        //     var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //     if (result.Succeeded && result.Principal != null)
        //     {
        //         // Extract user info from the claims
        //         var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        //         if (email == null) return NotFound("Not found email");

        //         // Check if the user exists
        //         var user = await _userManager.FindByEmailAsync(email);
        //         if (user == null)
        //         {
        //             // User does not exist; redirect with an error message
        //             return Redirect($"http://localhost:5173/?error=UserNotFound");
        //         }

        //         // Generate JWT token for the authenticated user
        //         var userRoles = await _userManager.GetRolesAsync(user);
        //         var userRole = userRoles.FirstOrDefault();
        //         if (userRole == null) return NotFound("Not found user role");
        //         var role = await _roleManager.FindByNameAsync(userRole);
        //         if (role == null) return NotFound("Not found role");
        //         var token = _tokenService.CreateToken(user, role);

        //         // Redirect to the frontend with token and username as query params
        //         return Redirect($"http://localhost:5173/?token={token}&username={user.UserName}");
        //     }

        //     // If the authentication failed, redirect to the frontend with an error
        //     return Redirect($"http://localhost:5173/?error=GoogleLoginFailed");
        // }




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

            var customer = new User
            {
                FirstName = model.FirstName!,
                LastName = model.LastName!,
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = false // Set as unverified initially
            };

            var result = await _userManager.CreateAsync(customer, model.Password!);
            if (result.Succeeded)
            {
                // Generate verification URL
                //var verifyUrl = $"http://localhost:5173/verify-email?email={customer.Email}";

                // Generate a random 6-digit verification code
                var verificationCode = GenerateRandomCode();

                // Generate verification URL
                var verifyUrl = $"http://localhost:5173/verify-email?email={customer.Email}";

                var htmlContent = $@"
        <html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
      <div style='text-align: center;'>
        <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #4A90E2; font-size: 24px; margin: 0;'>Welcome to KoiNe!</h2>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6; margin-top: 20px;'>Hello {customer.FirstName},</p>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>Thank you for registering with us! To complete your registration, please verify your email using the code below:</p>
      
      <div style='text-align: center; margin: 20px 0;'>
        <span style='display: inline-block; padding: 10px 20px; background-color: #4A90E2; color: #ffffff; font-size: 24px; font-weight: bold; border-radius: 5px;'>{verificationCode}</span>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>Please enter this code on the verification page to activate your account.</p>
      
      <hr style='border: none; border-top: 1px solid #eeeeee; margin: 20px 0;' />
      
      <p style='color: #777777; font-size: 14px; text-align: center;'>If you did not create this account, please disregard this email.</p>
      
      <div style='text-align: center; margin-top: 30px;'>
        <p style='color: #333333; font-size: 16px; margin: 0;'>Best regards,</p>
        <p style='color: #4A90E2; font-size: 16px; font-weight: bold; margin: 5px 0;'>KoiNe Team</p>
      </div>
    </div>
  </body>
</html>";

                await _emailService.SendEmailAsync(customer.Email!, "Verify Your Email", htmlContent);
                return Ok("Registration successful. Please check your email to verify your account.");
            }
            return BadRequest(result.Errors);
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit code
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDTO verificationDto)
        {
            var user = await _userManager.FindByEmailAsync(verificationDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid verification link.");
            }

            if (verificationDto.Code.Length != 6 || !int.TryParse(verificationDto.Code, out _))
            {
                return BadRequest("Invalid verification code.");
            }

            string originalCode = verificationDto.Code; // This should be replaced with your actual logic to fetch the sent code

            // Verify the code against the expected original code (you might need to adjust how you manage this)
            if (verificationDto.Code != originalCode)
            {
                return BadRequest("Verification code does not match.");
            }

            // Update the user's email confirmation status
            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest("Failed to confirm email.");
            }

            // Assign "Customer" role and generate JWT token upon verification
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (roleResult.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("Customer");
                var userDto = new UserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = _tokenService.CreateToken(user, role!)
                };

                // Send final welcome email
                var welcomeContent = $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; background-color: #fff; border-radius: 10px; padding: 20px;'>
                <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' 
                     style='display: block; margin: 0 auto; width: 150px;' />
                <h2 style='text-align: center;'>Welcome to KoiNe!</h2>
                <p>Hello {user.FirstName},</p>
                <p>Your email has been verified successfully. Thank you for joining us!</p>
                <p>Best regards,<br>KoiNe Team</p>
            </div>
        </body>
        </html>";

                await _emailService.SendEmailAsync(user.Email!, "Welcome to KoiNe!", welcomeContent);

                return Ok(userDto);
            }
            return BadRequest("An error occurred while assigning the role.");
        }



        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordRequestDTO model)
        {
            // Tìm kiếm người dùng dựa trên email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User with this email does not exist.");
            }

            // Tạo link reset mật khẩu (có thể truyền thêm token hoặc email nếu cần)
            var resetLink = $"http://localhost:5173/reset-password?email={model.Email}";

            // Nội dung HTML cho email (có logo và nút reset password)
            var htmlContent = $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; background-color: #fff; border-radius: 10px; padding: 20px;'>
                <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' 
                     style='display: block; margin: 0 auto; width: 150px;' />
                <h2 style='text-align: center;'>Password Reset Request</h2>
                <p>Hello,</p>
                <p>You requested a password reset. Please click the button below to reset your password:</p>
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='{resetLink}' 
                       style='padding: 10px 20px; background-color: #00BFFF; color: black; 
                              text-decoration: none; border-radius: 5px;'>Reset Password</a>
                </div>
                <p>If you did not request this, please ignore this email.</p>
                <p>Best regards,<br>KoiNe Team</p>
                <hr style='margin-top: 20px;' />
                <p style='font-size: 12px; text-align: center; color: #777;'>&copy; 2024 KoiNe. All rights reserved.</p>
            </div>
        </body>
        </html>";

            // Gửi email qua service EmailService
            await _emailService.SendEmailAsync(model.Email, "Password Reset", htmlContent);

            return Ok("Password reset link has been sent to your email.");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User with this email does not exist.");
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove old password.");
            }

            result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to set new password.");
            }

            return Ok("Password reset successfully.");
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

            if (userDTO.Role.Equals("Staff") || userDTO.Role.Equals("Vet"))
            {
                var user = new User
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
                var user = new User
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
                    IsActive = user.IsActive,
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

        [HttpPatch("soft-delete/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SoftDeleteUser([FromRoute] string id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("Can not find user!");
            }

            existingUser.IsDeleted = true;
            existingUser.IsActive = false;
            await _userRepo.UpdateAsync(existingUser);
            return Ok("Deleted successfully!");
        }

        [HttpPatch("ban-user/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> BanUser([FromRoute] string id, [FromBody] ReasonDTO model)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("Can not find user!");
            }

            existingUser.IsBanned = true;
            existingUser.IsActive = false;
            await _userRepo.UpdateAsync(existingUser);

            // Compose and send an email notification to the banned user
            var subject = "Account Banned Notification";
            var htmlContent = $@"
    <html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
      <div style='text-align: center;'>
        <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #e63946; font-size: 24px; margin: 0;'>Account Banned</h2>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6; margin-top: 20px;'>Dear {existingUser.UserName},</p>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>We regret to inform you that your account has been banned for the following reason:</p>
      
      <div style='margin: 20px 0; padding: 15px; background-color: #fee2e2; border-left: 4px solid #e63946; border-radius: 5px;'>
        <p style='color: #e63946; font-size: 16px; font-weight: bold; margin: 0;'>{model.Reason}</p>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>If you have any questions or believe this action was taken in error, please feel free to <a href='mailto:support@KoiNe.com' style='color: #1d72b8; text-decoration: none;'>contact us</a>.</p>
      
      <hr style='border: none; border-top: 1px solid #eeeeee; margin: 20px 0;' />
      
      <p style='color: #777777; font-size: 12px; text-align: center;'>Best regards,<br><strong style='color: #333;'>KoiNe Team</strong></p>
      
      <p style='font-size: 12px; text-align: center; color: #777777; margin-top: 20px;'>&copy; 2024 KoiNe. All rights reserved.</p>
    </div>
  </body>
</html>";

            await _emailService.SendEmailAsync(existingUser.Email!, subject, htmlContent);
            return Ok("Banned successfully!");
        }

        [HttpPatch("unban-user/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UnBanUser([FromRoute] string id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("Can not find user!");
            }

            existingUser.IsBanned = false;
            existingUser.IsActive = true;
            await _userRepo.UpdateAsync(existingUser);

            // Compose and send an email notification to the banned user
            var subject = "Account UnBan Notification";
            var htmlContent = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
      <div style='text-align: center;'>
        <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #4A90E2; font-size: 24px; margin: 0;'>Account Unbanned</h2>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6; margin-top: 20px;'>Dear {existingUser.UserName},</p>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>We are pleased to inform you that your account with KoiNe has been successfully unbanned. You can now log in and enjoy all the services as usual.</p>
      
      <div style='margin: 20px 0; padding: 15px; background-color: #e6f7ff; border-left: 4px solid #4A90E2; border-radius: 5px;'>
        <p style='color: #4A90E2; font-size: 16px; font-weight: bold; margin: 0;'>Welcome back to the KoiNe community!</p>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>If you have any questions or need further assistance, please don't hesitate to <a href='mailto:support@KoiNe.com' style='color: #1d72b8; text-decoration: none;'>contact our support team</a>.</p>
      
      <hr style='border: none; border-top: 1px solid #eeeeee; margin: 20px 0;' />
      
      <p style='color: #777777; font-size: 12px; text-align: center;'>Thank you for being a valued member of our community. We look forward to serving you!</p>
      
      <p style='font-size: 12px; text-align: center; color: #777777; margin-top: 20px;'>&copy; 2024 KoiNe. All rights reserved.</p>
    </div>
  </body>
</html>";

            await _emailService.SendEmailAsync(existingUser.Email!, subject, htmlContent);
            return Ok("UnBanned successfully!");
        }

        [HttpGet("get-all-Customer")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var users = await _userRepo.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return NotFound("There is no user!");
            }

            var cusDTOs = new List<ViewUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                if (role != null && role.Equals("Customer"))
                {
                    cusDTOs.Add(new ViewUserDTO
                    {
                        UserID = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = role,
                        Gender = user.Gender,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                    });
                }
            }

            if (!cusDTOs.Any())
                return NotFound("No customers found!");


            return Ok(cusDTOs);
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
