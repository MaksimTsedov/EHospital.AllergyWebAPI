using System.ComponentModel.DataAnnotations;

namespace EHospital.AllergyAPI.Views
{
    public class PatientAllergyRequest
    {
        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        [Required(ErrorMessage = "Please select patient.")]
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the allergy identifier.
        /// </summary>
        /// <value>
        /// The allergy identifier.
        /// </value>
        [Required(ErrorMessage = "Please select allergy.")]
        public int AllergyId { get; set; }

        /// <summary>
        /// Gets or sets the duration of allergy.
        /// </summary>
        /// <value>
        /// The allergy duration.
        /// </value>
        [MaxLength(25, ErrorMessage = "Please, input duration in one calculus(like '3 years' or '5 monthes').")]
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the notes for allergy of patient.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [MaxLength(500, ErrorMessage = "Too long note, please shorten it.")]
        public string Notes { get; set; }
    }
}
