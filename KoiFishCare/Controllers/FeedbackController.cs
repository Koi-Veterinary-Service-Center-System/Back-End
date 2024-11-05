using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly UserManager<User> _userManager;
        private readonly IBookingRepository _bookingRepo;
        public FeedbackController(IFeedbackRepository feedbackRepo, UserManager<User> userManager, IBookingRepository bookingRepo)
        {
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
            _bookingRepo = bookingRepo;
        }

        [HttpGet("all-feedback")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> GetAllFeedback()
        {
            var feedbacks = await _feedbackRepo.GetAllFeedbackAsync();
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("Can not find any feedback!");
            }

            var result = feedbacks.Select(x => x.ToViewFeedback()).ToList();
            return Ok(result);
        }

        [HttpGet("view-feedback/{bookingID}")]
        [Authorize(Roles = "Vet, Staff")]
        public async Task<IActionResult> GetFeedbackByBookingID([FromRoute] int bookingID)
        {
            var feedback = await _feedbackRepo.GetFeedbackByBookingIdAsync(bookingID);
            if (feedback == null)
            {
                return NotFound("Can not find any feedback!");
            }

            return Ok(feedback.ToViewFeedback());
        }

        [HttpGet("view-feedback-username/{userName}")]
        [Authorize]
        public async Task<IActionResult> GetFeedbackByUserName([FromRoute] string userName)
        {
            var feedbacks = await _feedbackRepo.GetAllFeedbackByUserNameAsync(userName);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("Can not find any feedback!");
            }

            var result = feedbacks.Select(x => x.ToViewFeedback());
            return Ok(result);
        }

        [HttpPost("create-feedback/{bookingID}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateFeedback([FromRoute] int bookingID, [FromBody] AddFeedbackDTO addFeedbackDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(this.User);
            if (user == null) return NotFound("Not found user");
            if (string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest("Could not determine the logged-in user's username.");
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingID);
            if (booking == null)
            {
                return NotFound("Can not find this booking with succeed status");
            }

            else if (booking.Customer.UserName != user.UserName)
            {
                return Unauthorized("You can only give feedback on your own booking!");
            }


            var existingFeedback = await _feedbackRepo.GetFeedbackByUserNameAndIdAsync(booking.BookingID, user.UserName);
            if (existingFeedback != null)
            {
                return BadRequest("You have already submitted feedback for this booking.");
            }


            if (addFeedbackDTO.Rate < 1 || addFeedbackDTO.Rate > 5)
            {
                return BadRequest("Rate must between 1-5!");
            }

            var feedbackModel = addFeedbackDTO.ToModelFromDTO();
            feedbackModel.CustomerName = user.UserName;
            feedbackModel.BookingID = booking.BookingID;
            var feedback = await _feedbackRepo.CreateFeedbackAsync(feedbackModel);
            if (feedback == null)
            {
                return NotFound("Can not find bookingID");
            }

            booking.hasFeedback = true;
            await _bookingRepo.UpdateBooking(booking);
            return Ok(feedback.ToViewFeedback());
        }

        [HttpDelete("delete-feedback/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id)
        {
            var feedback = await _feedbackRepo.DeleteFeedback(id);
            if (feedback == null)
            {
                return NotFound("Can not find any feedback!");
            }
            return Ok("Deleted successfully");
        }

        [HttpPut("show-hide-feedback/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateIsVisible(int id, bool isVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = await _feedbackRepo.UpdateFeedbackStatus(id, isVisible);
            if (feedback == null)
            {
                return NotFound("Can not find any feedback!");
            }
            return Ok(feedback.ToViewFeedback());
        }

        [HttpGet("view-hidden-feedback")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFeedbackIsHidden()
        {
            var feedbacks = await _feedbackRepo.GetAllFeedbackIsHidden();
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("There is no hidden feedback!");
            }

            var result = feedbacks.Select(x => x.ToViewFeedback()).ToList();
            return Ok(result);
        }

        [HttpGet("view-feedback-by/{serviceID}")]
        public async Task<IActionResult> GetFeedbackByServiceID([FromRoute] int serviceID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedbacks = await _feedbackRepo.GetFeedbackByServiceIDAsync(serviceID);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("There is no feedback of this service!");
            }

            return Ok(feedbacks.Select(x => x.ToViewFeedback()));
        }

        [HttpGet("feedback/{vetID}")]
        public async Task<IActionResult> GetFeedbackByVetID([FromRoute] string vetID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedbacks = await _feedbackRepo.GetFeedbackByVetIDAsync(vetID);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("There is no feedback of this Vet!");
            }

            return Ok(feedbacks.Select(x => x.ToViewFeedback()));
        }
    }
}