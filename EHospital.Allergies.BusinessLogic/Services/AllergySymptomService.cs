using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Allergies.BusinesLogic.Services
{
    public class AllergySymptomService : IAllergySymptomService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergySymptomService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AllergySymptomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all allergy symptoms.
        /// </summary>
        /// <param name="patientAllergyId">The patient-allergy identifier.</param>
        /// <returns>
        /// Enumeration of allergy symptoms.
        /// </returns>
        /// <exception cref="ArgumentException">Not found any symptom.</exception>
        public IQueryable<Symptom> GetAllAllergySymptoms(int patientAllergyId)
        {
            var allergySymptoms = _unitOfWork.AllergySymptoms.GetAll(a => a.PatientAllergyId == patientAllergyId);
            if (allergySymptoms.Count() == 0)
            {
                throw new ArgumentException("Not found any symptom.");
            }

            IEnumerable<Symptom> result = _unitOfWork.Symptoms.GetAll();
            return allergySymptoms.Join(result,
                                   a => a.SymptomId,
                                   s => s.Id,
                                   (a, s) => new Symptom { Id = s.Id, Naming = s.Naming, IsDeleted = s.IsDeleted }).
                                   OrderBy(s => s.Naming);
        }

        /// <summary>
        /// Gets the allergy-symptom pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Allergy-symptom pair.
        /// </returns>
        /// <exception cref="ArgumentNullException">Symptom doesn`t exist.</exception>
        public AllergySymptom GetAllergySymptom(int id)
        {
            var result = _unitOfWork.AllergySymptoms.Include(a => a.Symptom)
                                                    .FirstOrDefault(a => a.Id == id);
            if (result == null)
            {
                throw new ArgumentNullException("Allergy-symptom pair doesn`t exist.", new ArgumentException(""));
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
        /// <exception cref="ArgumentNullException">
        /// Not found such allergy of patient.
        /// or
        /// Not found such symptom.
        /// </exception>
        /// <exception cref="ArgumentException">Duplicate allergy-symptom pair.</exception>
        /// <exception cref="NullReferenceException">
        /// Not found such allergy of patient.
        /// or
        /// Not found such symptom.
        /// </exception>
        public async Task<AllergySymptom> CreateAllergySymptomAsync(AllergySymptom allergySymptom)
        {
            PatientAllergy patientAllergy = _unitOfWork.PatientAllergies.Get(allergySymptom.PatientAllergyId);
            Symptom symptom = _unitOfWork.Symptoms.Get(allergySymptom.SymptomId);
            if (patientAllergy == null)
            {
                throw new ArgumentNullException("Not found such allergy of patient.", new ArgumentException(""));
            }

            if (symptom == null)
            {
                throw new ArgumentNullException("Not found such symptom.", new ArgumentException(""));
            }

            if (_unitOfWork.AllergySymptoms.GetAll().Any(a => a.PatientAllergyId == allergySymptom.PatientAllergyId
                                                         && a.SymptomId == allergySymptom.SymptomId))
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
        /// <exception cref="ArgumentNullException">No sympthom of allergy found.</exception>
        public async Task<AllergySymptom> DeleteAllergySymptomAsync(int id)
        {
            var result = _unitOfWork.AllergySymptoms.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("No sympthom of allergy of patient found.", new ArgumentException(""));
            }

            result.IsDeleted = true;
            _unitOfWork.AllergySymptoms.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
