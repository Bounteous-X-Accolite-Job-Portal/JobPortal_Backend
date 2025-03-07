using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Bountous_X_Accolite_Job_Portal.JwtFeatures;

namespace Bountous_X_Accolite_Job_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(50, 1000);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisCacheConnection");
                options.InstanceName = "master";
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
            });

            builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>
                (
            options => options.SignIn.RequireConfirmedEmail = false);


            builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://kind-dune-058eee70f.5.azurestaticapps.net").AllowAnyHeader().AllowAnyMethod();
                }));

            // Adding JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    //ValidAudience = jwtSettings["validAudience"],
                    ValidAudiences = new List<String> {
                        jwtSettings["validAudience"],
                        jwtSettings["validAudience1"],
                    },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value)),
                    ClockSkew = TimeSpan.Zero
                };
            });

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
            //builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
            builder.Services.AddScoped<IJobCategoryService,JobCategoryService>();
            builder.Services.AddScoped<IJobPositionService,JobPositionService>();
            builder.Services.AddScoped<IJobLocationService,JobLocationService>();
            builder.Services.AddScoped<IJobService,JobService>();
            builder.Services.AddScoped<IJobTypeService,JobTypeService>();
            builder.Services.AddScoped<I_InterviewService, InterviewService>();
            builder.Services.AddScoped<I_InterviewFeedbackService, InterviewFeedbackService>();
            builder.Services.AddScoped<ISocialMediaService, SocialMediaService>();
            builder.Services.AddScoped<ISkillsService, SkillsService>();
            builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
            builder.Services.AddScoped<IJobStatusService, JobStatusService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IDesignationWithPrivilegeService, DesignationWithPrivilegeService>();
            builder.Services.AddScoped<IReferralService, ReferralService>();
            builder.Services.AddScoped<IOfferLetterEmailService , OfferLetterEmailService>();

            // Addding JWT as a service
            builder.Services.AddScoped<JwtHandler>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
