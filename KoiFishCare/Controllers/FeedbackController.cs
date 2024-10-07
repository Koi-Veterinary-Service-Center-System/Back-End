using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
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
        public FeedbackController(IFeedbackRepository feedbackRepo, UserManager<User> userManager)
        {
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
        }

        [HttpGet("all-feedback")]
        // [Authorize(Roles = "Staff")]
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

        [HttpGet("view-feedback-id/{id}")]
        public async Task<IActionResult> GetFeedbackById([FromRoute] int id)
        {
            var feedback = await _feedbackRepo.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound("Can not find any feedback!");
            }

            return Ok(feedback.ToViewFeedback());
        }

        [HttpGet("view-feedback-username/{userName}")]
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

        [HttpPost("create-feedback")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateFeedback(AddFeedbackDTO addFeedbackDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _userManager.GetUserId(this.User);
            var user = await _userManager.FindByIdAsync(id);
            if (string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest("Could not determine the logged-in user's username.");
            }

            var feedbackModel = addFeedbackDTO.ToModelFromDTO();
            feedbackModel.CustomerName = user.UserName;
            var feedback = await _feedbackRepo.CreateFeedbackAsync(feedbackModel);
            if (feedback == null)
            {
                return NotFound("Can not find bookingID");
            }


            return Ok(feedback.ToViewFeedback());
        }

        [HttpDelete("delete-feedback/{id}")]
        // [Authorize(Roles = "Manager")]
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
        // [Authorize(Roles = "Staff")]
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
        // [Authorize(Roles = "Manager")]
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
    }
}