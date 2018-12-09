using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.BusinessLogic.Contracts
{
    /// <summary>
    /// Service abstraction for allergies of patient
    /// </summary>
    public interface IPatientAllergyService
    {
        /// <summary>
        /// Gets all patient allergies.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>
        /// List of allergies of chosen patient.
        /// </returns>
        Task<IEnumerable<PatientAllergy>> GetAllPatientAllergies(int patientId);

        /// <summary>
        /// Gets the patient-allergy pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Patient-allergy pair.</returns>
        Task<PatientAllergy> GetPatientAllergy(int id);

        /// <summary>
        /// Creates the patient-allergy pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        /// <returns>Created patient-allergy pair.</returns>
        Task<PatientAllergy> CreatePatientAllergyAsync(PatientAllergy patientAllergy);

        /// <summary>
        /// Updates the patient-allergy pair asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        /// <returns>Updated patient-allergy pair.</returns>
        Task<PatientAllergy> UpdatePatientAllergyAsync(int id, PatientAllergy patientAllergy);

        /// <summary>
        /// Updates the notes of patient-allergy pair asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="notes">New notes.</param>
        /// <returns>
        /// Updated patient-allergy pair.
        /// </returns>
        Task<PatientAllergy> UpdateNotesAsync(int id, string notes);

        /// <summary>
        /// Deletes the patient-allergy pair asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task DeletePatientAllergyAsync(int id);
    }
}
