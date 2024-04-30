using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateEducationService : ICandidateEducationService
    {
        private readonly ApplicationDbContext _dbContext;
        public CandidateEducationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MultipleEducationResponseViewModel GetAllEducationOfACandidate(Guid CandidateId)
        {
            MultipleEducationResponseViewModel response = new MultipleEducationResponseViewModel(); 

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<CandidateEducation> allEducationsOfCandidate = _dbContext.CandidateEducations.Where(item => item.CandidateId == CandidateId).ToList();

            List<CandidateEducationViewModel> educations = new List<CandidateEducationViewModel>();
            foreach(var item in allEducationsOfCandidate)
            {
                educations.Add(new CandidateEducationViewModel(item));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all educations by Given Candidate Id.";
            response.CandidateEducation = educations;
            return response;
        }

        public CandidateEducationResponseViewModel GetEducationById(Guid Id)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            var education = _dbContext.CandidateEducations.Find(Id);
            if(education == null)
            {
                response.Status = 404;
                response.Message = "Candidate Education with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate education with given Id.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> AddCandidateEducation(AddCandidateEducationViewModel addCandidateEducation, Guid CandidateId)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            var degree = _dbContext.Degrees.Find(addCandidateEducation.DegreeId);
            if(degree == null)
            {
                response.Status = 404;
                response.Message = "This degree does not exist in database.";
                return response;
            }

            var institution = _dbContext.EducationInstitutions.Find(addCandidateEducation.InstitutionId);
            if(institution == null)
            {
                response.Status = 404;
                response.Message = "This institution does not exist in database.";
                return response;
            }

            CandidateEducation educationToBeAdded = new CandidateEducation
            {
                InstitutionOrSchoolName = addCandidateEducation.InstitutionOrSchoolName,
                StartYear = addCandidateEducation.StartYear,
                EndYear = addCandidateEducation.EndYear,
                Grade = addCandidateEducation.Grade,
                CandidateId = CandidateId,
                InstitutionId = addCandidateEducation.InstitutionId,
                DegreeId = addCandidateEducation.DegreeId
            };

            await _dbContext.CandidateEducations.AddAsync(educationToBeAdded);
            await _dbContext.SaveChangesAsync();
            
            if(educationToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add education, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added candidate education.";
            response.CandidateEducation = new CandidateEducationViewModel(educationToBeAdded);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> UpdateCandidateEducation(UpdateCandidateEducationViewModel updateEducation)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            var education = _dbContext.CandidateEducations.Find(updateEducation.EducationId);
            if (education == null)
            {
                response.Status = 404;
                response.Message = "The candidate education you are trying to update does not exist in database.";
                return response;
            }

            var degree = _dbContext.Degrees.Find(updateEducation.DegreeId);
            if (degree == null)
            {
                response.Status = 404;
                response.Message = "This degree does not exist in database.";
                return response;
            }

            var institution = _dbContext.EducationInstitutions.Find(updateEducation.InstitutionId);
            if (institution == null)
            {
                response.Status = 404;
                response.Message = "This institution does not exist in database.";
                return response;
            }

            education.InstitutionOrSchoolName = updateEducation.InstitutionOrSchoolName;
            education.StartYear = updateEducation.StartYear;
            education.EndYear = updateEducation.EndYear;
            education.Grade = updateEducation.Grade;
            education.InstitutionId = updateEducation.InstitutionId;
            education.DegreeId = updateEducation.DegreeId;

            _dbContext.CandidateEducations.Update(education);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that candidate education.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> RemoveEducation(Guid Id)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            var education = _dbContext.CandidateEducations.Find(Id);
            if (education == null)
            {
                response.Status = 404;
                response.Message = "Candidate Education with this Id does not exist";
                return response;
            }

            _dbContext.CandidateEducations.Remove(education);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that degree.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }
    }
}
