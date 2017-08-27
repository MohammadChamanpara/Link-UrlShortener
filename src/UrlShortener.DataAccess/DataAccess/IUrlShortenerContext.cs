using System;
using System.Data.Entity;
using UrlShortener.Core.Models;

namespace UrlShortener.DataAccess
{
	/// <summary>
	/// Db context interface for the Url Shortener domain
	/// with the aim of removing dependency to an actual implementation of dbcontext
	/// and also providing the Code First migration capabilities.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IUrlShortenerContext : IDisposable
	{
		/// <summary>
		/// Gets or sets the links dbset providing all the modification and retrieval operations on the set.
		/// </summary>
		/// <value>
		/// The links set.
		/// </value>
		DbSet<Link> Links { get; set; }

		/// <summary>
		/// Saves the changes to all the entities sets of the context.
		/// </summary>
		/// <returns>Number of rows affected</returns>
		int SaveChanges();

		/// <summary>
		/// A generic method to get a dbset based on the model type.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <returns>A dbset with the specified model type"/></returns>
		DbSet<TEntity> Set<TEntity>() where TEntity : class;

		/// <summary>
		/// Marks an entity as modified.
		/// In order not to expose details of EntityFramework operation to the callers
		/// and also acquiring the ability to pass this interfce to the repositories.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entityToUpdate">The entity to update.</param>
		void MarkAsModified<TEntity>(TEntity entityToUpdate) where TEntity : class;
	}
}