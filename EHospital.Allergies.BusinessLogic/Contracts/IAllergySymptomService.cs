using EHospital.Allergies.Model;
using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Allergies.BusinesLogic.Contracts
{
    public interface IAllergySymptomService
    {
        /// <summary>
        /// Gets all allergy symptoms.
        /// </summary>
        /// <param name="patientAllergyId">The allergy identifier.</param>
        /// <returns>Enumeration of allergy symptoms.</returns>
        IQueryable<Symptom> GetAllAllergySymptoms(int patientAllergyId);

        /// <summary>
        /// Gets the allergy-symptom pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Allergy-symptom pair.</returns>
        AllergySymptom GetAllergySymptom(int id);

        /// <summary>
        /// Creates the allergy-symptom pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergySymptom">The allergy-symptom pair.</param>
        /// <returns>Allergy-symptom pair.</returns>
        Task<AllergySymptom> CreateAllergySymptomAsync(AllergySymptom allergySymptom);

        /// <summary>
        /// Deletes the allergy-symptom pair asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted allergy-symptom
        /// </returns>
        Task<AllergySymptom> DeleteAllergySymptomAsync(int id);
    }
}
