using System;
using UrlShortener.Core.Models;

namespace UrlShortener.DataAccess
{
	/// <summary>
	/// interface to implement Unit Of Work pattern in the application.
	/// logic layer uses this unit of work to perform operations on repositories 
	/// on remove the dependency to any context and have the ability to simply commit the 
	/// transaction after all modifications on repositories.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Gets the links repository which is responsible for all the operations on <see cref="Link"/> entity.
		/// </summary>
		/// <value>
		/// The link repository.
		/// </value>
		IRepository<Link> LinkRepository { get; }

		/// <summary>
		/// Saves the underlying context of the unit of work which leads to
		/// all the repository changes to be saved.
		/// </summary>
		void Save();
	}
}
