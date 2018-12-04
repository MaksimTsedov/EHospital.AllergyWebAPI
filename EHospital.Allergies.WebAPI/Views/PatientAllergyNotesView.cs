namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// View model for notes of patient allergy
    /// </summary>
    public class PatientAllergyNotesView
    {
        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the notes for allergy of patient.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }
    }
}
