using Moq;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlShortener.UI.Tests.Helpers
{
	public static class TestHelpers
	{
		public static ControllerContext CreateControllerContext(Controller controller)
		{
			var mockContext = new Mock<HttpContextBase>();
			var mockRequest = new Mock<HttpRequestBase>();
			var mockResponse = new Mock<HttpResponseBase>();

			var  mockCache=new Mock<HttpCachePolicyBase>();
			mockResponse.SetupGet(x => x.Cache).Returns(mockCache.Object);

			mockResponse.SetupGet(x => x.Headers).Returns(new NameValueCollection());

			mockContext.Setup(x => x.Request).Returns(mockRequest.Object);
			mockContext.Setup(x => x.Response).Returns(mockResponse.Object);
			mockRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
			mockResponse.Setup(x => x.Cookies).Returns(new HttpCookieCollection());

			var requestContext = new RequestContext(mockContext.Object, new RouteData());
			return new ControllerContext(requestContext, controller);
		}
	}
}
