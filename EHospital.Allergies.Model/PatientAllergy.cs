using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Patient-allergy entity 
    /// </summary>
    public class PatientAllergy : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        [Required(ErrorMessage = "Please select patient.")]
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the patient. EFCore reference on element due to mapping on.
        /// </summary>
        /// <value>
        /// The patient.
        /// </value>
        public PatientInfo Patient { get; set; }

        /// <summary>
        /// Gets or sets the allergy identifier.
        /// </summary>
        /// <value>
        /// The allergy identifier.
        /// </value>
        [Required(ErrorMessage = "Please select allergy.")]
        public int AllergyId { get; set; }

        /// <summary>
        /// Gets or sets the allergy. EFCore reference on element due to mapping on.
        /// </summary>
        /// <value>
        /// The allergy.
        /// </value>
        public Allergy Allergy { get; set; }

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
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the deletion date.
        /// </summary>
        /// <value>
        /// The deletion date.
        /// </value>
        public DateTime? DeletionDate { get; set; }

        /// <summary>
        /// Gets or sets the allergy symptoms. EFCore reference on elements due to mapping on.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        public ICollection<AllergySymptom> AllergySymptoms { get; set; }

        /// <summary>
        /// Maps the specified patient allergy to this instance.
        /// </summary>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        public void Assign(PatientAllergy patientAllergy)
        {
            AllergyId = patientAllergy.AllergyId;
            Duration = patientAllergy.Duration;
            Notes = patientAllergy.Notes;
        }

        /// <summary>
        /// Clones the notes to this instance.
        /// </summary>
        /// <param name="notesToSet">The notes.</param>
        public void SetNotes(string notesToSet)
        {
            Notes = notesToSet;
        }
    }
}
