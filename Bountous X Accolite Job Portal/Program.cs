
using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bountous_X_Accolite_Job_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                        builder.Configuration.GetConnectionString("LocalConnection")
                    ));

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //            builder.Configuration.GetConnectionString("LocalConnection")
            //        ));

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //builder.Services.AddIdentityCore<Employee>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //  .AddDefaultTokenProviders();

            // CORS
            builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                }));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // adding for getting logged in user
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // adding services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmployeeAuthService, EmployeeAuthService>();
            builder.Services.AddScoped<IDesignationService, DesignationService>();
            builder.Services.AddScoped<IEducationInstitutionService, EducationInstitutionService>();
            builder.Services.AddScoped<IDegreeService, DegreeService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            // CORS
            app.UseCors("FrontendUI");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
