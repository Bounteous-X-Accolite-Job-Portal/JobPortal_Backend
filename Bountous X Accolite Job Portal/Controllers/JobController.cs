using Bountous_X_Accolite_Job_Portal.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bountous_X_Accolite_Job_Portal.Data;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        ApplicationDbContext context;
        public JobController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpDelete]
        public JsonResult deleteById(string id)
        {
            var job = context.Jobs.FirstOrDefault(x => x.JobId == id);
            if (job != null)
            {
                context.Jobs.Remove(job);
                context.SaveChanges();
                return new JsonResult(Ok("Job Id : " + id + " deleted !!"));
            }
            else
                return new JsonResult(NotFound(id));
        }

        [HttpPost]
        public JsonResult AddJob(Job job)
        {
            context.Jobs.Add(job);
            context.SaveChanges();
            return new JsonResult(Ok("Job Added " + job.JobId));
        }

        [HttpGet("/GetByJobId")]
        public JsonResult GetJob(string id)
        {
            var job = context.Jobs.FirstOrDefault(x => x.JobId == id);
            if (job != null)
                return new JsonResult(Ok(job));
            else
                return new JsonResult(NotFound(id));
        }

        [HttpGet("/GetAllJobs")]
        public JsonResult GetAllJobs()
        {
            return new JsonResult(context.Jobs.ToList());
        }

        [HttpPut]
        public JsonResult PutJob(string id, string loc, string desc, DateOnly dt)
        {
            var job = context.Jobs.FirstOrDefault(x => x.JobId == id);
            if (job != null)
            {
                job.JobLocation = loc;
                job.JobDescription = desc;
                job.LastDate = dt;
                context.SaveChanges();
                return new JsonResult(Ok("Job id : " + id + " Updated !!"));
            }
            else
                return new JsonResult(NotFound(id));
        }
    }
}
