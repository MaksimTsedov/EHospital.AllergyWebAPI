﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.DAL.Entities
{
    /// <summary>
    /// Patient-allergy entity 
    /// </summary>
    public class PatientAllergy
    {
        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        [Key]
        public int PatientAllergyId { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Maps the specified patient allergy to this instance.
        /// </summary>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        public void Map(PatientAllergy patientAllergy)
        {
            this.AllergyId = patientAllergy.AllergyId;
            this.Duration = patientAllergy.Duration;
            this.Notes = patientAllergy.Notes;
        }

        /// <summary>
        /// Clones the notes to this instance.
        /// </summary>
        /// <param name="notesToClone">The notes to clone.</param>
        public void CloneNotes(string notesToClone)
        {
            this.Notes = notesToClone;
        }
    }
}
