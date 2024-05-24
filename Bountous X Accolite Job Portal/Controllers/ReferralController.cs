using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralController : ControllerBase
    {
        private readonly IReferralService _referralService;
        public ReferralController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        [HttpPost]
        [Route("refer")]
        public async Task<ReferralResponseViewModel> Refer(AddReferralViewModel addReferral)
        {
            ReferralResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new ReferralResponseViewModel();
                response.Status = 404;
                response.Message = "Please enter all details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new ReferralResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Referral.";
                return response;
            }

            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            response = await _referralService.Refer(addReferral, employeeId);
            return response;
        }

        [HttpGet]
        [Route("getAllReferrals")]
        public async Task<AllReferralResponseViewModel> GetAllReferralsOfLoggedInEmployee()
        {
            AllReferralResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new AllReferralResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get Referrals.";
                return response;
            }

            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            response = await _referralService.GetAllReferralsOfLoggedInEmployee(employeeId);
            return response;
        }
    }
}
