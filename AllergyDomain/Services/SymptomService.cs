using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.BusinesLogic.Services
{
    public class SymptomService : ISymptomService
    {
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SymptomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all symptoms.
        /// </summary>
        /// <returns>
        /// Enumeration of symptoms.
        /// </returns>
        /// <exception cref="NullReferenceException">No records in db.</exception>
        public IQueryable<Symptom> GetAllSymptoms()
        {
            return _unitOfWork.Symptoms.GetAll().OrderBy(s => s.Naming);
        }

        /// <summary>
        /// Searches the symptoms by name beginning.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns>
        /// Enumeration of symptoms with start substring.
        /// </returns>
        /// <exception cref="NullReferenceException">No symptoms exist.</exception>
        public IQueryable<Symptom> SearchSymptomsByName(string searchKey)
        {
            return _unitOfWork.Symptoms.GetAll(s => s.Naming.StartsWith(searchKey))
                                                      .OrderBy(s => s.Naming);
        }

        /// <summary>
        /// Gets the symptom.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Symptom.
        /// </returns>
        /// <exception cref="NullReferenceException">Symptom doesn`t exist.</exception>
        public Symptom GetSymptom(int id)
        {
            var result = _unitOfWork.Symptoms.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Symptom doesn`t exist.", new ArgumentException(""));
            }

            return result;
        }

        /// <summary>
        /// Creates the symptom asynchronous and saves it into db.
        /// </summary>
        /// <param name="symptom">The symptom.</param>
        /// <returns>
        /// Symptom.
        /// </returns>
        /// <exception cref="ArgumentException">Duplicate symptom.</exception>
        public async Task<Symptom> CreateSymptomAsync(Symptom symptom)
        {
            if (_unitOfWork.Symptoms.GetAll().Any(s => s.Naming == symptom.Naming))
            {
                throw new ArgumentException("Duplicate symptom.");
            }

            Symptom result = _unitOfWork.Symptoms.Insert(symptom);
            await _unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Deletes the symptom asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted symptom.
        /// </returns>
        /// <exception cref="NullReferenceException">No symptom found.</exception>
        /// <exception cref="ArgumentException">There are exist records with involvment of this symptom.</exception>
        public async Task<Symptom> DeleteSymptomAsync(int id)
        {
            var result = _unitOfWork.Symptoms.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("No symptom found.", new ArgumentException(""));
            }

            if (_unitOfWork.AllergySymptoms.GetAll().Any(s => s.SymptomId == result.Id))
            {
                throw new InvalidOperationException("There are exist records with involvment of this symptom.");
            }

            result.IsDeleted = true;
            _unitOfWork.Symptoms.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
