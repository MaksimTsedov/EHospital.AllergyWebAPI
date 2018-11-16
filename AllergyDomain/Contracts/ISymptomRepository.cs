using EHospital.Allergies.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Allergies.Domain.Contracts
{
    public interface ISymptomRepository
    {
        /// <summary>
        /// Gets all symptoms.
        /// </summary>
        /// <returns>Enumeration of symptoms.</returns>
        IQueryable<Symptom> GetAllSymptoms();

        /// <summary>
        /// Searches the symptoms by name beginning.
        /// </summary>
        /// <returns>Enumeration of symptoms with start substring.</returns>
        IQueryable<Symptom> SearchSymptomsByName(string beginning);

        /// <summary>
        /// Gets the symptom.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Symptom.</returns>
        Symptom GetSymptom(int id);

        /// <summary>
        /// Creates the symptom asynchronous and saves it into db.
        /// </summary>
        /// <param name="symptom">The symptom.</param>
        /// <returns>Symptom.</returns>
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
