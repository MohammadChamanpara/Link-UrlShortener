using System.Data.Entity;
using UrlShortener.Models;

namespace UrlShortener.DataAccess
{
	/// <summary>
	/// An implementation of <see cref="IUrlShortenerContext"/> to contain the databse
	/// context for the Url Shortener 
	/// </summary>
	/// <seealso cref="System.Data.Entity.DbContext" />
	/// <seealso cref="UrlShortener.DataAccess.IUrlShortenerContext" />
	public class UrlShortenerContext : DbContext, IUrlShortenerContext
	{
		public virtual DbSet<Link> Links { get; set; }
		public UrlShortenerContext() : base("UrlShortenerDB") { }

		public void MarkAsModified<TEntity>(TEntity entity) where TEntity : class
		{
			Entry(entity).State = EntityState.Modified;
		}
	}
}