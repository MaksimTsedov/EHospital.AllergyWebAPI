using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Patient info entity
    /// </summary>
    /// <seealso cref="EHospital.Allergies.Model.IBaseEntity" />
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
        /// Gets or sets the patient allergies. EFCore reference on elements due to mapping on.
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
