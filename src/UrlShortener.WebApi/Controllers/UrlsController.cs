using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using UrlShortener.Logic;
using UrlShortener.Core.Models;
using UrlShortener.WebApi.ExceptionFilters;

namespace UrlShortener.Controllers
{
	/// <summary>
	/// The api controller to provide UrlShortener app with web api and 
	/// this service to be used widely and easily by any consumer client technology
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	[HandleExceptionFilter]
	public class UrlsController : ApiController
	{
		IUrlLogic urlLogic;
		/// <summary>
		/// Initializes a new instance of the <see cref="UrlsController"/> class.
		/// </summary>
		/// <param name="urlLogic">The URL logic.</param>
		public UrlsController(IUrlLogic urlLogic)
		{
			this.urlLogic = urlLogic;
		}

		/// <summary>
		/// A post method which shortens the LongUrl in the specified link object.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns>An <see cref="IHttpActionResult"/> based on the parameter and the operations</returns>
		[HttpPost]
		[ResponseType(typeof(Link))]
		[Route("api/shorten")]
		public IHttpActionResult Shorten(Link link)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			urlLogic.Shorten(link);

			return Ok(link);
		}

		/// <summary>
		/// Expands the specified short URL and redirects to the corresponding LongUrl in case of success.
		/// </summary>
		/// <param name="shortUrl">The short URL to be expanded.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{shortUrl}")]
		public HttpResponseMessage Expand(string shortUrl)
		{
			var link = urlLogic.Expand(shortUrl);

			string longUrl = link?.LongUrl;

			if (string.IsNullOrEmpty(longUrl))
			{
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The requested url could not be found.");
			}

			/* 
			 * Permanent redirect has been chosen to help SEO with better rating 
			 * bacause google when facing a 301, passes page rank to the final destination
			 * It is also useful for Googlebots operations
			 */
			var response = Request.CreateResponse(HttpStatusCode.Moved);

			try
			{
				response.Headers.Location = new Uri(longUrl,UriKind.RelativeOrAbsolute);
			}
			catch (Exception exception)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("The requested url {0} is invalid. \r\nDetails:{1}", longUrl, exception.Message));
			}
			
			/* 
			 * A no cache policy adopted to receive the requests every time 
			 * and store the analytics data such as clicks in database
			 */
			response.Headers.CacheControl = new CacheControlHeaderValue()
			{
				MustRevalidate = true,
				NoCache = true,
				NoStore = true,
				MaxAge = TimeSpan.Zero
			};
			response.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));

			return response;
		}
	}
}