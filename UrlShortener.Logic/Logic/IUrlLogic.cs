using UrlShortener.Models;

namespace UrlShortener.Logic
{
	/// <summary>
	/// The logic of Url shortening and expanding in Url Shortener domain
	/// </summary>
	public interface IUrlLogic
	{
		/// <summary>
		/// Shortens the LongUrl in the specified link parameter and initializes the ShortUrl of the same object.
		/// </summary>
		/// <param name="link">The link objbect to be modified with the calculated of ShortUrl.</param>
		void Shorten(Link link);

		/// <summary>
		/// Expands the specified short URL into a new <see cref="Link"/> object.
		/// </summary>
		/// <param name="shortUrl">The short URL to be expanded.</param>
		/// <returns>A link object having all the url information including the calculated LongUrl</returns>
		Link Expand(string shortUrl);
	}
}
