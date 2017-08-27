using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using UrlShortener.Logic;
using UrlShortener.Models;

namespace UrlShortener.Controllers.Tests
{
	[TestClass()]
	public class UrlsControllerTests
	{
		[TestMethod()]
		public void Shorten_WithValidModel_ShouldReturnOkLink()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var controller = new UrlsController(mockUrlLogic.Object);
			var link = new Link();
			//Act
			IHttpActionResult actionResult = controller.Shorten(link);

			//Assert
			actionResult.Should().BeOfType<OkNegotiatedContentResult<Link>>(because: "model is valid");
		}


		[TestMethod()]
		public void Shorten_WithValidModel_ShouldCallShortenLogic()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var controller = new UrlsController(mockUrlLogic.Object);
			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			IHttpActionResult actionResult = controller.Shorten(link);

			//Assert
			mockUrlLogic.Verify(x => x.Shorten(It.IsAny<Link>()), "valid model should reach logic");
		}

		[TestMethod()]
		public void Shorten_WithInvalidModel_ShouldNotCallShortenLogic()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.ModelState.AddModelError("", "");

			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			IHttpActionResult actionResult = controller.Shorten(link);

			//Assert
			mockUrlLogic.Verify(x => x.Shorten(It.IsAny<Link>()),Times.Never, "Invalid model should not reach logic");
		}

		[TestMethod()]
		public void Shorten_WithInvalidModel_ShouldReturnInvalidModelStateResult()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.ModelState.AddModelError("", "");
			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			IHttpActionResult actionResult = controller.Shorten(link);

			//Assert
			actionResult.Should().BeOfType<InvalidModelStateResult>(because: "model is not valid");
		}
		[TestMethod()]
		public void Expand_Always_ShouldCallExpandLogic()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			mockUrlLogic.Verify(x => x.Expand(It.IsAny<string>()), "expand should reach logic");
		}

		[TestMethod()]
		public void Expand_WithNoResultFromLogic_ShouldReturnHttpResponseMessage()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns((Link)null);
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			actionResult.Should().BeOfType<HttpResponseMessage>();
		}
		[TestMethod()]
		public void Expand_WithNoResultFromLogic_ShouldReturnNotFound()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns((Link)null);
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			(actionResult as HttpResponseMessage).StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
		}

		[TestMethod()]
		public void Expand_WithResultFromLogic_ShouldReturnHttpResponseMessage()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns(new Link() { LongUrl = "anyNonEmptyResult" });
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			actionResult.Should().BeOfType<HttpResponseMessage>();
		}
		[TestMethod()]
		public void Expand_WithInvalidResultFromLogic_ShouldReturnBadRequest()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns(new Link() { LongUrl = "http://invalid:url" });
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			(actionResult as HttpResponseMessage).StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
		}

		[TestMethod()]
		public void Expand_WithValidResultFromLogic_ShouldRedirect()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns(new Link() { LongUrl = "Valid.Url" });
			var controller = new UrlsController(mockUrlLogic.Object);
			controller.Request = new HttpRequestMessage();

			//Act
			var actionResult = controller.Expand("shortUrl");

			//Assert
			(actionResult as HttpResponseMessage).StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Moved);
		}
	}
}