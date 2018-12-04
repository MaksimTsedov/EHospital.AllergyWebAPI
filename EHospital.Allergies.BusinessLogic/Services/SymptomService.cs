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
    /// Symptom service business logic
    /// </summary>
    /// <seealso cref="EHospital.Allergies.BusinesLogic.Contracts.ISymptomService" />
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
        public async Task<IEnumerable<Symptom>> GetAllSymptoms()
        {
            return await Task.FromResult(_unitOfWork.Symptoms.GetAll().OrderBy(s => s.Naming));
        }

        /// <summary>
        /// Searches the symptoms by search key.
        /// </summary>
        /// <param name="searchKey">The start of symptom naming.</param>
        /// <returns>
        /// Enumeration of symptoms with start substring.
        /// </returns>
        public async Task<IEnumerable<Symptom>> SearchSymptomsByName(string searchKey)
        {
            var result = await Task.FromResult(_unitOfWork.Symptoms.GetAll(s => s.Naming.StartsWith(searchKey))
                                                      .OrderBy(s => s.Naming));
            return result;
        }

        /// <summary>
        /// Gets the symptom.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Symptom.
        /// </returns>
        /// <exception cref="ArgumentNullException">Symptom doesn`t exist.</exception>
        public async Task<Symptom> GetSymptom(int id)
        {
            var result = await _unitOfWork.Symptoms.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("Symptom doesn`t exist.");
            }

            return result;
        }

        /// <summary>
        /// Creates the symptom asynchronous and saves it into db.
        /// </summary>
        /// <param name="symptom">The symptom.</param>
        /// <returns>
        /// Created symptom.
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
        /// <exception cref="ArgumentNullException">No symptom found.</exception>
        /// <exception cref="InvalidOperationException">There are exist records with involvement of this symptom.</exception>
        public async Task<Symptom> DeleteSymptomAsync(int id)
        {
            var result = await _unitOfWork.Symptoms.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException("No symptom found.");
            }

            if (_unitOfWork.AllergySymptoms.GetAll().Any(s => s.SymptomId == result.Id))
            {
                throw new InvalidOperationException("There are exist records with involvement of this symptom.");
            }

            result.IsDeleted = true;
            _unitOfWork.Symptoms.Delete(result);
            await _unitOfWork.Save();
            return result;
        }
    }
}
