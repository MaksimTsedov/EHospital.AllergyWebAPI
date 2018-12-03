using EHospital.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EHospital.Allergies.WebAPI.Views
{
    public class AllergySymptomRequest
    {
        private int _symptomId;

        /// <summary>
        /// Gets or sets the patient-allergy pair identifier.
        /// </summary>
        /// <value>
        /// The patient-allergy pair identifier.
        /// </value>
        [Required(ErrorMessage = "It is required to give symptom to certain patient allergy.")]
        public int PatientAllergyId { get; set; }

        /// <summary>
        /// Gets or sets the symptom identifier.
        /// </summary>
        /// <value>
        /// The symptom identifier.
        /// </value>
        [Required(ErrorMessage = "Please select symptom.")]
        public int SymptomId
        {
            get
            {
                return _symptomId;
            }

            set
            {
                LoggingToFile.LoggingInfo("Assignment symptom to allergy of patient.");
                _symptomId = value;
                ValidationContext context = new ValidationContext(this);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(this, context, results, true);
                if (!isValid)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.Append($"Invalid build of {context.DisplayName} :"
                                                            + Environment.NewLine);
                    foreach (var error in results)
                    {
                        errorMessage.Append(error.MemberNames.First() + " : "
                                          + error.ErrorMessage + " ; "
                                          + Environment.NewLine);
                    }

                    LoggingToFile.LoggingError(errorMessage.ToString());
                    return;
                }
            }
        }
    }
}
