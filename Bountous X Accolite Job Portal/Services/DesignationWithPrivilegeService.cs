using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DesignationWithPrivilegeService : IDesignationWithPrivilegeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDesignationService _designationService;
        private readonly IDistributedCache _cache;
        public DesignationWithPrivilegeService(ApplicationDbContext dbContext, IDesignationService designationService, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _designationService = designationService;
            _cache = cache;
        }

        public async Task<AllPrivilegeResponseViewModel> GetAllPrivileges()
        {
            AllPrivilegeResponseViewModel response = new AllPrivilegeResponseViewModel();

            string key = $"allDesignationWithPrivilege";
            string? getAllDesignationWithPrivilegeFromCache = await _cache.GetStringAsync(key);

            List<DesignationWhichHasPrivilege> allPrivileges;
            if (string.IsNullOrWhiteSpace(getAllDesignationWithPrivilegeFromCache))
            {
                allPrivileges = _dbContext.DesignationWhichHasPrivileges.Where(item => true).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(allPrivileges));
            }
            else
            {
                allPrivileges = JsonSerializer.Deserialize<List<DesignationWhichHasPrivilege>>(getAllDesignationWithPrivilegeFromCache);
            }

            List<PrivilegeViewModel> privileges = new List<PrivilegeViewModel>();
            foreach (var privilege in allPrivileges)
            {
                privileges.Add(new PrivilegeViewModel(privilege));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all privileged designation.";
            response.AllPrivileges = privileges;
            return response;
        }

        public async Task<PrivilegeResponseViewModel> GetPrivilegeWithId(int PrivilegeId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            string key = $"getPrivilegeById-{PrivilegeId}";
            string? getPrivilegeByIdFromCache = await _cache.GetStringAsync(key);

            DesignationWhichHasPrivilege privilege;
            if (string.IsNullOrWhiteSpace(getPrivilegeByIdFromCache))
            {
                privilege = _dbContext.DesignationWhichHasPrivileges.Find(PrivilegeId);
                if (privilege == null)
                {
                    response.Status = 404;
                    response.Message = "Privilege with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(privilege));
            }
            else
            {
                privilege = JsonSerializer.Deserialize<DesignationWhichHasPrivilege>(getPrivilegeByIdFromCache);
            }
           
            response.Status = 200;
            response.Message = "Successfully retrieved privilege with given Id.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(privilege);
            return response;
        }

        public async Task<PrivilegeResponseViewModel> AddPrivilege(AddPrivilegeViewModel addPrivilege, Guid EmpId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            var privilege = await GetPrivilegeByDesignationId(addPrivilege.DesignationId);
            if (privilege.DesignationWithPrivilege != null)
            {
                response.Status = 404;
                response.Message = "Privilege with this Id already exist.";
                return response;
            }

            DesignationWhichHasPrivilege hasPrivilege = new DesignationWhichHasPrivilege();
            hasPrivilege.DesignationId = addPrivilege.DesignationId;
            hasPrivilege.EmployeeId = EmpId;

            await _dbContext.DesignationWhichHasPrivileges.AddAsync(hasPrivilege);
            await _dbContext.SaveChangesAsync();

            if(hasPrivilege == null)
            {
                response.Status = 500;
                response.Message = "Something went wrong, please try again.";
                return response;
            }

            await _cache.RemoveAsync($"allDesignationWithPrivilege");

            response.Status = 200;
            response.Message = "Successfully added privilege.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(hasPrivilege);
            return response;
        }

        public async Task<PrivilegeResponseViewModel> RemovePrivilege(int PrivilegeId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            string key = $"getPrivilegeById-{PrivilegeId}";
            string? getPrivilegeByIdFromCache = await _cache.GetStringAsync(key);

            DesignationWhichHasPrivilege privilege;
            if (string.IsNullOrWhiteSpace(getPrivilegeByIdFromCache))
            {
                privilege = _dbContext.DesignationWhichHasPrivileges.Find(PrivilegeId);
                if (privilege == null)
                {
                    response.Status = 404;
                    response.Message = "Privilege with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(privilege));
            }
            else
            {
                privilege = JsonSerializer.Deserialize<DesignationWhichHasPrivilege>(getPrivilegeByIdFromCache);
            }

            _dbContext.DesignationWhichHasPrivileges.Remove(privilege);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allDesignationWithPrivilege");
            await _cache.RemoveAsync($"getPrivilegeByDesignationId-{privilege.DesignationId}");
            await _cache.RemoveAsync($"getPrivilegeById-{PrivilegeId}");

            response.Status = 200;
            response.Message = "Successfully removed privilege with given Id.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(privilege);
            return response;
        }

        public async Task<PrivilegeResponseViewModel> GetPrivilegeByDesignationId(int DesignationId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            var designation = await _designationService.GetDesignationById(DesignationId);
            if(designation.Designation == null)
            {
                response.Status = designation.Status;
                response.Message = designation.Message;
                return response;
            }

            string key = $"getPrivilegeByDesignationId-{DesignationId}";
            string? getPrivilegeByDesignationIdFromCache = await _cache.GetStringAsync(key);

            DesignationWhichHasPrivilege privilege;
            if (string.IsNullOrWhiteSpace(getPrivilegeByDesignationIdFromCache))
            {
                privilege = _dbContext.DesignationWhichHasPrivileges.Where(item => item.DesignationId == DesignationId).FirstOrDefault();
                if (privilege == null)
                {
                    response.Status = 404;
                    response.Message = "This designation does not have privilege.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(privilege));
            }
            else
            {
                privilege = JsonSerializer.Deserialize<DesignationWhichHasPrivilege>(getPrivilegeByDesignationIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved privilege with this designationId.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(privilege);
            return response;
        }
    }
}
