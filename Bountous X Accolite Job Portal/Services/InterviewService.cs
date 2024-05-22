using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class InterviewService : I_InterviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeAccountService _employeeAccountService;
        public InterviewService(
            ApplicationDbContext context, 
            IEmployeeAccountService employeeAccountService
        )
        {
            _context = context;
            _employeeAccountService = employeeAccountService;
        }

        public async Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview , Guid EmpId)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            var application = _context.JobApplications.Find(interview.ApplicationId);
            if(application == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            var interviewer = _context.Employees.Find(interview.InterViewerId);
            if (interviewer == null)
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

            if(newInterview==null)
            {
                response.Status = 500;
                response.Message = "Unable to Add Interview !!";
            }
            else
            {
                response.Status = 200;
                response.Message = "Interview Scheduled Successfully !!";
                response.Interview = new InterviewViewModel(newInterview);
            }
            return response;
        }

        public async Task<InterviewResponseViewModel> DeleteInterview(Guid Id)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            var interview = _context.Interviews.Find(Id);
            if(interview != null)
            {
                 _context.Interviews.Remove(interview);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Interview Successfully Removed !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Remove Interview !";
            }
            return response;
        }

        public async Task<InterviewResponseViewModel> EditInterview(EditInterviewViewModel interview)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();

            var application = _context.JobApplications.Find(interview.ApplicationId);
            if (application == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            var interviewer = _context.Employees.Find(interview.InterViewerId);
            if (interviewer == null)
            {
                response.Status = 404;
                response.Message = "The application does not exist.";
                return response;
            }

            var dbinterview = _context.Interviews.Find(interview.InterviewId);
            if(dbinterview != null)
            {
                dbinterview.InterviewDate = interview.InterviewDate;
                dbinterview.InterviewTime = interview.InterviewTime;
                dbinterview.InterViewerId = interview.InterViewerId; 
                dbinterview.Link = interview.Link;
                dbinterview.FeedbackId = interview.FeedbackId;

                _context.Interviews.Update(dbinterview);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Interview Successfully Updated !";
                response.Interview = new InterviewViewModel(dbinterview);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Update Interview !";
            }
            return response;
        }

        public async void ChangeInterviewApplicationToClosedApplication(Guid ApplicationId, Guid ClosedApplicationId)
        {
            List<Interview> list = _context.Interviews.Where(item => item.ApplicationId == ApplicationId).ToList();
            foreach(Interview interview in list)
            {
                interview.ApplicationId = null;
                interview.ClosedJobApplicationId = ClosedApplicationId;

                _context.Interviews.Update(interview);
            }

            await _context.SaveChangesAsync();
        }

        public All_InterviewResponseViewModel GetAllInterviews()
        {
            List<Interview> list = _context.Interviews.ToList();

            List<InterviewViewModel> interviewList = new List<InterviewViewModel>();
            foreach (Interview interview in list)
                interviewList.Add(new InterviewViewModel(interview));

            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully Reterived Interviews";
            response.allInterviews = interviewList;

            return response;
        }
        public All_InterviewResponseViewModel GetAllInterviewsForInterviewer(Guid InterViewerId)
        {
            List<Interview> list = _context.Interviews.Where( e => e.InterViewerId==InterViewerId).ToList();

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

        public InterviewResponseViewModel GetInterviewById(Guid Id)
        {
            InterviewResponseViewModel response = new InterviewResponseViewModel();
            var dbinterview = _context.Interviews.Find(Id);
            if (dbinterview != null)
            {
                response.Status = 200;
                response.Message = "Successfully Found Interview";
                response.Interview = new InterviewViewModel(dbinterview);
            }
            else
            {
                response.Status = 500;
                response.Message = "Not Able to Found Interview";
            }
            return response;
        }
   
        public async Task<bool> UpdateFeedbackId(Guid InterviewId, Guid FeedbackId)
        {
            var interview = _context.Interviews.Find(InterviewId);
            if(interview == null)
            {
                return false;
            }

            interview.FeedbackId = FeedbackId;
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();

            return true;
        }

        public All_InterviewResponseViewModel GetAllInterviewByApplicationId(Guid ApplicationId)
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();

            var application = _context.JobApplications.Find(ApplicationId);
            if (application == null)
            {
                response.Status = 404;
                response.Message = "Application with this Id does not exist.";
                return response;
            }

            List<Interview> interviews = _context.Interviews.Where(item => item.ApplicationId == ApplicationId).ToList();

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

            var allInterviews = GetAllInterviewByApplicationId(ApplicationId);
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
                var interviewer = _employeeAccountService.GetEmployeeById((Guid)item.InterViewerId);
                var feedback = _context.InterviewFeedbacks.Find(item.FeedbackId);

                ApplicantInterviewViewModel interview = new ApplicantInterviewViewModel();
                interview.Interview = item;
                interview.Interviewer = interviewer.Employee;
                interview.Feedback = feedback == null ? null : new InterviewFeedbackViewModel(feedback);

                response.AllInterviews.Add(interview);
            }

            return response;
        }
    }
}
