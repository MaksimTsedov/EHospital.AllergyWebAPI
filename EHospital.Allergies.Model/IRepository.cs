using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EHospital.Allergies.Model
{
    /// <summary>
    /// Abstract CRUD operations for entity.
    /// </summary>
    /// <typeparam name="T">Entity.</typeparam>
    public interface IRepository<T> where T : IBaseEntity
    {
        /// <summary>
        /// Gets all entity collection.
        /// </summary>
        /// <returns>Entity collection.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all entities by the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to entity sample.</param>
        /// <returns>Entity collection.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Entity.</returns>
        Task<T> Get(int id);

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>Created entity.</returns>
        T Insert(T entity);

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>Updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>Deleted entity.</returns>
        T Delete(T entity);

        /// <summary>
        /// Specifies related entities to include in the query results. 
        /// The navigation property to be included is specified starting with the type of entity being queried. 
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="navigationPropertyPath">A lambda expression representing the navigation property to be included.</param>
        /// <returns>A new query with the related data included.</returns>
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
    }
}
