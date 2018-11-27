using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EHospital.Allergies.Data
{
    /// <summary>
    /// Repository consists CRUD operations above data of 'T' type
    /// </summary>
    /// <typeparam name="T">Entity.</typeparam>
    /// <seealso cref="Allergies.Data.Contracts.IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
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
        public IQueryable<T> GetAll()
        {
            return _entities.AsNoTracking();
        }

        /// <summary>
        /// Gets all entities by the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to entity sample.</param>
        /// <returns>
        /// Entity collection.
        /// </returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate).AsNoTracking();
        }

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Entity.
        /// </returns>
        public async Task<T> Get(int id)
        {
            return await _entities.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>
        /// Created entity.
        /// </returns>
        /// <exception cref="ArgumentException">Tried to insert null entity!</exception>
        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Tried to insert null entity!");
            }
            _entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// Updated entity.
        /// </returns>
        public T Update(T entity)
        {  
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// Deleted entity.
        /// </returns>
        public T Delete(T entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Specifies related entities to include in the query results.
        /// The navigation property to be included is specified starting with the type of entity being queried.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="navigationPropertyPath">A lambda expression representing the navigation property to be included.</param>
        /// <returns>
        /// A new query with the related data included.
        /// </returns>
        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return _entities.Include(navigationPropertyPath);
        }
    }
}
