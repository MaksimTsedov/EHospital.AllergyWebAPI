using EHospital.Allergies.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHospital.Allergies.BusinesLogic.Contracts
{
    public interface IAllergyService
    {
        /// <summary>
        /// Gets all allergies.
        /// </summary>
        /// <returns>Enumeration of allergies.</returns>
        Task<IEnumerable<Allergy>> GetAllAllergies();

        /// <summary>
        /// Searches the allergies by name beginning.
        /// </summary>
        /// <param name="beginning">The beginning of naming.</param>
        /// <returns>
        /// Enumeration of allergies with start substring.
        /// </returns>
        Task<IEnumerable<Allergy>> SearchAllergiesByName(string beginning);

        /// <summary>
        /// Gets the allergy.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Allergy.</returns>
        Task<Allergy> GetAllergy(int id);

        /// <summary>
        /// Creates the allergy asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergy">The allergy.</param>
        /// <returns>Allergy.</returns>
        Task<Allergy> CreateAllergyAsync(Allergy allergy);

        /// <summary>
        /// Deletes the allergy asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted allergy.
        /// </returns>
        Task<Allergy> DeleteAllergyAsync(int id);
    }
}
