using UrlShortener.Core.Models;

namespace UrlShortener.DataAccess
{
	/// <summary>
	/// An implementation of <see cref="IUnitOfWork"/> for the Url Shortener domain.
	/// Having the dependacies as constructor parameters
	/// </summary>
	/// <seealso cref="UrlShortener.DataAccess.IUnitOfWork" />
	public class UnitOfWork : IUnitOfWork
	{
		private IUrlShortenerContext dbContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <param name="linkRepository">The link repository.</param>
		public UnitOfWork(IUrlShortenerContext dbContext, IRepository<Link> linkRepository)
		{
			this.dbContext = dbContext;
			this.LinkRepository = linkRepository;
		}
		public virtual IRepository<Link> LinkRepository { get; protected set; }

		public void Save()
		{
			dbContext.SaveChanges();
		}
		public void Dispose()
		{
			dbContext.Dispose();
		}
	}
}
