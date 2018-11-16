using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.DAL.Contracts;
using EHospital.Allergies.DAL.Entities;
using EHospital.Allergies.Domain.Contracts;

namespace EHospital.Allergies.Domain.Services
{
    public class AllergySymptomRepository : IAllergySymptomRepository
    {
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomRepository" /> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public AllergySymptomRepository(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        /// <summary>
        /// Gets all allergy symptoms.
        /// </summary>
        /// <param name="patientAllergyId">The patient-allergy identifier.</param>
        /// <returns>
        /// Enumeration of allergy symptoms.
        /// </returns>
        /// <exception cref="NullReferenceException">Not found any symptom.</exception>
        public IQueryable<Symptom> GetAllAllergySymptoms(int patientAllergyId)
        {
            var allergySymptoms = _unitOfWork.AllergySymptoms.GetAll(a => a.PatientAllergyId == patientAllergyId).
                                                              Where(a => !a.IsDeleted);
            if (allergySymptoms.Count() == 0)
            {
                throw new NullReferenceException("Not found any symptom.");
            }

            IEnumerable<Symptom> result = _unitOfWork.Symptoms.GetAll();
            return allergySymptoms.Join(result,
                                   a => a.SymptomId,
                                   s => s.SymptomId,
                                   (a, s) => new Symptom { SymptomId = s.SymptomId, Naming = s.Naming, IsDeleted = s.IsDeleted }).
                                   OrderBy(s => s.Naming);
        }

        /// <summary>
        /// Gets the allergy-symptom pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Allergy-symptom pair.
        /// </returns>
        /// <exception cref="NullReferenceException">Symptom doesn`t exist.</exception>
        public AllergySymptom GetAllergySymptom(int id)
        {
            var result = _unitOfWork.AllergySymptoms.Get(id);
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Allergy-symptom pair doesn`t exist.");
            }

            return result;
        }

        /// <summary>
        /// Creates the allergy-symptom pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergySymptom">The allergy-symptom pair.</param>
        /// <returns>
        /// Allergy-symptom pair.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Not found such allergy of patient.
        /// or
        /// Not found such symptom.</exception>
        /// <exception cref="ArgumentException">Duplicate allergy-symptom pair.</exception>
        public async Task<AllergySymptom> CreateAllergySymptomAsync(AllergySymptom allergySymptom)
        {
            PatientAllergy patientAllergy = _unitOfWork.PatientAllergies.Get(allergySymptom.PatientAllergyId);
            Symptom symptom = _unitOfWork.Symptoms.Get(allergySymptom.SymptomId);
            if (patientAllergy == null)
            {
                throw new NullReferenceException("Not found such allergy of patient.");
            }

            if (symptom == null)
            {
                throw new NullReferenceException("Not found such symptom.");
            }

            if (_unitOfWork.AllergySymptoms.GetAll().Any(a => 
                                                         a.AllergySymptomId == allergySymptom.AllergySymptomId &&
                                                         a.SymptomId == allergySymptom.SymptomId))
            {
                throw new ArgumentException("Duplicate allergy-symptom pair.");
            }

            AllergySymptom result = _unitOfWork.AllergySymptoms.Insert(allergySymptom);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Deletes the allergy-symptom pair asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted allergy-symptom pair
        /// </returns>
        /// <exception cref="NullReferenceException">No sympthom of allergy found.</exception>
        public async Task<AllergySymptom> DeleteAllergySymptomAsync(int id)
        {
            var result = _unitOfWork.AllergySymptoms.Get(id);
            if (result == null)
            {
                throw new NullReferenceException("No sympthom of allergy found.");
            }

            result.IsDeleted = true;
            _unitOfWork.AllergySymptoms.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
