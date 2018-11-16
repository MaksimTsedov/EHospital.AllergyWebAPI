using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.DAL.Entities
{
    /// <summary>
    /// Symptom-allergy entity
    /// </summary>
    public class AllergySymptom
    {
        /// <summary>
        /// Gets or sets the allergy-symptom pair identifier.
        /// </summary>
        /// <value>
        /// The allergy-symptom pair identifier.
        /// </value>
        [Key]
        public int AllergySymptomId { get; set; }

        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        [Required(ErrorMessage = "It is required to give symptom to certain patient allergy.")]
        public int PatientAllergyId { get; set; }

        /// <summary>
        /// Gets or sets the symptom identifier.
        /// </summary>
        /// <value>
        /// The symptom identifier.
        /// </value>
        [Required(ErrorMessage = "Please select symptom.")]
        public int SymptomId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
