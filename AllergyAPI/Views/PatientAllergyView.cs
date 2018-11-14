namespace EHospital.AllergyAPI.Views
{
    public class PatientAllergyView
    {
        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        public int PatientAllergyId { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the allergy.
        /// </summary>
        /// <value>
        /// The allergy.
        /// </value>
        public int AllergyId { get; set; }

        /// <summary>
        /// Gets or sets the duration of allergy.
        /// </summary>
        /// <value>
        /// The allergy duration.
        /// </value>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the notes for allergy of patient.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }
    }
}
