using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EducationInstitutionService : IEducationInstitutionService
    {
        private readonly ApplicationDbContext _dbContext;
        public EducationInstitutionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<InstitutionViewModel> GetAllInstitutions()
        {
            List<EducationInstitution> list = _dbContext.EducationInstitutions.Where(item => true).ToList();
            List<InstitutionViewModel> response = new List<InstitutionViewModel>();
            foreach (var item in list)
            {
                response.Add(new InstitutionViewModel(item));
            }

            return response;
        }

        public InstitutionResponseViewModel GetInstitution(Guid id)
        {
            InstitutionResponseViewModel response = new InstitutionResponseViewModel(); 

            var institution = _dbContext.EducationInstitutions.Find(id);
            if (institution == null)
            {
                response.Status = 404;
                response.Message = "Institution with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved institution with given Id.";
            response.EducationInstitution = new InstitutionViewModel(institution);
            return response;
        }

        public async Task<InstitutionResponseViewModel> AddInstitution(AddInstitutionViewModel institution, Guid EmpId)
        {
            EducationInstitution institutionToBeAdded = new EducationInstitution();
            institutionToBeAdded.EmpId = EmpId;
            institutionToBeAdded.InstitutionOrSchool = institution.InstitutionOrSchool;
            institutionToBeAdded.UniversityOrBoard = institution.UniversityOrBoard;

            await _dbContext.EducationInstitutions.AddAsync(institutionToBeAdded);
            await _dbContext.SaveChangesAsync();

            InstitutionResponseViewModel response = new InstitutionResponseViewModel();
            if(institutionToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add institution, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added institution.";
            response.EducationInstitution = new InstitutionViewModel(institutionToBeAdded);
            return response;
        }

        public async Task<InstitutionResponseViewModel> UpdateInstution(UpdateInstitutionViewModel updateInstitution)
        {
            InstitutionResponseViewModel response = new InstitutionResponseViewModel();

            var institution = _dbContext.EducationInstitutions.Find(updateInstitution.InstitutionId);
            if(institution == null)
            {
                response.Status = 404;
                response.Message = "The institution you are trying to update does not exist in database.";
                return response;
            }

            institution.InstitutionOrSchool = updateInstitution.InstitutionOrSchool;
            institution.UniversityOrBoard = updateInstitution.UniversityOrBoard;

            _dbContext.EducationInstitutions.Update(institution);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that institution.";
            response.EducationInstitution = new InstitutionViewModel(institution);
            return response;
        }

        public async Task<InstitutionResponseViewModel> RemoveInstitution(Guid id)
        {
            InstitutionResponseViewModel res = new InstitutionResponseViewModel();

            var institution = _dbContext.EducationInstitutions.Find(id);
            if (institution == null)
            {
                res.Status = 404;
                res.Message = "Institution with this Id does not exist";
                return res; // data with that Id does not exist
            }

            var education = _dbContext.CandidateEducations.Where(item => item.InstitutionId == id).ToList();
            if (education.Count != 0)
            {
                res.Status = 409;
                res.Message = "Candidate Education Section still using this institution.";
                return res;  // conflict
            }

            _dbContext.EducationInstitutions.Remove(institution);
            await _dbContext.SaveChangesAsync();

            res.Status = 200;
            res.Message = "Successfully removed that institution.";
            res.EducationInstitution = new InstitutionViewModel(institution);
            return res;
        }
    }
}
