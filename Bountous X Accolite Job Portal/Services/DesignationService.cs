using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public DesignationService(ApplicationDbContext applicationDbContext, IDistributedCache cache)
        {
            _dbContext = applicationDbContext;
            _cache = cache;
        }

        public bool HasSpecialPrivilege(string role)
        {
            if (String.Equals(role.ToLower(), "admin"))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> HasPrivilege(int designationId)
        {
            string key = $"getPrivilegeByDesignationId-{designationId}";
            string? getPrivilegeByDesignationIdFromCache = await _cache.GetStringAsync(key);

            DesignationWhichHasPrivilege check;
            if (string.IsNullOrWhiteSpace(getPrivilegeByDesignationIdFromCache))
            {
                check = _dbContext.DesignationWhichHasPrivileges.Where(item => item.DesignationId == designationId).FirstOrDefault();
                if (check == null)
                {
                    return false;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(check, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
            }
            else
            {
                check = JsonSerializer.Deserialize<DesignationWhichHasPrivilege>(getPrivilegeByDesignationIdFromCache, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            }

            return true;
        }

        //public async Task<bool> HasPrivilege(string role)
        //{
        //    string key = $"getDesignationByRole-{role}";
        //    string? getDesignationByRoleFromCache = await _cache.GetStringAsync(key);

        //    Designation designation;
        //    if (string.IsNullOrWhiteSpace(getDesignationByRoleFromCache))
        //    {
        //        designation = _dbContext.Designations.Where(item => String.Equals(item.DesignationName.ToLower(), role.ToLower())).FirstOrDefault();
        //        if(designation == null)
        //        {
        //            return false;
        //        }

        //        await _cache.SetStringAsync(key, JsonSerializer.Serialize(designation, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
        //    }
        //    else
        //    {
        //        designation = JsonSerializer.Deserialize<Designation>(getDesignationByRoleFromCache, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
        //    }

        //    key = $"getPrivilegeByDesignationId-{designation.DesignationId}";
        //    string? getPrivilegeByDesignationIdFromCache = await _cache.GetStringAsync(key);

        //    DesignationWhichHasPrivilege check;
        //    if (string.IsNullOrWhiteSpace(getPrivilegeByDesignationIdFromCache))
        //    {
        //        check = _dbContext.DesignationWhichHasPrivileges.Where(item => item.DesignationId == designation.DesignationId).FirstOrDefault();
        //        if (check == null)
        //        {
        //            return false;
        //        }

        //        await _cache.SetStringAsync(key, JsonSerializer.Serialize(check, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
        //    }
        //    else
        //    {
        //        check = JsonSerializer.Deserialize<DesignationWhichHasPrivilege>(getPrivilegeByDesignationIdFromCache, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
        //    }

        //    return true;
        //}

        public async Task<DesignationResponseViewModel> GetDesignationById(int Id)
        {
            DesignationResponseViewModel response = new DesignationResponseViewModel();

            string key = $"getDesignationById-{Id}";
            string? getDesignationByIdFromCache = await _cache.GetStringAsync(key);

            Designation designation;
            if (string.IsNullOrWhiteSpace(getDesignationByIdFromCache))
            {
                designation = _dbContext.Designations.Find(Id);
                if (designation == null)
                {
                    response.Status = 404;
                    response.Message = "Desigation with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(designation, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
            }
            else
            {
                designation = JsonSerializer.Deserialize<Designation>(getDesignationByIdFromCache, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            }

            response.Status = 200;
            response.Message = "Successfully retrieved the designation with given Id";
            response.Designation = new DesignationViewModel(designation);
            return response;
        }

        public async Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel designation, Guid empId)
        {
            DesignationResponseViewModel response = new DesignationResponseViewModel();

            Designation addDesignation = new Designation();
            addDesignation.DesignationName = designation.DesignationName;
            addDesignation.EmpId = empId;

            await _dbContext.Designations.AddAsync(addDesignation);    
            await _dbContext.SaveChangesAsync();

            if(addDesignation == null)
            {
                response.Status = 500;
                response.Message = "Something went wrong, plaese try again.";
                return response;
            }

            await _cache.RemoveAsync($"allDesignations");
            await _cache.RemoveAsync($"allDesignationsWithAdmin");

            response.Status = 200;
            response.Message = "Successfully add designation.";
            response.Designation = new DesignationViewModel(addDesignation);
            return response;
        }

        public async Task<AllDesignationResponseViewModel> GetAllDesignation(bool hasSpecialPrivilege)
        {
            string key = "";
            if (hasSpecialPrivilege)
            {
                key = $"allDesignationsWithAdmin";
            }
            else
            {
                key = $"allDesignations";
            }
            string? getAllDesignationsFromCache = await _cache.GetStringAsync(key);

            List<Designation> list;
            if (string.IsNullOrWhiteSpace(getAllDesignationsFromCache))
            {
                list = _dbContext.Designations.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Designation>>(getAllDesignationsFromCache);
            }

            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            foreach (var item in list)
            {
                if(hasSpecialPrivilege || !string.Equals(item.DesignationName.ToLower(), "admin")) 
                {
                    designations.Add(new DesignationViewModel(item));
                }
            }

            AllDesignationResponseViewModel response = new AllDesignationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully retrieved all designations.";
            response.AllDesignations = designations;
            return response;
        }

        public async Task<DesignationResponseViewModel> RemoveDesignation(int Id)
        {
            DesignationResponseViewModel response = new DesignationResponseViewModel();

            string key = $"getDesignationById-{Id}";
            string? getDesignationByIdFromCache = await _cache.GetStringAsync(key);

            Designation designation;
            if (string.IsNullOrWhiteSpace(getDesignationByIdFromCache))
            {
                designation = _dbContext.Designations.Find(Id);
                if (designation == null)
                {
                    response.Status = 404;
                    response.Message = "Desigation with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(designation));
            }
            else
            {
                designation = JsonSerializer.Deserialize<Designation>(getDesignationByIdFromCache);
            }

            key = $"getEmployeesByDesignationId-{Id}";
            string? getEmployeesByDesignationIdFromCache = await _cache.GetStringAsync(key);

            List<Employee> list;
            if (string.IsNullOrWhiteSpace(getEmployeesByDesignationIdFromCache))
            {
                list = _dbContext.Employees.Where(item => item.DesignationId == Id).ToList();
                if (list.Count != 0)
                {
                    response.Status = 409;
                    response.Message = "Employees with this designationId is present in the system.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Employee>>(getEmployeesByDesignationIdFromCache);
            }

            _dbContext.Designations.Remove(designation);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allDesignations");
            await _cache.RemoveAsync($"allDesignationsWithAdmin");
            await _cache.RemoveAsync($"getDesignationById-{designation.DesignationId}");
            await _cache.RemoveAsync($"getPrivilegeByDesignationId-{designation.DesignationId}");
            await _cache.RemoveAsync($"getEmployeesByDesignationId-{designation.DesignationId}");

            response.Status = 200;
            response.Message = "Successfully removed the designation.";
            response.Designation = new DesignationViewModel(designation);
            return response;
        }
    }
}
