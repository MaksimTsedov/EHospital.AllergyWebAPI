namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Allergy symptom view model. Hides unnecessary returned information.
    /// </summary>
    public class AllergySymptomView
    {
        /// <summary>
        /// Gets or sets the allergy-symptom pair identifier.
        /// </summary>
        /// <value>
        /// The allergy-symptom pair identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        public int PatientAllergyId { get; set; }

        /// <summary>
        /// Gets or sets the symptom.
        /// </summary>
        /// <value>
        /// The symptom.
        /// </value>
        public string SymptomName { get; set; }
    }
}
