using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class InterviewService : I_InterviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeAccountService _employeeAccountService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IDistributedCache _cache;
        public InterviewService(
            ApplicationDbContext context, 
            IEmployeeAccountService employeeAccountService,
            IDistributedCache cache,
            IJobApplicationService jobApplicationService
        )
        {
            _context = context;
            _employeeAccountService = employeeAccountService;
            _cache = cache; 
            _jobApplicationService = jobApplicationService;
        }

        public async Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview , Guid EmpId)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            var application = await _jobApplicationService.GetJobApplicaionById((Guid)interview.ApplicationId);
            if(application.Application == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            var interviewer = await _employeeAccountService.GetEmployeeById((Guid)interview.InterViewerId); 
            if (interviewer.Employee == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            Interview newInterview = new Interview();
            newInterview.ApplicationId = interview.ApplicationId;
            newInterview.InterViewerId = interview.InterViewerId;
            newInterview.InterviewDate = interview.InterviewDate;
            newInterview.InterviewTime = interview.InterviewTime;
            newInterview.Link = interview.Link;
            newInterview.EmpId = EmpId;

            await _context.Interviews.AddAsync(newInterview);
            await _context.SaveChangesAsync();

            if(newInterview == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add Interview !!";
            }
            else
            {
                await _cache.RemoveAsync($"allInterviews");
                await _cache.RemoveAsync($"getAllInterviewsByApplicationId-{interview.ApplicationId}");
                await _cache.RemoveAsync($"getAllInterviewsByInterviewerId-{interview.InterViewerId}");

                response.Status = 200;
                response.Message = "Interview Scheduled Successfully !!";
                response.Interview = new InterviewViewModel(newInterview);
            }
            return response;
        }

        public async Task<InterviewResponseViewModel> DeleteInterview(Guid Id)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            string key = $"getInterviewById-{Id}";
            string? getInterviewByIdFromCache = await _cache.GetStringAsync(key);

            Interview interview;
            if (string.IsNullOrWhiteSpace(getInterviewByIdFromCache))
            {
                interview = _context.Interviews.Find(Id);
                if (interview == null)
                {
                    response.Status = 500;
                    response.Message = "Not Able to Found Interview you are trying to remove";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(interview));
            }
            else
            {
                interview = JsonSerializer.Deserialize<Interview>(getInterviewByIdFromCache);
            }

            _context.Interviews.Remove(interview);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allInterviews");
            await _cache.RemoveAsync($"getAllInterviewsByApplicationId-{interview.ApplicationId}");
            await _cache.RemoveAsync($"getInterviewById-{interview.InterviewId}");
            await _cache.RemoveAsync($"getAllInterviewsByInterviewerId-{interview.InterViewerId}");

            response.Status = 200;
            response.Message = "Interview Successfully Removed !";
            return response;
        }

        public async Task<InterviewResponseViewModel> EditInterview(EditInterviewViewModel interview)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            var application = await _jobApplicationService.GetJobApplicaionById((Guid)interview.ApplicationId);
            if (application.Application == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            var interviewer = await _employeeAccountService.GetEmployeeById((Guid)interview.InterViewerId);
            if (interviewer.Employee == null)
            {
                response.Status = 404;
                response.Message = "The interviewer does not exist.";
                return response;
            }

            string key = $"getInterviewById-{interview.InterviewId}";
            string? getInterviewByIdFromCache = await _cache.GetStringAsync(key);

            Interview dbinterview;
            if (string.IsNullOrWhiteSpace(getInterviewByIdFromCache))
            {
                dbinterview = _context.Interviews.Find(interview.InterviewId);
                if (dbinterview == null)
                {
                    response.Status = 500;
                    response.Message = "Not Able to Found Interview you are trying to update";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbinterview));
            }
            else
            {
                dbinterview = JsonSerializer.Deserialize<Interview>(getInterviewByIdFromCache);
            }

            dbinterview.InterviewDate = interview.InterviewDate;
            dbinterview.InterviewTime = interview.InterviewTime;
            dbinterview.InterViewerId = interview.InterViewerId;
            dbinterview.Link = interview.Link;
            dbinterview.FeedbackId = interview.FeedbackId;

            _context.Interviews.Update(dbinterview);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allInterviews");
            await _cache.RemoveAsync($"getAllInterviewsByApplicationId-{dbinterview.ApplicationId}");
            await _cache.RemoveAsync($"getInterviewById-{dbinterview.InterviewId}");
            await _cache.RemoveAsync($"getAllInterviewsByInterviewerId-{dbinterview.InterViewerId}");

            response.Status = 200;
            response.Message = "Interview Successfully Updated !";
            response.Interview = new InterviewViewModel(dbinterview);
            return response;
        }

        public async Task<All_InterviewResponseViewModel> GetAllInterviews()
        {
            string key = $"allInterviews";
            string? getAllInterviewsFromCache = await _cache.GetStringAsync(key);

            List<Interview> list;
            if (string.IsNullOrWhiteSpace(getAllInterviewsFromCache))
            {
                list = _context.Interviews.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Interview>>(getAllInterviewsFromCache);
            }

            List<InterviewViewModel> interviewList = new List<InterviewViewModel>();
            foreach (Interview interview in list)
                interviewList.Add(new InterviewViewModel(interview));

            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully Reterived Interviews";
            response.allInterviews = interviewList;

            return response;
        }
        public async Task<All_InterviewResponseViewModel> GetAllInterviewsForInterviewer(Guid InterViewerId)
        {
            string key = $"getAllInterviewsByInterviewerId-{InterViewerId}";
            string? getAllInterviewsByInterviewerIdFromCache = await _cache.GetStringAsync(key);

            List<Interview> list;
            if (string.IsNullOrWhiteSpace(getAllInterviewsByInterviewerIdFromCache))
            {
                list = _context.Interviews.Where(e => e.InterViewerId == InterViewerId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Interview>>(getAllInterviewsByInterviewerIdFromCache);
            }

            List<InterviewViewModel> interviewList = new List<InterviewViewModel>();
            foreach (Interview interview in list)
                interviewList.Add(new InterviewViewModel(interview));

            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            response.Status = 200;
            response.allInterviews = interviewList;
            if(response.allInterviews.Count>0)
                response.Message = "Successfully Reterived Interviews for Interviewer !";
            else
                response.Message = "No Scheduled Interviews Found for Interviewer !";

            return response;
        }

        public async Task<InterviewResponseViewModel> GetInterviewById(Guid Id)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            string key = $"getInterviewById-{Id}";
            string? getInterviewByIdFromCache = await _cache.GetStringAsync(key);

            Interview dbinterview;
            if (string.IsNullOrWhiteSpace(getInterviewByIdFromCache))
            {
                dbinterview = _context.Interviews.Find(Id);
                if (dbinterview == null)
                {
                    response.Status = 500;
                    response.Message = "Not Able to Found Interview";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbinterview));
            }
            else
            {
                dbinterview = JsonSerializer.Deserialize<Interview>(getInterviewByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully Found Interview";
            response.Interview = new InterviewViewModel(dbinterview);
            return response;
        }
   
        public async Task<bool> UpdateFeedbackId(Guid InterviewId, Guid FeedbackId)
        {
            string key = $"getInterviewById-{InterviewId}";
            string? getInterviewByIdFromCache = await _cache.GetStringAsync(key);

            Interview interview;
            if (string.IsNullOrWhiteSpace(getInterviewByIdFromCache))
            {
                interview = _context.Interviews.Find(InterviewId);
                if (interview == null)
                {
                    return false;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(interview));
            }
            else
            {
                interview = JsonSerializer.Deserialize<Interview>(getInterviewByIdFromCache);
            }

            interview.FeedbackId = FeedbackId;
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allInterviews");
            await _cache.RemoveAsync($"getInterviewById-{InterviewId}");
            await _cache.RemoveAsync($"getAllInterviewsByApplicationId-{interview.ApplicationId}");
            await _cache.RemoveAsync($"getAllInterviewsByInterviewerId-{interview.InterViewerId}");

            return true;
        }

        public async Task<All_InterviewResponseViewModel> GetAllInterviewByApplicationId(Guid ApplicationId)
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();

            var application = await _jobApplicationService.GetJobApplicaionById(ApplicationId);
            if (application.Application == null)
            {
                response.Status = 404;
                response.Message = "Application with this Id does not exist.";
                return response;
            }

            string key = $"getAllInterviewsByApplicationId-{ApplicationId}";
            string? getAllInterviewsByApplicationIdFromCache = await _cache.GetStringAsync(key);

            List<Interview> interviews;
            if (string.IsNullOrWhiteSpace(getAllInterviewsByApplicationIdFromCache))
            {
                interviews = _context.Interviews.Where(item => item.ApplicationId == ApplicationId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(interviews));
            }
            else
            {
                interviews = JsonSerializer.Deserialize<List<Interview>>(getAllInterviewsByApplicationIdFromCache);
            }

            List<InterviewViewModel> allInterview = new List<InterviewViewModel>();
            foreach (var item in interviews)
            {
                allInterview.Add(new InterviewViewModel(item));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all interviews by applicationId";
            response.allInterviews = allInterview;
            return response;
        }

        public async Task<AllApplicantInterviewResponseViewModel> GetAllApplicantInterviewByApplicantionId(Guid ApplicationId)
        {
            AllApplicantInterviewResponseViewModel response = new AllApplicantInterviewResponseViewModel();

            var allInterviews = await GetAllInterviewByApplicationId(ApplicationId);
            if(allInterviews.Status != 200)
            {
                response.Status = allInterviews.Status;
                response.Message = allInterviews.Message;
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrived all applicant interviews.";
            response.AllInterviews = new List<ApplicantInterviewViewModel>();

            foreach (var item in allInterviews.allInterviews)
            {
                var interviewer = await _employeeAccountService.GetEmployeeById((Guid)item.InterViewerId);

                string key = $"getInterviewFeedbackById-{item.FeedbackId}";
                string? getInterviewFeedbackByIdFromCache = await _cache.GetStringAsync(key);

                InterviewFeedback feedback;
                if (string.IsNullOrWhiteSpace(getInterviewFeedbackByIdFromCache))
                {
                    feedback = _context.InterviewFeedbacks.Find(item.FeedbackId);
                    if (feedback != null)
                    {
                        await _cache.SetStringAsync(key, JsonSerializer.Serialize(feedback));
                    }
                }
                else
                {
                    feedback = JsonSerializer.Deserialize<InterviewFeedback>(getInterviewFeedbackByIdFromCache);
                }

                ApplicantInterviewViewModel interview = new ApplicantInterviewViewModel();
                interview.Interview = item;
                interview.Interviewer = interviewer.Employee;
                interview.Feedback = (feedback == null ? null : new InterviewFeedbackViewModel(feedback));

                response.AllInterviews.Add(interview);
            }

            return response;
        }
    }
}
