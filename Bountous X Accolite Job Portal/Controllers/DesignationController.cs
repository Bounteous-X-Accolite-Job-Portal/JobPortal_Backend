﻿using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;   
        }

        [HttpGet]
        [Route("getAllDesignations")]
        public async Task<AllDesignationResponseViewModel> GetAllDesignations()
        {
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                AllDesignationResponseViewModel response = new AllDesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to get all Designations.";
                return response;
            }

            bool hasSpecialPrivilege = Convert.ToBoolean(User.FindFirstValue("HasSpecialPrivilege"));
            return await _designationService.GetAllDesignation(hasSpecialPrivilege);
        }

        [HttpGet]
        [Route("designation/{Id}")]
        public async Task<DesignationResponseViewModel> GetDesignationsById(int Id)
        {
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                DesignationResponseViewModel response = new DesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to get Designation.";
                return response;
            }

            return await _designationService.GetDesignationById(Id);
        }

        [HttpPost]
        [Route("addDesignation")]
        public async Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel addDesignation)
        {
            DesignationResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new DesignationResponseViewModel();
                response.Status = 404;
                response.Message = "Please Enter all details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new DesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to add Designation.";
                return response;  
            }

            response = await _designationService.AddDesignation(addDesignation, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("removeDesignation/{Id}")]
        public async Task<DesignationResponseViewModel> RemoveDesignationById(int Id)
        {
            DesignationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new DesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to delete Designation.";
                return response;
            }

            return await _designationService.RemoveDesignation(Id);
        }
    }
}
