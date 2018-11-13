using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EHospital.AllergyDA.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EHospital.AllergyDA.Repository
{
    /// <summary>
    /// Repository consists CRUD operations above data of 'T' type
    /// </summary>
    /// <typeparam name="T">Entity.</typeparam>
    /// <seealso cref="AllergyDA.Contracts.IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The allergy db context
        /// </summary>
        private readonly AllergyDbContext _context;

        /// <summary>
        /// Set of entities
        /// </summary>
        private DbSet<T> _entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(AllergyDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        /// <summary>
        /// Gets all entity collection.
        /// </summary>
        /// <returns>
        /// Entity collection.
        /// </returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets all entities by the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to entity sample.</param>
        /// <returns>
        /// Entity collection.
        /// </returns>
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Entity.
        /// </returns>
        public async Task<T> GetEntity(int id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>
        /// Created entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">Tried to insert null entity!</exception>
        public T InsertEntity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Tried to insert null entity!");
            }
            _entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <returns>
        /// Updated entity.
        /// </returns>
        public T UpdateEntity(T entityToUpdate)
        {
            _entities.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        /// <returns>
        /// Deleted entity.
        /// </returns>
        public T DeleteEntity(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _entities.Attach(entityToDelete);
            }
            _entities.Remove(entityToDelete);
            return entityToDelete;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Deleted entity.</returns>
        public T DeleteEntity(int id)
        {
            T entityToDelete = _entities.Find(id);
            DeleteEntity(entityToDelete);
            return entityToDelete;
        }
    }
}
