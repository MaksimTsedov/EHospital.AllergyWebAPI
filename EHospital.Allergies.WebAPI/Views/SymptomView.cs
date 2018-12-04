namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Symptom view model. Hides unnecessary returned information.
    /// </summary>
    public class SymptomView
    {
        /// <summary>
        /// Gets or sets the identifier of symptom.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the naming of symptom.
        /// </summary>
        /// <value>
        /// The naming.
        /// </value>
        public string Naming { get; set; }
    }
}
