using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UrlShortener.DataAccess
{
	/// <summary>
	/// The generic interface to implement repository pattern for all entities 
	/// in the application. As constructor input the DbContext is injected in order to 
	/// remove dependency to implementation of the DbContext
	/// </summary>
	/// <typeparam name="TEntity">The type of the domain model/entity.</typeparam>
	public interface IRepository<TEntity> where TEntity : class
	{
		/// <summary>
		/// Queries data from the underlying database context with specified filtering and ordering parameters.
		/// </summary>
		/// <param name="filter">The expression to filter the resuts.</param>
		/// <param name="orderBy">A <see cref="Func{T, TResult}"/> to specify the ordering of resuts.</param>
		/// <param name="includeProperties">The properties to be included in results.</param>
		/// <returns></returns>
		IEnumerable<TEntity> Get
		(
		  Expression<Func<TEntity, bool>> filter = null,
		  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
		  string includeProperties = ""
		);
		/// <summary>
		/// Gets the entity by id.
		/// </summary>
		/// <param name="id">The id of entity.</param>
		/// <returns></returns>
		TEntity GetByID(object id);

		/// <summary>
		/// Inserts the specified entity to the underlying context.
		/// </summary>
		/// <param name="entity">The entity to be inserted.</param>
		void Insert(TEntity entity);

		/// <summary>
		/// Deletes an entity the by id from the underlying context.
		/// </summary>
		/// <param name="id">The id of entity to be deleted.</param>
		void DeleteById(object id);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity object to be deleted.</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Updates the specified entity to update in the underlying context.
		/// </summary>
		/// <param name="entityToUpdate">The entity to update.</param>
		void Update(TEntity entityToUpdate);
	}
}

