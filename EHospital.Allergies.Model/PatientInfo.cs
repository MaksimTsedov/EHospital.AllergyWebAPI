using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    public class PatientInfo
    {
        /// <summary>
        /// Gets or sets the patient information identifier.
        /// </summary>
        /// <value>
        /// The patient information identifier.
        /// </value>
        [Key]
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the patient allergies.
        /// </summary>
        /// <value>
        /// The patient allergies.
        /// </value>
        public ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}
