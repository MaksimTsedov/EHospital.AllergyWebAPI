using System;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDA.Contracts;
using System.Threading.Tasks;

namespace EHospital.AllergyDA.Contracts
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
        /// Saves this instance into db.
        /// </summary>
        Task Save();
    }
}
