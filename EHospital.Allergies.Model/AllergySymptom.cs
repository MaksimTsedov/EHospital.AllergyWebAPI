using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Symptom-allergy entity
    /// </summary>
    public class AllergySymptom : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the allergy-symptom pair identifier.
        /// </summary>
        /// <value>
        /// The allergy-symptom pair identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        [Required(ErrorMessage = "It is required to give symptom to certain patient allergy.")]
        public int PatientAllergyId { get; set; }

        /// <summary>
        /// Gets or sets the patient allergy.
        /// </summary>
        /// <value>
        /// The patient allergy.
        /// </value>
        public PatientAllergy PatientAllergy { get; set; }

        /// <summary>
        /// Gets or sets the symptom identifier.
        /// </summary>
        /// <value>
        /// The symptom identifier.
        /// </value>
        [Required(ErrorMessage = "Please select symptom.")]
        public int SymptomId { get; set; }

        /// <summary>
        /// Gets or sets the symptom.
        /// </summary>
        /// <value>
        /// The symptom.
        /// </value>
        public Symptom Symptom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
