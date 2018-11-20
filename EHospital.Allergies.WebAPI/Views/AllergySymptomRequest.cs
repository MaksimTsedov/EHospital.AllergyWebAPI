using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.WebAPI.Views
{
    public class AllergySymptomRequest
    {
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
    }
}
