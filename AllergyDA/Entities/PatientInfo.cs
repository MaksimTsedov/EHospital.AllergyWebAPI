using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.DAL.Entities
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
    }
}
