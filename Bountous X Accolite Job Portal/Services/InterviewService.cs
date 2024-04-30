using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class InterviewService : I_InterviewService
    {
        private readonly ApplicationDbContext _context;
        public InterviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview , Guid EmpId)
        {
            Interview newInterview = new Interview(interview);
            newInterview.EmpId = EmpId;

            await _context.Interviews.AddAsync(newInterview);
            await _context.SaveChangesAsync();

            InterviewResponseViewModel response = new InterviewResponseViewModel();
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
            var dbinterview = _context.Interviews.Find(interview.InterviewId);
            if(dbinterview != null)
            {
                dbinterview.InterviewDate = interview.InterviewDate;
                dbinterview.InterviewTime = interview.InterviewTime;
                dbinterview.InterViewerId = interview.InterViewerId; 
                dbinterview.Link = interview.Link;

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
   
        
    }
}
