using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.BusinessLogic.Contracts;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.BusinessLogic.Services
{
    /// <summary>
    /// Allergy service business logic
    /// </summary>
    /// <seealso cref="IAllergyService" />
    public class AllergyService : IAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AllergyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all allergies.
        /// </summary>
        /// <returns>
        /// Enumeration of allergies.
        /// </returns>
        public async Task<IEnumerable<Allergy>> GetAllAllergies()
        {
            var result = _unitOfWork.Allergies.GetAll().OrderBy(a => a.Pathogen);
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Gets the allergy.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Allergy.
        /// </returns>
        /// <exception cref="ArgumentNullException">Allergy doesn`t exist.</exception>
        public async Task<Allergy> GetAllergy(int id)
        {
            var result = await _unitOfWork.Allergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Allergy doesn`t exist.");
            }

            return result;
        }

        /// <summary>
        /// Searches the allergies by name beginning.
        /// </summary>
        /// <param name="searchKey">The search key.</param>
        /// <returns>
        /// Enumeration of allergies with start substring.
        /// </returns>
        public async Task<IEnumerable<Allergy>> SearchAllergiesByName(string searchKey)
        {
            var result = _unitOfWork.Allergies.GetAll(a => a.Pathogen.StartsWith(searchKey))
                                                       .OrderBy(a => a.Pathogen);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Creates the allergy asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergy">The input validated allergy.</param>
        /// <returns>
        /// Created allergy.
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
        /// <param name="id">The identifier of allergy.</param>
        /// <returns>
        /// Deleted allergy.
        /// </returns>
        /// <exception cref="ArgumentNullException">No allergy found.</exception>
        /// <exception cref="InvalidOperationException">There are exist records with involvement of this allergy.</exception>
        public async Task<Allergy> DeleteAllergyAsync(int id)
        {
            var result = await _unitOfWork.Allergies.Get(id);

            if (result == null)
            {
                throw new ArgumentNullException("No allergy found.");
            }

            if (_unitOfWork.PatientAllergies.GetAll().Any(a => a.AllergyId == result.Id))
            {
                throw new InvalidOperationException("There are exist records with involvement of this allergy.");
            }

            result.IsDeleted = true;
            _unitOfWork.Allergies.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
