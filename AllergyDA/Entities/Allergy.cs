﻿using System.ComponentModel.DataAnnotations;

namespace EHospital.AllergyDA.Entities
{
    /// <summary>
    /// Allergy entity
    /// </summary>
    public class Allergy
    {
        /// <summary>
        /// Gets or sets the allergy identifier.
        /// </summary>
        /// <value>
        /// The allergy identifier.
        /// </value>
        [Key]
        public int AllergyId { get; set; }

        /// <summary>
        /// Gets or sets the pathogen.
        /// </summary>
        /// <value>
        /// The pathogen.
        /// </value>
        [Required(ErrorMessage = "Please enter the pathogen of allergy.")]
        [MaxLength(200, ErrorMessage = "Pathogen can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for pathogen.")]
        public string Pathogen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
