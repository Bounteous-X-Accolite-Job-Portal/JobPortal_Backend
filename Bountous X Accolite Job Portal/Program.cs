
using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Bountous_X_Accolite_Job_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

             builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));


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

            builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                }));


            builder.Services.AddControllers();

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(opt =>
                opt.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
                })
            );

            // adding for getting logged in user
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // adding services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICandidateAccountService, CandidateAccountServices>();
            builder.Services.AddScoped<IEmployeeAccountService, EmployeeAccountService>();
            builder.Services.AddScoped<IDesignationService, DesignationService>();
            builder.Services.AddScoped<IEducationInstitutionService, EducationInstitutionService>();
            builder.Services.AddScoped<IDegreeService, DegreeService>();
            builder.Services.AddScoped<ICandidateEducationService, CandidateEducationService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<ICandidateExperienceService, CandidateExperienceService>();
            builder.Services.AddScoped<IResumeService, ResumeService>();
            builder.Services.AddScoped<IJobCategory,JobCategoryService>();
            builder.Services.AddScoped<IJobPosition,JobPositionService>();
            builder.Services.AddScoped<IJobLocation,JobLocationService>();
            builder.Services.AddScoped<IJob,JobService>();


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
