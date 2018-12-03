using EHospital.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EHospital.Allergies.WebAPI.Views
{
    public class SymptomRequest
    {
        private string _naming;

        [Required(ErrorMessage = "Every symptom has it naming")]
        [MaxLength(200, ErrorMessage = "Symptom can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for symptom.")]
        public string Naming
        {
            get
            {
                return _naming;
            }

            set
            {
                LoggingToFile.LoggingInfo("Creating symptom.");
                _naming = value;
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
