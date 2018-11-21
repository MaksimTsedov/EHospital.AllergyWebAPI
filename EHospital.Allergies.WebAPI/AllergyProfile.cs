using AutoMapper;
using EHospital.Allergies.Model;
using EHospital.Allergies.WebAPI.Views;
using System.Linq;

namespace EHospital.Allergies.WebAPI
{
    public class AllergyProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyProfile"/> class.
        /// </summary>
        public AllergyProfile()
        {
            //Allergy
            CreateMap<Allergy, AllergyView>();
            CreateMap<AllergyRequest, Allergy>().ConvertUsing(arg =>
            {
                return new Allergy()
                {
                    Pathogen = arg.Pathogen
                };
            });

            //AllergySymptom
            CreateMap<AllergySymptom, AllergySymptomView>().
                                ForMember(dest => dest.SymptomName, opt => opt.MapFrom
                                (src => src.Symptom.Naming));
            CreateMap<AllergySymptomRequest, AllergySymptom>().ConvertUsing(arg =>
            {
                return new AllergySymptom()
                {
                    PatientAllergyId = arg.PatientAllergyId,
                    SymptomId = arg.SymptomId
                };
            });

            //PatientAllergy
            CreateMap<PatientAllergy, PatientAllergiesSymptomsView>()
                                .ForMember(dest => dest.Allergy, opt => opt.MapFrom
                                (src => src.Allergy.Pathogen))
                                .ForMember(dest => dest.Duration, opt => opt.MapFrom
                                (src => src.Duration))
                                .ForMember(dest => dest.Symptoms, opt => opt.MapFrom
                                (src => src.AllergySymptoms.Select(c => c.Symptom.Naming)));
            CreateMap<PatientAllergy, PatientAllergyView>().
                                ForMember(dest => dest.Allergy, opt => opt.MapFrom
                                (src => src.Allergy.Pathogen));           
            CreateMap<PatientAllergyRequest, PatientAllergy>().ConvertUsing(arg =>
            {
                return new PatientAllergy()
                {
                    AllergyId = arg.AllergyId,
                    Duration = arg.Duration,
                    Notes = arg.Notes
                };
            });
            CreateMap<PatientAllergy, PatientAllergyNotesView>().
                ForMember(desc => desc.Notes, opt => opt.MapFrom(c => c.Notes));

            //Symptoms
            CreateMap<Symptom, SymptomView>();
            CreateMap<SymptomRequest, Symptom>().ConvertUsing(arg =>
            {
                return new Symptom()
                {
                    Naming = arg.Naming
                };
            });
        }
    }
}
