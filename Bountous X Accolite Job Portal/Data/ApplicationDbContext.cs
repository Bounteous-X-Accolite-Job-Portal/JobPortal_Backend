using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Bountous_X_Accolite_Job_Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EducationInstitution> EducationInstitutions { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<CandidateEducation> CandidateEducations { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CandidateExperience> CandidateExperience { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobLocation> JobLocation { get; set; }
        public DbSet<JobPosition> JobPosition { get; set; }
        public DbSet<JobCategory> JobCategory { get; set; }
        public DbSet<JobType> JobType { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewFeedback> InterviewFeedbacks { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<DesignationWhichHasPrivilege> DesignationWhichHasPrivileges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call base method to apply default behavior

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Designation)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.DesignationId)
                .IsRequired();
        }
    }
}
