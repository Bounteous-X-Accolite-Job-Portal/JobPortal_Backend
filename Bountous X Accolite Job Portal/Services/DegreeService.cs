using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DegreeModels;
using Bountous_X_Accolite_Job_Portal.Models.DegreeModels.DegreeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DegreeService : IDegreeService
    {
        private readonly ApplicationDbContext _dbContext;
        public DegreeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DegreeViewModel> GetAllDegree()
        {
            List<Degree> list = _dbContext.Degrees.Where(item => true).ToList();    
            List<DegreeViewModel> response = new List<DegreeViewModel>();
            foreach (var item in list)
            {
                response.Add(new DegreeViewModel(item));
            }

            return response;
        }

        public DegreeResponseViewModel GetDegree(Guid id)
        {
            DegreeResponseViewModel response = new DegreeResponseViewModel();

            var degree = _dbContext.Degrees.Find(id);
            if (degree == null)
            {
                response.Status = 404;
                response.Message = "Degree with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved degree with given Id.";
            response.Degree = new DegreeViewModel(degree);
            return response;
        }

        public async Task<DegreeResponseViewModel> AddDegree(AddDegreeViewModel degree, Guid EmpId)
        {
            Degree degreeToBeAdded = new Degree
            {
                DegreeName = degree.DegreeName,
                DurationInYears = degree.DurationInYears,
                EmpId = EmpId
            };

            await _dbContext.Degrees.AddAsync(degreeToBeAdded);
            await _dbContext.SaveChangesAsync();

            DegreeResponseViewModel response = new DegreeResponseViewModel();
            if (degreeToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add degree, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added degree.";
            response.Degree = new DegreeViewModel(degreeToBeAdded);
            return response;
        }

        public async Task<DegreeResponseViewModel> UpdateDegree(UpdateDegreeViewModel updateDegree)
        {
            DegreeResponseViewModel response = new DegreeResponseViewModel();

            var degree = _dbContext.Degrees.Find(updateDegree.DegreeId);
            if (degree == null)
            {
                response.Status = 404;
                response.Message = "The degree you are trying to update does not exist in database.";
                return response;
            }

            degree.DegreeName = updateDegree.DegreeName;
            degree.DurationInYears = updateDegree.DurationInYears;

            _dbContext.Degrees.Update(degree);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that degree.";
            response.Degree = new DegreeViewModel(degree);
            return response;
        }

        public async Task<DegreeResponseViewModel> RemoveDegree(Guid id)
        {
            DegreeResponseViewModel response = new DegreeResponseViewModel();

            var degree = _dbContext.Degrees.Find(id);
            if (degree == null)
            {
                response.Status = 404;
                response.Message = "Degree with this Id does not exist";
                return response; // data with that Id does not exist
            }

            var education = _dbContext.CandidateEducations.Where(item => item.DegreeId == id).ToList();
            if (education.Count != 0)
            {
                response.Status = 409;
                response.Message = "Candidate Education Section still using this degree.";
                return response;  // conflict
            }

            _dbContext.Degrees.Remove(degree);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that degree.";
            response.Degree = new DegreeViewModel(degree);
            return response;
        }
    }
}
