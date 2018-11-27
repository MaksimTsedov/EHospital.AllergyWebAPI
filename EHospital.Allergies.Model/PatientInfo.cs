using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    public class PatientInfo : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the patient information identifier.
        /// </summary>
        /// <value>
        /// The patient information identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the patient allergies.
        /// </summary>
        /// <value>
        /// The patient allergies.
        /// </value>
        public ICollection<PatientAllergy> PatientAllergies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
