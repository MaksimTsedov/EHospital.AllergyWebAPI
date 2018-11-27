using System.ComponentModel.DataAnnotations;

namespace EHospital.Allergies.WebAPI.Views
{
    /// <summary>
    /// Allergy view model for filling in
    /// </summary>
    public class AllergyRequest
    {
        [Required(ErrorMessage = "Please enter the pathogen of allergy.")]
        [MaxLength(200, ErrorMessage = "Pathogen can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for pathogen.")]
        public string Pathogen { get; set; }
    }
}
