using System;
using System.Web;
using System.Web.Mvc;
using UrlShortener.Logic;
using UrlShortener.Models;

namespace UrlShortener.UI.Controllers
{
	/// <summary>
	/// Mvc controller for the UrlShortener application flow
	/// </summary>
	/// <seealso cref="System.Web.Mvc.Controller" />
	public class UrlsController : Controller
	{
		IUrlLogic urlLogic;
		/// <summary>
		/// Initializes a new instance of the <see cref="UrlsController"/> class.
		/// </summary>
		/// <param name="urlLogic">The URL logic dependency.</param>
		public UrlsController(IUrlLogic urlLogic)
		{
			this.urlLogic = urlLogic;
		}

		/// <summary>
		/// Provides the Home page of application which performs the shortening operation.
		/// </summary>
		/// <returns>The shorten view</returns>
		[Route]
		[HttpGet]
		public ActionResult Shorten()
		{
			return View("shorten");
		}

		/// <summary>
		/// A post action which shortens the LongUrl in the specified link object.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns>
		/// Action result based on the operation and provide parameter
		/// </returns>
		[Route]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Shorten([Bind(Include = "LongUrl")] Link link)
		{
			if (ModelState.IsValid)
			{
				urlLogic.Shorten(link);
			}
			return View("shorten", link);
		}

		/// <summary>
		/// Expands the specified short URL and redirects to the corresponding LongUrl .
		/// </summary>
		/// <param name="shortUrl">The short URL to be expanded.</param>
		/// <returns></returns>
		[Route("{shortUrl}", Name = "Expand")]
		[HttpGet]
		public ActionResult Expand(string shortUrl)
		{
			var link = urlLogic.Expand(shortUrl);

			string longUrl = link?.LongUrl;

			if (string.IsNullOrEmpty(longUrl))
			{
				return new HttpNotFoundResult("The requested url could not be found.");
			}

			/* Permanent redirect has been chosen to help SEO with better rating 
			 * bacause google when facing a 301, passes page rank to the final destination
			 * It is also useful for Googlebots operations
			 */

			var result = RedirectPermanent(longUrl);

			/* 
			 * A no cache policy adopted to receive the requests every time 
			 * and store the analytics data such as clicks in database
			 */
			Response.Cache.SetExpires(DateTime.MinValue);
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.Cache.SetNoStore();
			Response.Cache.SetMaxAge(TimeSpan.Zero);
			Response.Headers.Add("pragma", "no-cache");

			return result;

		}
	}
}
