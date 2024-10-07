using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>?> GetAllFeedbackAsync();
        Task<Feedback?> GetFeedbackByIdAsync(int id);
        Task<List<Feedback>?> GetAllFeedbackByUserNameAsync(string userName);
        Task<Feedback?> CreateFeedbackAsync(Feedback feedbackModel);
        Task<Feedback?> DeleteFeedback(int id);
        Task<Feedback?> UpdateFeedbackStatus(int id, bool isVisible);
       Task<List<Feedback>?> GetAllFeedbackIsHidden();

    }
}