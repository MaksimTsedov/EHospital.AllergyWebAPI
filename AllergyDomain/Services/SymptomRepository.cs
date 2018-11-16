using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Allergies.DAL.Contracts;
using EHospital.Allergies.DAL.Entities;
using EHospital.Allergies.Domain.Contracts;

namespace EHospital.Allergies.Domain.Services
{
    public class SymptomRepository : ISymptomRepository
    {
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomRepository" /> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        public SymptomRepository(IUnitOfWork uow)
        {
            _unitOfWork = uow;
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
            var result = _unitOfWork.Symptoms.GetAll().Where(a => !a.IsDeleted); ;
            if (result.Count() == 0)
            {
                throw new NullReferenceException("No records in db.");
            }

            return result.OrderBy(a => a.Naming);
        }

        /// <summary>
        /// Searches the symptoms by name beginning.
        /// </summary>
        /// <param name="beginning"></param>
        /// <returns>
        /// Enumeration of symptoms with start substring.
        /// </returns>
        /// <exception cref="NullReferenceException">No symptoms exist.</exception>
        public IQueryable<Symptom> SearchSymptomsByName(string beginning)
        {
            var result = _unitOfWork.Symptoms.GetAll(a => a.Naming.StartsWith(beginning)).
                                              Where(a => !a.IsDeleted); ;
            if (result.Count() == 0)
            {
                throw new NullReferenceException("No symptoms exist.");
            }

            return result.OrderBy(a => a.Naming);
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
            if (result == null || result.IsDeleted)
            {
                throw new NullReferenceException("Symptom doesn`t exist.");
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
                throw new NullReferenceException("No symptom found.");
            }

            if (_unitOfWork.AllergySymptoms.GetAll().Any(s => s.SymptomId == result.SymptomId))
            {
                throw new ArgumentException("There are exist records with involvment of this symptom.");
            }

            result.IsDeleted = true;
            _unitOfWork.Symptoms.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
