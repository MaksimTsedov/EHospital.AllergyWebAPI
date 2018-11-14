using System.ComponentModel.DataAnnotations;

namespace EHospital.AllergyAPI.Views
{
    public class SymptomRequest
    {
        [Required(ErrorMessage = "Every symptom has it naming")]
        [MaxLength(200, ErrorMessage = "Symptom can`t have so long naming.")]
        [MinLength(2, ErrorMessage = "Too short naming for symptom.")]
        public string Naming { get; set; }
    }
}
