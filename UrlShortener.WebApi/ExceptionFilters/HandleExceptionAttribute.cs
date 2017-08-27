using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace UrlShortener.WebApi.ExceptionFilters
{
	/// <summary>
	/// Handles the exceptions thrown by Logic or Data Layers and casts to 
	/// a web meaningful target Response message 
	/// </summary>
	/// <seealso cref="System.Web.Http.Filters.ExceptionFilterAttribute" />
	public class HandleExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
			if (context.Exception is ArgumentException)
			{
				context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
				{
					Content = new StringContent(context.Exception.Message)
				};
			}
		}
	}
}