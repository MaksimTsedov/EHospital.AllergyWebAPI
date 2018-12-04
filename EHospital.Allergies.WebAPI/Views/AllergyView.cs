namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Allergy view model. Hides unnecessary returned information.
    /// </summary>
    public class AllergyView
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the naming of allergy.
        /// </summary>
        /// <value>
        /// The pathogen.
        /// </value>
        public string Pathogen { get; set; }
    }
}
