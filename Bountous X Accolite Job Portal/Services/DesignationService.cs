using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly ApplicationDbContext _dbContext;

        public DesignationService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public bool HasSpecialPrivilege(string role)
        {
            if (String.Equals(role.ToLower(), "admin"))
            {
                return true;
            }

            return false;
        }

        public bool HasPrivilege(string role)
        {
            var designation = _dbContext.Designations.Where(item => String.Equals(item.DesignationName.ToLower(), role.ToLower())).FirstOrDefault();
            if(designation == null)
            {
                return false;
            }

            var check = _dbContext.DesignationWhichHasPrivileges.Where(item => item.DesignationId == designation.DesignationId).FirstOrDefault();
            if(check == null)
            {
                return false;
            }

            return true;
        }

        public async Task<DesignationResponseViewModel> GetDesignationById(int Id)
        {
            DesignationResponseViewModel response = new DesignationResponseViewModel();

            var designation = _dbContext.Designations.Find(Id);
            if(designation == null)
            {
                response.Status = 404;
                response.Message = "Desigation with this Id does not exist.";
                return response;
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

            response.Status = 200;
            response.Message = "Successfully add designation.";
            response.Designation = new DesignationViewModel(addDesignation);
            return response;
        }

        public AllDesignationResponseViewModel GetAllDesignation()
        {
            List<Designation> list = _dbContext.Designations.ToList();

            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            foreach (var item in list)
            {
                if(!string.Equals(item.DesignationName.ToLower(), "admin")) 
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

            var designation = _dbContext.Designations.Find(Id);
            if(designation == null)
            {
                response.Status = 404;
                response.Message = "Designation with this Id does not exist.";
                return response;
            }

            List<Employee> list = _dbContext.Employees.Where(item => item.DesignationId == Id).ToList();

            if(list.Count != 0)
            {
                response.Status = 409;
                response.Message = "Employees with this designationId is present in the system.";
                return response;
            }

            _dbContext.Designations.Remove(designation);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed the designation.";
            response.Designation = new DesignationViewModel(designation);
            return response;
        }
    }
}
