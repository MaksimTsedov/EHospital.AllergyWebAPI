namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Patient allergy view model. Hides unnecessary returned information.
    /// </summary>
    public class PatientAllergyView
    {
        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        public int Id { get; set; }

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
        public string Allergy { get; set; }

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
