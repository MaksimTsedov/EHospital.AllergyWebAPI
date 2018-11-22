using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EHospital.Allergies.WebAPI.Views
{
    public class SymptomRequest
    {

        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
                log.Info("Creating symptom.");
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

                    log.Error(errorMessage.ToString());
                    return;
                }
            }
        }
    }
}
