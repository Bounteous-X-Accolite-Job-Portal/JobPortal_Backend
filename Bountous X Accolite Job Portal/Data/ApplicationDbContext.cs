using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Bountous_X_Accolite_Job_Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> User {  get; set; }
        public DbSet<Job> Jobs {  get; set; }

        public DbSet<Emplyoee> Employees { get; set; }

        public DbSet<Designation> Designations { get; set; }


    }
}
