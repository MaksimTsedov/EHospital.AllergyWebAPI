using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Allergy view model for filling in
    /// </summary>
    public class AllergyRequest
    {
        private string _pathogen;
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Required(ErrorMessage = "Please enter the pathogen of allergy.")]
        [MaxLength(200, ErrorMessage = "Pathogen can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for pathogen.")]
        public string Pathogen
        {
            get
            {
                return _pathogen;
            }

            set
            {
                log.Info("Creating allergy.");
                _pathogen = value;
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
                        foreach (var member in error.MemberNames)
                        {
                            errorMessage.Append(member);
                        }
                       errorMessage.Append(" : " 
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
