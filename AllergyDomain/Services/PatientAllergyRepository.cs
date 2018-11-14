using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.AllergyDA.Contracts;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDomain.Contracts;

namespace EHospital.AllergyDomain.Services
{
    public class PatientAllergyRepository : IPatientAllergyRepository
    {
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomRepository" /> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public PatientAllergyRepository(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        /// <summary>
        /// Gets all patient allergies.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>
        /// List of allergies of chosen patient.
        /// </returns>
        /// <exception cref="NullReferenceException">Not found any allergy of chosen patient.</exception>
        public IQueryable<Allergy> GetAllPatientAllergies(int patientId)
        {
            var patienAlletgies = _unitOfWork.PatientAllergies.GetAll(a => a.PatientId == patientId).
                                                              Where(a => !a.IsDeleted);
            if (patienAlletgies.Count() == 0)
            {
                throw new NullReferenceException("Not found any allergy of chosen patient.");
            }

            IEnumerable<Allergy> result = _unitOfWork.Allergies.GetAll();
            return patienAlletgies.Join(result,
                                   a => a.AllergyId,
                                   s => s.AllergyId,
                                   (a, s) => new Allergy { AllergyId = s.AllergyId, Pathogen = s.Pathogen, IsDeleted = s.IsDeleted }).
                                   OrderBy(s => s.Pathogen);
        }

        /// <summary>
        /// Gets the patient-allergy pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Patient-allergy pair.
        /// </returns>
        /// <exception cref="NullReferenceException">Patient-allergy pair doesn`t exist.</exception>
        public PatientAllergy GetPatientAllergy(int id)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Patient-allergy pair doesn`t exist.");
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
        /// <exception cref="NullReferenceException">
        /// Not found such patient.
        /// or
        /// Not found such allergy.</exception>
        /// <exception cref="ArgumentException">Duplicate patient-allergy pair.</exception>
        public async Task<PatientAllergy> CreatePatientAllergyAsync(PatientAllergy patientAllergy)
        {
            if (_unitOfWork.PatientsInfo.Get(patientAllergy.PatientId) == null)
            {
                throw new NullReferenceException("Not found such patient.");
            }

            if (_unitOfWork.Allergies.Get(patientAllergy.AllergyId) == null)
            {
                throw new NullReferenceException("Not found such allergy.");
            }

            if (_unitOfWork.PatientAllergies.GetAll().Any(a =>
                                                         a.AllergyId == patientAllergy.AllergyId &&
                                                         a.PatientId == patientAllergy.PatientId))
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
        /// <exception cref="NullReferenceException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> UpdatePatientAllergyAsync(int id, PatientAllergy patientAllergy)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Patient-allergy pair doesn`t exist.");
            }

            result.Map(patientAllergy);
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
        /// <exception cref="NullReferenceException">Patient-allergy pair doesn`t exist.</exception>
        public async Task<PatientAllergy> UpdateNotesAsync(int id, string notes)
        {
            var result = _unitOfWork.PatientAllergies.Get(id);
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Patient-allergy pair doesn`t exist.");
            }

            result.CloneNotes(notes);
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
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Patient-allergy pair doesn`t exist.");
            }

            _unitOfWork.CascadeDeletePatientAllergy(id);
            await _unitOfWork.Save();
        }
    }
}
