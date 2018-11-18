using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Symptom entity
    /// </summary>
    public class Symptom : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the symptom identifier.
        /// </summary>
        /// <value>
        /// The symptom identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the naming.
        /// </summary>
        /// <value>
        /// The naming.
        /// </value>
        [Required(ErrorMessage = "Every symptom has it naming")]
        [MaxLength(200, ErrorMessage = "Symptom can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for symptom.")]
        public string Naming { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the allergy symptoms.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        public ICollection<AllergySymptom> AllergySymptoms { get; set; }
    }
}
