using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationWithPrivilegeController : ControllerBase
    {
        private readonly IDesignationWithPrivilegeService _designationWithPrivilegeService;
        private readonly IDesignationService _designationService;
        public DesignationWithPrivilegeController(IDesignationService designationService, IDesignationWithPrivilegeService designationWithPrivilegeService)
        {
            _designationWithPrivilegeService = designationWithPrivilegeService;
            _designationService = designationService;
        }

        [HttpGet]
        [Route("getAllPrivileges")]
        public AllPrivilegeResponseViewModel GetAllPrivileges()
        {
            AllPrivilegeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasSpecialPrivilege(role))
            {
                response = new AllPrivilegeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to get all special privileges.";
                return response;
            }

            response = _designationWithPrivilegeService.GetAllPrivileges();
            return response;
        }

        [HttpGet]
        [Route("getPrivilege/{Id}")]
        public PrivilegeResponseViewModel GetPrivilegeById(int Id)
        {
            PrivilegeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasSpecialPrivilege(role))
            {
                response = new PrivilegeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to get special privilege.";
                return response;
            }

            response = _designationWithPrivilegeService.GetPrivilegeWithId(Id);
            return response;
        }

        [HttpGet]
        [Route("getPrivilege/designation/{DesignationId}")]
        public async Task<PrivilegeResponseViewModel> GetPrivilegeByDesignationId(int DesignationId)
        {
            PrivilegeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new PrivilegeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to get special privilege.";
                return response;
            }

            response = await _designationWithPrivilegeService.GetPrivilegeByDesignationId(DesignationId);
            return response;
        }

        [HttpPost]
        [Route("addPrivilege")]
        public async Task<PrivilegeResponseViewModel> AddPrivilege(AddPrivilegeViewModel addPrivilege)
        {
            PrivilegeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasSpecialPrivilege(role))
            {
                response = new PrivilegeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to add special privilege.";
                return response;
            }

            DesignationResponseViewModel designation = await _designationService.GetDesignationById(addPrivilege.DesignationId);
            if(designation.Designation == null)
            {
                response = new PrivilegeResponseViewModel();
                response.Status = 404;
                response.Message = "Designation with this Id does not exist.";
                return response;
            }

            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            response = await _designationWithPrivilegeService.AddPrivilege(addPrivilege, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("removePrivilege/{Id}")]
        public async Task<PrivilegeResponseViewModel> RemovePrivilege(int Id)
        {
            PrivilegeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasSpecialPrivilege(role))
            {
                response = new PrivilegeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to remove special privilege.";
                return response;
            }

            response = await _designationWithPrivilegeService.RemovePrivilege(Id);
            return response;
        }
    }
}
