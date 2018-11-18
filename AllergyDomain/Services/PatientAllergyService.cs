using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.BusinesLogic.Services
{
    public class PatientAllergyService : IPatientAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PatientAllergyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all patient allergies.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>
        /// List of allergies of chosen patient.
        /// </returns>
        /// <exception cref="NullReferenceException">Not found any allergy of chosen patient.</exception>
        public IQueryable<PatientAllergy> GetAllPatientAllergies(int patientId)
        {
            return _unitOfWork.PatientAllergies.GetAll(a => a.PatientId == patientId);
        }

        /// <summary>
        /// Gets the patient-allergy pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public PatientAllergy GetPatientAllergy(int id)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            return result;
        }

        /// <summary>
        /// Creates the patient-allergy pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        /// <returns>
        /// Patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Not found such patient.
        /// or
        /// Not found such allergy.</exception>
        /// <exception cref="ArgumentException">Duplicate patient-allergy pair.</exception>
        public async Task<PatientAllergy> CreatePatientAllergyAsync(PatientAllergy patientAllergy)
        {
            PatientInfo patientInfo = _unitOfWork.PatientInfo.Get(patientAllergy.PatientId);
            Allergy allergy = _unitOfWork.Allergies.Get(patientAllergy.AllergyId);
            if (patientInfo == null)
            {
                throw new ArgumentNullException("Not found such patient.");
            }

            if (allergy == null)
            {
                throw new ArgumentNullException("Not found such allergy.");
            }

            if (_unitOfWork.PatientAllergies.GetAll().Any(a => a.AllergyId == patientAllergy.AllergyId
                                                         && a.PatientId == patientAllergy.PatientId))
            {
                throw new ArgumentException("Duplicate patient-allergy pair.");
            }

            PatientAllergy result = _unitOfWork.PatientAllergies.Insert(patientAllergy);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Updates the patient-allergy pair asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patientAllergy">The patient-allergy pair.</param>
        /// <returns>
        /// Updated patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> UpdatePatientAllergyAsync(int id, PatientAllergy patientAllergy)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            result.Bind(patientAllergy);
            _unitOfWork.PatientAllergies.Update(result);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Updates the notes of patient-allergy pair asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        /// Updated patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> UpdateNotesAsync(int id, string notes)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            result.SetNotes(notes);
            _unitOfWork.PatientAllergies.Update(result);
            await _unitOfWork.Save();
            return result;
        }

        // HACK: Used stored procedure        
        /// <summary>
        /// Deletes the patient-allergy pair asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        /// <exception cref="NullReferenceException">Patient-allergy pair doesn`t exist.</exception>
        public async Task DeletePatientAllergyAsync(int id)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            _unitOfWork.CascadeDeletePatientAllergy(id);
            await _unitOfWork.Save();
        }
    }
}
