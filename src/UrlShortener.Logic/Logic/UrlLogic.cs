using System;
using UrlShortener.DataAccess;
using UrlShortener.Core.Models;

namespace UrlShortener.Logic
{
	/// <summary>
	/// An implemantation of Url Shortener domain logic which performs based on storing the data in database and 
	/// using the auto generated id for the calculation of the shorturl.
	/// </summary>
	/// <seealso cref="UrlShortener.Logic.IUrlLogic" />
	public class UrlLogic : IUrlLogic
	{
		private IUnitOfWork unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="UrlLogic"/> class.
		/// </summary>
		/// <param name="unitOfWork">The unit of work to access the data.</param>
		public UrlLogic(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Shortens the LongUrl in the specified link parameter and initializes the ShortUrl of the same object.
		/// </summary>
		/// <param name="link">The link objbect to be modified with the calculated of ShortUrl.</param>
		public virtual void Shorten(Link link)
		{
			if (link?.LongUrl == null)
				throw new ArgumentException("Long url is not provided to be shortened.");

			link.CreatedDate = DateTime.Now;

			unitOfWork.LinkRepository.Insert(link);
			unitOfWork.Save();

			if (link.Id == 0)
				throw new Exception("Unable to add record to the database.");

			link.ShortUrl = this.IdToShortUrl(link.Id);
		}

		/// <summary>
		/// Expands the specified short URL into a new <see cref="Link"/> object.
		/// </summary>
		/// <param name="shortUrl">The short URL to be expanded.</param>
		/// <returns>A link object having all the url information including the calculated LongUrl</returns>
		public virtual Link Expand(string shortUrl)
		{
			if (string.IsNullOrEmpty(shortUrl))
				throw new ArgumentException("Short url must have a value to be expanded.");

			int id = ShortUrltoId(shortUrl);

			var exsitinglink = unitOfWork.LinkRepository.GetByID(id);

			if (exsitinglink?.LongUrl == null)
				throw new ArgumentException(string.Format("Long Url not found for short Url:{0}.", shortUrl));

			exsitinglink.Clicks++;
			unitOfWork.LinkRepository.Update(exsitinglink);
			unitOfWork.Save();

			exsitinglink.LongUrl = new UriBuilder(exsitinglink.LongUrl.Trim()).Uri.AbsoluteUri;

			return exsitinglink;
		}

		/// <summary>
		/// Converts the auto generated id of database record to a Base64 string 
		/// which is going to be used as shortUrl.
		/// </summary>
		/// <param name="id">The id retrieved from database.</param>
		/// <returns>Base64 string Short url calculated from Id</returns>
		protected virtual string IdToShortUrl(int id)
		{
			var byteArray = BitConverter.GetBytes(id);
			string shortUrl = Convert.ToBase64String(byteArray);
			shortUrl = shortUrl.Replace('/', '_');
			shortUrl = shortUrl.Replace('+', '-');
			shortUrl = shortUrl.TrimEnd('=');
			return shortUrl;
		}

		/// <summary>
		/// Converts a Base64 string which is shortUrl to a database Id in order to find the record in db.
		/// </summary>
		/// <param name="shortUrl">The Base64 string short URL.</param>
		/// <returns>A number that has converted back from Base64 string</returns>
		/// <exception cref="ArgumentException">short url is not a valid Base64 string</exception>
		protected virtual int ShortUrltoId(string shortUrl)
		{
			if (shortUrl.Length % 4 > 2)
				throw new ArgumentException("short url is not a valid Base64 string");

			shortUrl = shortUrl.Replace('_', '/');
			shortUrl = shortUrl.Replace('-', '+');
			shortUrl += new string('=', shortUrl.Length % 4);
			var byteArray = Convert.FromBase64String(shortUrl);
			var id = BitConverter.ToInt32(byteArray, 0);
			return id;
		}
	}
}
