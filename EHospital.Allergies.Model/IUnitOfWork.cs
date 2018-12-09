using System;
using System.Threading.Tasks;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Abstraction about union of needed entities
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the allergies.
        /// </summary>
        /// <value>
        /// The allergies.
        /// </value>
        IRepository<Allergy> Allergies { get; }

        /// <summary>
        /// Gets the symptoms.
        /// </summary>
        /// <value>
        /// The symptoms.
        /// </value>
        IRepository<Symptom> Symptoms { get; }

        /// <summary>
        /// Gets the patient allergies.
        /// </summary>
        /// <value>
        /// The patient allergies.
        /// </value>
        IRepository<PatientAllergy> PatientAllergies { get; }

        /// <summary>
        /// Gets the allergy symptoms.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        IRepository<AllergySymptom> AllergySymptoms { get; }

        /// <summary>
        /// Gets the patients information.
        /// </summary>
        /// <value>
        /// The patients information.
        /// </value>
        IRepository<PatientInfo> PatientInfo { get; }

        /// <summary>
        /// Soft cascade delete patient-allergy pair. Related on stored procedure.
        /// </summary>
        /// <param name="id">The identifier of patient-allergy pair.</param>
        void CascadeDeletePatientAllergy(int id);

        /// <summary>
        /// Saves changes into db.
        /// </summary>
        Task Save();
    }
}
