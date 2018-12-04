using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Allergies.BusinesLogic.Services
{
    /// <summary>
    /// Patient allergy service business logic
    /// </summary>
    /// <seealso cref="EHospital.Allergies.BusinesLogic.Contracts.IPatientAllergyService" />
    public class PatientAllergyService : IPatientAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientAllergyService" /> class.
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
        public async Task<IEnumerable<PatientAllergy>> GetAllPatientAllergies(int patientId)
        {
            var result = await Task.FromResult(_unitOfWork.PatientAllergies.Include(pa => pa.Allergy)                                         
                                               .Include(a => a.AllergySymptoms)                                              
                                               .ThenInclude(a => (a as AllergySymptom).Symptom)
                                               .Where(a => a.PatientId == patientId));
            return result;
        }

        /// <summary>
        /// Gets the patient-allergy pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> GetPatientAllergy(int id)
        {
            var result = await Task.FromResult(_unitOfWork.PatientAllergies.Include(pa => pa.Allergy)
                                                                           .FirstOrDefault(pa => pa.Id == id));
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
        /// Created patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Not found such patient.
        /// or
        /// Not found such allergy.
        /// </exception>
        /// <exception cref="ArgumentException">Duplicate patient-allergy pair.</exception>
        public async Task<PatientAllergy> CreatePatientAllergyAsync(PatientAllergy patientAllergy)
        {
            Task<PatientInfo> patientInfo = _unitOfWork.PatientInfo.Get(patientAllergy.PatientId);
            Task<Allergy> allergy = _unitOfWork.Allergies.Get(patientAllergy.AllergyId);
            Task.WaitAll(patientInfo, allergy);

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

            patientAllergy.Allergy = allergy.Result;
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
        /// <exception cref="ArgumentException">Duplicate patient-allergy pair.</exception>
        public async Task<PatientAllergy> UpdatePatientAllergyAsync(int id, PatientAllergy patientAllergy)
        {
            PatientAllergy result = await _unitOfWork.PatientAllergies.Get(id);
            Allergy allergyUpdate = await _unitOfWork.Allergies.Get(patientAllergy.AllergyId);
        
            if (result == null || allergyUpdate == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            if (result.AllergyId != patientAllergy.AllergyId)
            {
                if (_unitOfWork.PatientAllergies.GetAll().Any(a => a.AllergyId == patientAllergy.AllergyId
                                                         && a.PatientId == result.PatientId))
                {
                    throw new ArgumentException("Duplicate patient-allergy pair.");
                }
            }
            
            result.Assign(patientAllergy);
            _unitOfWork.PatientAllergies.Update(result);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Updates the notes of patient-allergy pair asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="notes">New notes.</param>
        /// <returns>
        /// Updated patient-allergy pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> UpdateNotesAsync(int id, string notes)
        {
            var result = await _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            result.SetNotes(notes);
            _unitOfWork.PatientAllergies.Update(result);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Deletes the patient-allergy pair asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentNullException">Patient-allergy pair doesn`t exist.</exception>
        public async Task DeletePatientAllergyAsync(int id)
        {
            var result = await _unitOfWork.PatientAllergies.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Patient-allergy pair doesn`t exist.");
            }

            _unitOfWork.CascadeDeletePatientAllergy(id);
            await _unitOfWork.Save();
        }
    }
}
