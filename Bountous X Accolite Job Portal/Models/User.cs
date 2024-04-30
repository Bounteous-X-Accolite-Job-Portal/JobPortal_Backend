using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class User : IdentityUser
    {
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }

        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }

        public Designation? Designations { get; set; }
        public EducationInstitution? EducationInstitutions { get; set; }
        public Degree? Degrees { get; set; }
        public CandidateEducation? CandidateEducations { get; set; }
        public Company? Company { get; set; }
        public CandidateExperience? CandidateExperience { get; set; }
        public Job? Jobs { get; set; }
        public JobLocation? JobLocation { get; set; }
        public JobPosition? JobPosition { get; set; }
        public JobCategory? JobCategory { get; set; }
        public Resume? Resumes { get; set; }
        public Interview? Interviews { get; set; }
        public InterviewFeedback? InterviewFeedbacks { get; set; }
        public Application? Applications { get; set; }
    }
}
