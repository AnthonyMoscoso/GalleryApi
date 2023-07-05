using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : class, new()
    {
        #region Get
        /// <summary>
        /// Get an entity by identifier.
        /// </summary>
        /// <param name="key">The key of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<TEntity?> Get(TKey key);

        /// <summary>
        /// Get an entity by filter.
        /// </summary>
        /// <param name="expression">The lambda expression used to filter the entities.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<TEntity?> Get(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        Task<IQueryable<TEntity>> GetAll();

        /// <summary>
        /// Get all entities filtered by lambda expression.
        /// </summary>
        /// <param name="expression">The lambda expression used to filter the entities.</param>
        /// <returns>Collection of entities filtered by lambda expression.</returns>
        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get a collection of entities ordered by property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property on TEntity.</typeparam>
        /// <param name="orderExpression">The lambda expression used to order the entities.</param>
        /// <param name="isAsc">Whether the list is in ascending order. Default is false.</param>
        /// <returns>Collection of entities ordered by property expression.</returns>
        Task<IQueryable<TEntity>> GetOrder<TProperty>(Expression<Func<TEntity, TProperty>> orderExpression, bool isAsc = false);

        /// <summary>
        /// Get a collection of entities paginated.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property on TEntity.</typeparam>
        /// <param name="orderExpression">The lambda expression used to order the entities.</param>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="take">The number of elements to take.</param>
        /// <param name="isAsc">Whether the list is in ascending order. Default is false.</param>
        /// <returns>Collection of entities paginated.</returns>
        Task<IQueryable<TEntity>> GetPag<TProperty>(Expression<Func<TEntity, TProperty>> orderExpression, int skip, int take, bool isAsc = false);

        /// <summary>
        /// Get a collection of entities paginated and filtered.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property on TEntity.</typeparam>
        /// <param name="expression">The lambda expression used to filter the entities.</param>
        /// <param name="orderExpression">The lambda expression used to order the entities.</param>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="take">The number of elements to take.</param>
        /// <param name="isAsc">Whether the list is in ascending order. Default is false.</param>
        /// <returns>Collection of entities paginated and filtered.</returns>
        Task<IQueryable<TEntity>> GetPag<TProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TProperty>> orderExpression, int skip, int take, bool isAsc = false);
        #endregion
        #region Insert
        /// <summary>
        /// Insert entity on dbs
        /// </summary>
        /// <param name="entity">Entity to save</param>
        /// <returns>Entity after being inserted</returns>
        Task<TEntity> Insert(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> entities);
        #endregion
        #region Update
        /// <summary>
        /// Update entity on dbs
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="key">Key to find entity</param>
        /// <returns>Entity after being updated</returns>
        Task<TEntity?> Update(TEntity entity, TKey key);
        #endregion
        #region Delete
        /// <summary>
        /// Remove entity from dbs
        /// </summary>
        /// <param name="key">Key of entity to remove</param>
        /// <returns>Success if entity was deleted</returns>
        Task Delete(TKey key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task DeleteRange(ICollection<TEntity> list);
        #endregion
        #region Include
        /// <summary>
        /// Specify related entities to include in the query results using a lambda expression
        /// </summary>
        /// <param name="includeExpressions">Lambda expressions that identify the related entities to include</param>
        /// <returns>The query with the related entities included</returns>
        DbSet<TEntity> Include(params Expression<Func<TEntity, object>>[] includeExpressions);
        #endregion
    }
}
