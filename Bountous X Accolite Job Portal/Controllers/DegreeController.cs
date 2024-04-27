﻿using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel.DegreeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DegreeController : ControllerBase
    {
        private readonly IDegreeService _degreeService;
        private readonly UserManager<User> _userManager;
        public DegreeController(IDegreeService degreeService, UserManager<User> userManager)
        {
            _degreeService = degreeService;
            _userManager = userManager;
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

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new DegreeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Degrees.";
                return response;
            }

            response = await _degreeService.AddDegree(addDegree, (Guid)employee.EmpId);    
            return response;
        }

        [HttpPost]
        [Route("updateDegree")]
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

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new DegreeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Degrees.";
                return response;
            }

            response = await _degreeService.UpdateDegree(updateDegree);
            return response;
        }

        [HttpPost]
        [Route("removeDegree/{id}")]
        public async Task<DegreeResponseViewModel> RemoveDegree(Guid id)
        {
            DegreeResponseViewModel response;

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
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
