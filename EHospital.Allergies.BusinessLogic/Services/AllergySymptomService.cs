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
    /// Service on symptoms of patient allergy
    /// </summary>
    /// <seealso cref="EHospital.Allergies.BusinesLogic.Contracts.IAllergySymptomService" />
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
        /// Gets all symptoms of patient allergy.
        /// </summary>
        /// <param name="patientAllergyId">The patient allergy identifier.</param>
        /// <returns>
        /// Enumeration of allergy symptoms.
        /// </returns>
        /// <exception cref="ArgumentException">Not found any symptom.</exception>
        public async Task<IEnumerable<Symptom>> GetAllAllergySymptoms(int patientAllergyId)
        {
            var allergySymptoms = _unitOfWork.AllergySymptoms.GetAll(a => a.PatientAllergyId == patientAllergyId);
            if (allergySymptoms.Count() == 0)
            {
                throw new ArgumentException("Not found any symptom.");
            }

            IEnumerable<Symptom> result = _unitOfWork.Symptoms.GetAll();
            return await Task.FromResult(
                                   allergySymptoms.Join(result,
                                   a => a.SymptomId,
                                   s => s.Id,
                                   (a, s) => new Symptom { Id = s.Id, Naming = s.Naming, IsDeleted = s.IsDeleted }).
                                   OrderBy(s => s.Naming));
        }

        /// <summary>
        /// Gets the allergy-symptom pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Symptom of patient allergy.
        /// </returns>
        /// <exception cref="ArgumentNullException">Allergy-symptom pair doesn`t exist.</exception>
        public async Task<AllergySymptom> GetAllergySymptom(int id)
        {
            var result = _unitOfWork.AllergySymptoms.Include(a => a.Symptom).FirstOrDefault(a => a.Id == id);
            if (result == null)
            {
                throw new ArgumentNullException("Allergy-symptom pair doesn`t exist.");
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Creates the allergy-symptom pair asynchronous and saves it into db.
        /// </summary>
        /// <param name="allergySymptom">The allergy-symptom pair.</param>
        /// <returns>
        /// Created symptom of patient allergy.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Not found such allergy of patient.
        /// or
        /// Not found such symptom.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Duplicate allergy-symptom pair.
        /// </exception>
        public async Task<AllergySymptom> CreateAllergySymptomAsync(AllergySymptom allergySymptom)
        {
            Task<PatientAllergy> patientAllergy = _unitOfWork.PatientAllergies.Get(allergySymptom.PatientAllergyId);
            Task<Symptom> symptom = _unitOfWork.Symptoms.Get(allergySymptom.SymptomId);
            Task.WaitAll(patientAllergy, symptom);

            if (patientAllergy == null)
            {
                throw new ArgumentNullException("Not found such allergy of patient.");
            }

            if (symptom == null)
            {
                throw new ArgumentNullException("Not found such symptom.");
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
        /// Deleted allergy-symptom
        /// </returns>
        /// <exception cref="ArgumentNullException">No symptom of allergy of patient found.</exception>
        public async Task<AllergySymptom> DeleteAllergySymptomAsync(int id)
        {
            var result = await Task.FromResult(_unitOfWork.AllergySymptoms.Include(a => a.Symptom)
                                                                          .FirstOrDefault(a => a.Id == id));
            if (result == null)
            {
                throw new ArgumentNullException("No symptom of allergy of patient found.");
            }

            result.IsDeleted = true;
            result.DeletionDate = DateTime.UtcNow;
            _unitOfWork.AllergySymptoms.Delete(result);
            await _unitOfWork.Save();
            return await Task.FromResult(result);
        }
    }
}
