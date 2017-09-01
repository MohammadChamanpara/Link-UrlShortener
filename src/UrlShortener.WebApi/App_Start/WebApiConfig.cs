using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace UrlShortener
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
           name: "swagger_root",
           routeTemplate: "",
           defaults: null,
           constraints: null,
           handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));
    }
  }
}
