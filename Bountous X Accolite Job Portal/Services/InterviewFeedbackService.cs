using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class InterviewFeedbackService : I_InterviewFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public InterviewFeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewFeedbackResponseViewModel> AddInterviewFeedback(CreateInterviewFeedbackViewModel interviewFeedback , Guid Empid)
        {
            var interview = _context.Interviews.Find(interviewFeedback.InterviewId);

            InterviewFeedbackResponseViewModel response;
            if (interview == null)
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 402;
                response.Message = "Interview Not Exists ! ";
                return response;
            }
            
            if(interview.InterViewerId!=Empid)
            {
                response = new InterviewFeedbackResponseViewModel();
                response.Status = 402;
                response.Message = " Currently Logged In Employee & Interviewer Mismatches !! ";
                return response;
            }

            InterviewFeedback newInterviewFeedback = new InterviewFeedback(interviewFeedback);
            await _context.InterviewFeedbacks.AddAsync(newInterviewFeedback);
            await _context .SaveChangesAsync();

            if(newInterviewFeedback!=null)
            {
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
            var interviewFeedback = _context.InterviewFeedbacks.Find(Id);
            if(interviewFeedback!=null)
            {
                _context.InterviewFeedbacks.Remove(interviewFeedback);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Interview Feedback Successfully Removed !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Remove Interview Feedback !!";
            }
            return response;
        }

        public async Task<InterviewFeedbackResponseViewModel> EditInterviewFeedback(EditInterviewFeedbackViewModel interviewFeedback , Guid EmpId)
        {
            var interview = _context.Interviews.Find(interviewFeedback.InterviewId);

            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();
            if (interview == null)
            {
                response.Status = 402;
                response.Message = "Interview Not Exists ! ";
                return response;
            }

            if (interview.InterViewerId != EmpId)
            {
                response.Status = 402;
                response.Message = " Currently Logged In Employee & Interviewer Mismatches !! ";
                return response;
            }

            var dbInterviewFeedback = _context.InterviewFeedbacks.Find(interviewFeedback.FeedbackId);

            if (dbInterviewFeedback != null)
            {
                dbInterviewFeedback.Rating = interviewFeedback.Rating;
                dbInterviewFeedback.Feedback = interviewFeedback.Feedback;
                dbInterviewFeedback.AdditionalLink = interviewFeedback.AdditionalLink;
                
                _context.InterviewFeedbacks.Update(dbInterviewFeedback);
                await _context.SaveChangesAsync();
                
                response.Status = 200;
                response.Message = "Interview Feedback Successfully Updated ! ! ";
                response.interviewFeedback = new InterviewFeedbackViewModel(dbInterviewFeedback);
            }
            else
            {
                response.Status = 403;
                response.Message = "Unable to UPDATE Interview Feedback ! ";
            }
            return response;
        }

        public AllInterviewFeedbackResponseViewModel GetAllInterviewFeedbacksByAEmployee(Guid EmployeeId)
        {
            List<InterviewFeedback> list = _context.InterviewFeedbacks.ToList();
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

        public InterviewFeedbackResponseViewModel GetInterviewFeedbackById(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();
            var dbInterviewFeedback = _context.InterviewFeedbacks.Find(Id);

            if(dbInterviewFeedback != null)
            {
                response.Status = 200;
                response.Message = "Successfully Found Interview Feedback";
                response.interviewFeedback = new InterviewFeedbackViewModel(dbInterviewFeedback);
            }
            else
            {
                response.Status = 500;
                response.Message = "Not Able to Found Interview Feedback";
            }

            return response;
        }
    }
}
