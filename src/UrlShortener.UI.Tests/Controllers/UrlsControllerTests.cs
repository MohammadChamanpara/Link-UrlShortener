using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using UrlShortener.Logic;
using UrlShortener.Models;
using UrlShortener.UI.Controllers;
using UrlShortener.UI.Tests.Helpers;

namespace UrlShortener.UI.Tests
{
	[TestClass()]
	public class UrlsControllerTests
	{
		[TestMethod()]
		public void ShortenGet_Always_ShouldReturnView()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);

			//Act
			ActionResult actionResult = urlsController.Shorten();

			//Assert
			actionResult.Should().BeOfType<ViewResult>(because: "shorten get action should return view");
		}
		[TestMethod()]
		public void ShortenGet_Always_ShouldReturnShortenView()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);

			//Act
			ActionResult actionResult = urlsController.Shorten();

			//Assert
			(actionResult as ViewResult)?.ViewName.ShouldBeEquivalentTo("shorten");
		}
		[TestMethod()]
		public void ShortenPost_WithValidModel_ShouldCallShortenLogic()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);
			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			ActionResult actionResult = urlsController.Shorten(link);

			//Assert
			mockUrlLogic.Verify(x => x.Shorten(It.IsAny<Link>()), "valid model should reach logic");
		}

		[TestMethod()]
		public void ShortenPost_WithInalidModel_ShouldNotCallShortenLogic()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);
			urlsController.ModelState.AddModelError("", "");
			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			ActionResult actionResult = urlsController.Shorten(link);

			//Assert
			mockUrlLogic.Verify(x => x.Shorten(It.IsAny<Link>()), Times.Never, "invalid model should not reach logic");
		}

		[TestMethod()]
		public void ShortenPost_WithInalidModel_ShouldReturnModel()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);
			urlsController.ModelState.AddModelError("", "");
			var link = new Link() { LongUrl = "nonEmpty" };

			//Act
			ActionResult actionResult = urlsController.Shorten(link);

			//Assert
			(actionResult as ViewResult).Model.Should().BeOfType<Link>(because: "action returns model in any case");
		}
		[TestMethod()]
		public void ShortenPost_Always_ShouldReturnModel()
		{
			//Arrange
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);
			var link = new Link() { LongUrl = "" };

			//Act
			ActionResult actionResult = urlsController.Shorten(link);

			//Assert
			(actionResult as ViewResult).Model.Should().BeOfType<Link>(because: "action returns model in any case");
		}
		[TestMethod()]
		public void Expand_Always_ShouldCallExpandLogic()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			var urlsController = new UrlsController(mockUrlLogic.Object);

			//Act
			ActionResult actionResult = urlsController.Expand("shortUrl");

			//Assert
			mockUrlLogic.Verify(x => x.Expand(It.IsAny<string>()), "expand should reach logic");
		}

		[TestMethod()]
		public void Expand_WithNoResultFromLogic_ShouldReturnNotFound()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns((Link)null);
			var urlsController = new UrlsController(mockUrlLogic.Object);

			//Act
			ActionResult actionResult = urlsController.Expand("shortUrl");

			//Assert
			actionResult.Should().BeOfType<HttpNotFoundResult>();
		}
		[TestMethod()]
		public void Expand_WithResultFromLogic_ShouldRedirect()
		{
			var mockUrlLogic = new Mock<IUrlLogic>();
			mockUrlLogic.Setup(x => x.Expand(It.IsAny<string>())).Returns(new Link() { LongUrl = "nonEmpty" });
			var urlsController = new UrlsController(mockUrlLogic.Object);
			urlsController.ControllerContext = TestHelpers.CreateControllerContext(urlsController);

			//Act
			ActionResult actionResult = urlsController.Expand("shortUrl");

			//Assert
			actionResult.Should().BeOfType<RedirectResult>();
		}
	}
}