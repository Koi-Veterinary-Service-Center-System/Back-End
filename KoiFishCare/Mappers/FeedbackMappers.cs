using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Distance;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.DTOs;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class FeedbackMappers
    {
        public static Feedback ToModelFromDTO(this AddFeedbackDTO addFeedbackDTO)
        {
            return new Feedback
            {
                Rate = addFeedbackDTO.Rate,
                Comments = addFeedbackDTO.Comments,
                IsVisible = true
            };
        }

        public static FeedbackDTO ToViewFeedback(this Feedback feedbackModel)
        {
            return new FeedbackDTO
            {
                FeedbackID = feedbackModel.FeedbackID,
                BookingID = feedbackModel.BookingID,
                ServiceName = feedbackModel.Booking.Service.ServiceName,
                CustomerName = feedbackModel.CustomerName,
                Rate = feedbackModel.Rate,
                Comments = feedbackModel.Comments,
                IsVisible = feedbackModel.IsVisible,
            };
        }
    }
}