//using Bountous_X_Accolite_Job_Portal.Models;
//using System;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Bountous_X_Accolite_Job_Portal.Data;

//namespace Bountous_X_Accolite_Job_Portal.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class JobController : ControllerBase
//    {
//        ApplicationDbContext context;
//        public JobController(ApplicationDbContext context)
//        {
//            this.context = context;
//        }

//        [HttpDelete]
//        public JsonResult deleteById(int id)
//        {
//            var job = context.JobApplications.FirstOrDefault(x => x.JobId == id);
//            if (job != null)
//            {
//                context.JobApplications.Remove(job);
//                context.SaveChanges();
//                return new JsonResult(Ok("Job Id : " + id + " deleted !!"));
//            }
//            else
//                return new JsonResult(NotFound(id));
//        }

//        [HttpPost]
//        public JsonResult AddJob(JobApplication job)
//        {
//            context.JobApplications.Add(job);
//            context.SaveChanges();
//            return new JsonResult(Ok("Job Added " + job.JobId));
//        }

//        [HttpGet("/GetByJobId")]
//        public JsonResult GetJob(int id)
//        {
//            var job = context.JobApplications.FirstOrDefault(x => x.JobId == id);
//            if (job != null)
//                return new JsonResult(Ok(job));
//            else
//                return new JsonResult(NotFound(id));
//        }

//        [HttpGet("/GetAllJobs")]
//        public JsonResult GetAllJobs()
//        {
//            return new JsonResult(context.JobApplications.ToList());
//        }

//    }

