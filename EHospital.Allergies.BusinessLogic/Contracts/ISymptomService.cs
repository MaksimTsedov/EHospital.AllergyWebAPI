using EHospital.Allergies.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHospital.Allergies.BusinesLogic.Contracts
{
    /// <summary>
    /// Service abstraction for symptoms working on
    /// </summary>
    public interface ISymptomService
    {
        /// <summary>
        /// Gets all symptoms.
        /// </summary>
        /// <returns>Enumeration of symptoms.</returns>
        Task<IEnumerable<Symptom>> GetAllSymptoms();

        /// <summary>
        /// Searches the symptoms by search key.
        /// </summary>
        /// <param name="searchKey">The start of symptom naming.</param>
        /// <returns>
        /// Enumeration of symptoms with start substring.
        /// </returns>
        Task<IEnumerable<Symptom>> SearchSymptomsByName(string searchKey);

        /// <summary>
        /// Gets the symptom.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Symptom.</returns>
        Task<Symptom> GetSymptom(int id);

        /// <summary>
        /// Creates the symptom asynchronous and saves it into db.
        /// </summary>
        /// <param name="symptom">The symptom.</param>
        /// <returns>Created symptom.</returns>
        Task<Symptom> CreateSymptomAsync(Symptom symptom);

        /// <summary>
        /// Deletes the symptom asynchronous from db.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted symptom.
        /// </returns>
        Task<Symptom> DeleteSymptomAsync(int id);
    }
}
