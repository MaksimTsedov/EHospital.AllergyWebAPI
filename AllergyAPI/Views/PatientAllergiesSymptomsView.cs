using System.Collections.Generic;

namespace EHospital.Allergies.WebAPI.Views
{
    public class PatientAllergiesSymptomsView
    {
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
        /// Gets or sets the symptoms.
        /// </summary>
        /// <value>
        /// The symptoms.
        /// </value>
        public List<string> Symptoms { get; set; }
    }
}