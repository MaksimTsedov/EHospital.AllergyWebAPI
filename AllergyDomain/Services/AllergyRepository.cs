using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.AllergyDA.Contracts;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDomain.Contracts;

namespace EHospital.AllergyDomain.Services
{
    public class AllergyRepository : IAllergyRepository
    {
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyRepository" /> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public AllergyRepository(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        /// <summary>
        /// Gets all allergies.
        /// </summary>
        /// <returns>
        /// Enumeration of allergies.
        /// </returns>
        /// <exception cref="NullReferenceException">No records in db.</exception>
        public IQueryable<Allergy> GetAllAllergies()
        {
            var result = _unitOfWork.Allergies.GetAll().Where(a => !a.IsDeleted);
            if (result.Count() == 0)
            {
                throw new NullReferenceException("No records in db.");
            }

            return result.OrderBy(a => a.Pathogen);
        }

        /// <summary>
        /// Gets the allergy.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Allergy.
        /// </returns>
        /// <exception cref="NullReferenceException">Allergy doesn`t exist.</exception>
        public Allergy GetAllergy(int id)
        {
            var result = _unitOfWork.Allergies.Get(id);
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Allergy doesn`t exist.");
            }

            return result;
        }

        /// <summary>
        /// Searches the allergies by name beginning.
        /// </summary>
        /// <param name="beginning">The beginning of naming.</param>
        /// <returns>
        /// Enumeration of allergies with start substring.
        /// </returns>
        /// <exception cref="NullReferenceException">Not found any allergy.</exception>
        public IQueryable<Allergy> SearchAllergiesByName(string beginning)
        {
            var result = _unitOfWork.Allergies.GetAll(a => a.Pathogen.StartsWith(beginning)).
                                               Where(a => !a.IsDeleted);
            if (result.Count() == 0)
            {
                throw new NullReferenceException("Not found any allergy.");
            }

            return result.OrderBy(a => a.Pathogen);
        }

        /// <summary>
        /// Creates the allergy asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergy">The allergy.</param>
        /// <returns>
        /// Inserted allergy.
        /// </returns>
        /// <exception cref="ArgumentException">Duplicate allergy.</exception>
        public async Task<Allergy> CreateAllergyAsync(Allergy allergy)
        {
            if (_unitOfWork.Allergies.GetAll().Any(a => a.Pathogen == allergy.Pathogen))
            {
                throw new ArgumentException("Duplicate allergy.");
            }

            Allergy result = _unitOfWork.Allergies.Insert(allergy);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Deletes the allergy asynchronous from db.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>
        /// Deleted allergy.
        /// </returns>
        /// <exception cref="NullReferenceException">No allergy found.</exception>
        /// <exception cref="ArgumentException">There are exist records with involvment of this allergy.</exception>
        public async Task<Allergy> DeleteAllergyAsync(int id)
        {
            var result = _unitOfWork.Allergies.Get(id);

            if (result == null)
            {
                throw new NullReferenceException("No allergy found.");
            }

            if (_unitOfWork.PatientAllergies.GetAll().Count() != 0)
            {
                if (_unitOfWork.PatientAllergies.GetAll().Any(a => a.AllergyId == result.AllergyId))
                {
                    throw new ArgumentException("There are exist records with involvment of this allergy.");
                }
            }

            result.IsDeleted = true;
            _unitOfWork.Allergies.Delete(result);
            await _unitOfWork.Save();
            return result;
        }      
    }
}
