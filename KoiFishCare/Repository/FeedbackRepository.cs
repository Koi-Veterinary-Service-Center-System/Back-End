using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;

        public FeedbackRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<Feedback?> CreateFeedbackAsync(Feedback feedbackModel)
        {
            var CheckBookingStatus = await _context.Bookings.Where(b => b.BookingStatus == BookingStatus.Succeeded).FirstOrDefaultAsync(b => b.BookingID == feedbackModel.BookingID);
            if (CheckBookingStatus == null)
            {
                return null;
            }

            var feedback = await _context.Feedbacks.AddAsync(feedbackModel);
            await _context.SaveChangesAsync();
            return feedbackModel;
        }

        public async Task<Feedback?> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackID == id);
            if (feedback == null)
            {
                return null;
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<List<Feedback>?> GetAllFeedbackAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<List<Feedback>?> GetAllFeedbackByUserNameAsync(string userName)
        {
            return await _context.Feedbacks.Where(c => c.Booking.Customer.UserName == userName).ToListAsync();
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(x => x.FeedbackID == id);
        }

        public async Task<Feedback?> UpdateFeedbackStatus(int id, bool isVisible)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(x => x.FeedbackID == id);
            if (feedback == null)
            {
                return null;
            }

            feedback.IsVisible = isVisible;
            _context.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<List<Feedback>?> GetAllFeedbackIsHidden()
        {
            return await _context.Feedbacks.Where(x => x.IsVisible == false).ToListAsync();
        }


    }
}