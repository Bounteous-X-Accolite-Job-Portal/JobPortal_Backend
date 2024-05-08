using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel.DegreeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private readonly IDegreeService _degreeService;
        public DegreeController(IDegreeService degreeService)
        {
            _degreeService = degreeService;
        }

        [HttpGet]
        [Route("getAllDegrees")]
        public AllDegreeResponseViewModel GetAllDegrees()
        {
            List<DegreeViewModel> degrees = _degreeService.GetAllDegree();

            AllDegreeResponseViewModel response = new AllDegreeResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully retrieved all institutions.";
            response.Degrees = degrees;
            return response;
        }

        [HttpGet]
        [Route("getDegree/{id}")]
        public DegreeResponseViewModel GetDegree(Guid id)
        {
            DegreeResponseViewModel response = _degreeService.GetDegree(id);
            return response;
        }

        [HttpPost]
        [Route("addDegree")]
        [Authorize]
        public async Task<DegreeResponseViewModel> AddDegree(AddDegreeViewModel addDegree)
        {
            DegreeResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new DegreeResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee)
            {
                response = new DegreeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Degrees.";
                return response;
            }

            response = await _degreeService.AddDegree(addDegree, employeeId);    
            return response;
        }

        [HttpPut]
        [Route("updateDegree")]
        [Authorize]
        public async Task<DegreeResponseViewModel> UpdateDegree(UpdateDegreeViewModel updateDegree)
        {
            DegreeResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new DegreeResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new DegreeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Degrees.";
                return response;
            }

            response = await _degreeService.UpdateDegree(updateDegree);
            return response;
        }

        [HttpDelete]
        [Route("removeDegree/{id}")]
        [Authorize]
        public async Task<DegreeResponseViewModel> RemoveDegree(Guid id)
        {
            DegreeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new DegreeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Degrees.";
                return response;
            }

            response = await _degreeService.RemoveDegree(id);
            return response;
        }
    }
}
