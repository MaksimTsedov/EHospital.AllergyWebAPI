using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.BusinessLogic.Contracts
{
    /// <summary>
    /// Service abstraction for allergy symptoms
    /// </summary>
    public interface IAllergySymptomService
    {
        /// <summary>
        /// Gets all symptoms of patient allergy.
        /// </summary>
        /// <param name="patientAllergyId">The patient allergy identifier.</param>
        /// <returns>Enumeration of allergy symptoms.</returns>
        Task<IEnumerable<Symptom>> GetAllAllergySymptoms(int patientAllergyId);

        /// <summary>
        /// Gets the allergy-symptom pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Symptom of patient allergy.</returns>
        Task<AllergySymptom> GetAllergySymptom(int id);

        /// <summary>
        /// Creates the allergy-symptom pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergySymptom">The allergy-symptom pair.</param>
        /// <returns>Created symptom of patient allergy.</returns>
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
