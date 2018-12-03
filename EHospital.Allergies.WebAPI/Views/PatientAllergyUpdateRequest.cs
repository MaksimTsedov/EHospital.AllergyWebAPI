using EHospital.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EHospital.Allergies.WebAPI.Views
{
    public class PatientAllergyUpdateRequest
    {
        private string _notes;

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
        public string Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                LoggingToFile.LoggingInfo("Updating patient allergy information.");
                _notes = value;
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
