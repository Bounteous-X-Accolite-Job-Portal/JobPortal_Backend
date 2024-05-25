using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class InterviewFeedbackService : I_InterviewFeedbackService
    {
        private readonly ApplicationDbContext _context;
        private readonly I_InterviewService _interviewService;
        private readonly IDistributedCache _cache;
        public InterviewFeedbackService(ApplicationDbContext context, I_InterviewService interviewService, IDistributedCache cache)
        {
            _context = context;
            _interviewService = interviewService;
            _cache = cache; 
        }

        public async Task<InterviewFeedbackResponseViewModel> AddInterviewFeedback(CreateInterviewFeedbackViewModel interviewFeedback , Guid Empid)
        {
            var interview = await _interviewService.GetInterviewById((Guid)interviewFeedback.InterviewId); 

            InterviewFeedbackResponseViewModel response;
            if (interview.Interview == null)
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 402;
                response.Message = "Interview Not Exists ! ";
                return response;
            }
            
            if(interview.Interview.InterViewerId != Empid)
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 402;
                response.Message = " Currently Logged In Employee & Interviewer Mismatches !! ";
                return response;
            }

            InterviewFeedback newInterviewFeedback = new InterviewFeedback(interviewFeedback);
            await _context.InterviewFeedbacks.AddAsync(newInterviewFeedback);

            bool addedId = await _interviewService.UpdateFeedbackId((Guid)interviewFeedback.InterviewId, (Guid)newInterviewFeedback.FeedbackId);
            if(addedId == false)
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 500;
                response.Message = "Internal server error, please try again !";
                return response;
            }

            await _context .SaveChangesAsync();

            if(newInterviewFeedback!=null)
            {
                await _cache.RemoveAsync($"getAllInterviewFeedbackByEmployeeId-{newInterviewFeedback.EmployeeId}");

                response = new InterviewFeedbackResponseViewModel();
                response.Status = 200;
                response.Message = "Interview Feedback Successfully Added ! ! ";
            }
            else
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 403;
                response.Message = "Unable to Add Interview Feedback ! ";
            }

            return response;
        }

        public async Task<InterviewFeedbackResponseViewModel> DeleteInterviewFeedback(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            string key = $"getInterviewFeedbackById-{Id}";
            string? getInterviewFeedbackByIdFromCache = await _cache.GetStringAsync(key);

            InterviewFeedback interviewFeedback;
            if (string.IsNullOrWhiteSpace(getInterviewFeedbackByIdFromCache))
            {
                interviewFeedback = _context.InterviewFeedbacks.Find(Id);
                if (interviewFeedback == null)
                {
                    response.Status = 404;
                    response.Message = "Not Able to Found Interview Feedback";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(interviewFeedback));
            }
            else
            {
                interviewFeedback = JsonSerializer.Deserialize<InterviewFeedback>(getInterviewFeedbackByIdFromCache);
            }

            _context.InterviewFeedbacks.Remove(interviewFeedback);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"getInterviewFeedbackById-{Id}");
            await _cache.RemoveAsync($"getAllInterviewFeedbackByEmployeeId-{interviewFeedback.EmployeeId}");

            response.Status = 200;
            response.Message = "Interview Feedback Successfully Removed !";
            return response;
        }

        public async Task<InterviewFeedbackResponseViewModel> EditInterviewFeedback(EditInterviewFeedbackViewModel interviewFeedback , Guid EmpId)
        {
            var interview = await _interviewService.GetInterviewById((Guid)interviewFeedback.InterviewId);

            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();
            if (interview.Interview == null)
            {
                response.Status = 402;
                response.Message = "Interview Not Exists ! ";
                return response;
            }

            if (interview.Interview.InterViewerId != EmpId)
            {
                response.Status = 402;
                response.Message = " Currently Logged In Employee & Interviewer Mismatches !! ";
                return response;
            }

            string key = $"getInterviewFeedbackById-{interviewFeedback.FeedbackId}";
            string? getInterviewFeedbackByIdFromCache = await _cache.GetStringAsync(key);

            InterviewFeedback dbInterviewFeedback;
            if (string.IsNullOrWhiteSpace(getInterviewFeedbackByIdFromCache))
            {
                dbInterviewFeedback = _context.InterviewFeedbacks.Find(interviewFeedback.FeedbackId);
                if (dbInterviewFeedback == null)
                {
                    response.Status = 404;
                    response.Message = "Not Able to Found Interview Feedback";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbInterviewFeedback));
            }
            else
            {
                dbInterviewFeedback = JsonSerializer.Deserialize<InterviewFeedback>(getInterviewFeedbackByIdFromCache);
            }


            dbInterviewFeedback.Rating = interviewFeedback.Rating;
            dbInterviewFeedback.Feedback = interviewFeedback.Feedback;
            dbInterviewFeedback.AdditionalLink = interviewFeedback.AdditionalLink;

            _context.InterviewFeedbacks.Update(dbInterviewFeedback);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"getInterviewFeedbackById-{dbInterviewFeedback.FeedbackId}");
            await _cache.RemoveAsync($"getAllInterviewFeedbackByEmployeeId-{dbInterviewFeedback.EmployeeId}");

            response.Status = 200;
            response.Message = "Interview Feedback Successfully Updated ! ! ";
            response.interviewFeedback = new InterviewFeedbackViewModel(dbInterviewFeedback);
            return response;
        }

        public async Task<AllInterviewFeedbackResponseViewModel> GetAllInterviewFeedbacksByAEmployee(Guid EmployeeId)
        {
            string key = $"getAllInterviewFeedbackByEmployeeId-{EmployeeId}";
            string? getAllInterviewFeedbackByEmployeeIdFromCache = await _cache.GetStringAsync(key);

            List<InterviewFeedback> list;
            if (string.IsNullOrWhiteSpace(getAllInterviewFeedbackByEmployeeIdFromCache))
            {
                list = _context.InterviewFeedbacks.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<InterviewFeedback>>(getAllInterviewFeedbackByEmployeeIdFromCache);
            }

            List<InterviewFeedbackViewModel> interviewFeedbackList = new List<InterviewFeedbackViewModel>();
            foreach (var item in list)
            {
                if(item.EmployeeId == EmployeeId)
                {
                    interviewFeedbackList.Add(new InterviewFeedbackViewModel(item));
                }
            }   

            AllInterviewFeedbackResponseViewModel response = new AllInterviewFeedbackResponseViewModel();
            response.Status = 200;
            response.interviewFeedbacks = interviewFeedbackList;
            if(list.Count > 0) 
                response.Message = "Successfully Reterived All Interviews Feedbacks !";
            else
                response.Message = "No Interviews Feedbacks Exists!";

            return response;
        }

        public async Task<InterviewFeedbackResponseViewModel> GetInterviewFeedbackById(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            string key = $"getInterviewFeedbackById-{Id}";
            string? getInterviewFeedbackByIdFromCache = await _cache.GetStringAsync(key);

            InterviewFeedback dbInterviewFeedback;
            if (string.IsNullOrWhiteSpace(getInterviewFeedbackByIdFromCache))
            {
                dbInterviewFeedback = _context.InterviewFeedbacks.Find(Id);
                if (dbInterviewFeedback == null)
                {
                    response.Status = 404;
                    response.Message = "Not Able to Found Interview Feedback";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbInterviewFeedback));
            }
            else
            {
                dbInterviewFeedback = JsonSerializer.Deserialize<InterviewFeedback>(getInterviewFeedbackByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully Found Interview Feedback";
            response.interviewFeedback = new InterviewFeedbackViewModel(dbInterviewFeedback);
            return response;
        }
    }
}
