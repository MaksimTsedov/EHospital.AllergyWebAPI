using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EHospital.AllergyDA.Contracts
{
    /// <summary>
    /// Abstract CRUD operations for entity.
    /// </summary>
    /// <typeparam name="T">Entity.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entity collection.
        /// </summary>
        /// <returns>Entity collection.</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Gets all entities by the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to entity sample.</param>
        /// <returns>Entity collection.</returns>
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Entity.</returns>
        Task<T> GetEntity(int id);

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>Created entity.</returns>
        T InsertEntity(T entity);

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>Updated entity.</returns>
        T UpdateEntity(T entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>Deleted entity.</returns>
        T DeleteEntity(T entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Deleted entity.</returns>
        T DeleteEntity(int Id);
    }
}
