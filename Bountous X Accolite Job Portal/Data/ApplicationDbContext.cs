﻿using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Bountous_X_Accolite_Job_Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users {  get; set; }
        public DbSet<Candidate> Candidates { get; set; }    
        public DbSet<Employee> Employees { get; set; }  
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Job> Jobs { get; set; }

    }
}
