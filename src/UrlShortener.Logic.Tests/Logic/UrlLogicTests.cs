using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlShortener.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using UrlShortener.DataAccess;
using UrlShortener.Core.Models;

namespace UrlShortener.Logic.Tests
{
	[TestClass]
	public class UrlLogicTests
	{
		#region Shorten
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Shorten_WithNullLink_ShouldThrowArgumentException()
		{
			//Arrange 
			var mockUnitOfWork = Mock.Of<IUnitOfWork>();
			var logic = new UrlLogic(mockUnitOfWork);

			//Act
			logic.Shorten(null);

			//Assert
			// ExpectedException Attribute
		}
		[TestMethod]
		public void Shorten_WithLink_ShouldCalculateShortUrl()
		{
			//Arrange 
			var mockLinkRepository = Mock.Of<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository);

			var logic = new UrlLogic(mockUnitOfWork.Object);
			var link = new Link()
			{
				LongUrl = "some url",
				Id = 1
			};

			//Act
			logic.Shorten(link);

			//Assert
			link.ShortUrl.Should().NotBeNullOrEmpty("Shorten should calculate the ShortUrl Property");
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Shorten_WithZeroId_ShouldThrowException()
		{
			//Arrange 
			var mockLinkRepository = Mock.Of<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository);

			var logic = new UrlLogic(mockUnitOfWork.Object);
			var link = new Link()
			{
				LongUrl = "some url",
				Id = 0
			};

			//Act
			logic.Shorten(link);

			//Assert
			// ExpectedException Attribute

		}

		[TestMethod]
		public void Shorten_WithLink_ShouldSetCreatedDate()
		{
			//Arrange 
			var mockLinkRepository = Mock.Of<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository);

			var logic = new UrlLogic(mockUnitOfWork.Object);
			var link = new Link()
			{
				LongUrl = "some url",
				CreatedDate = DateTime.Now.AddDays(-1),
				Id = 1
			};

			//Act
			logic.Shorten(link);

			//Assert
			link.CreatedDate.Should().BeCloseTo(DateTime.Now, precision: 500, because: "Shorten should calculate the ShortUrl Property");
		}

		[TestMethod]
		public void Shorten_WithLink_ShouldCallRepositoryInsert()
		{
			//Arrange 
			var mockLinkRepository = new Mock<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository.Object);

			var logic = new UrlLogic(mockUnitOfWork.Object);
			var link = new Link()
			{
				LongUrl = "some url",
				CreatedDate = DateTime.Now.AddDays(-1),
				Id = 1
			};

			//Act
			logic.Shorten(link);

			//Assert
			mockLinkRepository.Verify(x => x.Insert(link), "Link Repository should call insert when shortening a url");
		}
		#endregion

		#region Expand
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Expand_WithNullUrl_ShouldThrowArgumentException()
		{
			//Arrange 
			var mockUnitOfWork = Mock.Of<IUnitOfWork>();
			var logic = new UrlLogic(mockUnitOfWork);

			//Act
			logic.Expand(null);

			//Assert
			// ExpectedException Attribute
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Expand_WithMissingUrl_ShouldThrowArgumentException()
		{
			//Arrange 
			var mockLinkRepository = new Mock<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository.Object);
			mockLinkRepository.Setup(x => x.GetByID(0)).Returns((Link)null);
			var logic = new UrlLogic(mockUnitOfWork.Object);

			//Act
			logic.Expand("AAAAAA");

			//Assert
			// ExpectedException Attribute

		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void Expand_WithInvalidShortUrl_ShouldThrowFormatException()
		{
			//Arrange 
			var mockLinkRepository = new Mock<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository.Object);
			mockLinkRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns((Link)null);
			var logic = new UrlLogic(mockUnitOfWork.Object);

			//Act
			logic.Expand("InvalidValule");

			//Assert
			// ExpectedException Attribute

		}
		[TestMethod]
		public void Expand_WithValidShortUrl_ShouldIncrementClicks()
		{
			//Arrange 
			var mockLinkRepository = new Mock<IRepository<Link>>();
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			mockUnitOfWork.SetupGet(x => x.LinkRepository).Returns(mockLinkRepository.Object);
			var link = new Link()
			{
				Clicks = 0,
				LongUrl="longUrl"
			};
			mockLinkRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(link);
			var logic = new UrlLogic(mockUnitOfWork.Object);

			//Act
			logic.Expand("AAAAAA");

			//Assert
			link.Clicks.ShouldBeEquivalentTo(1, because: "Expand should increment clicks by one");
		}
		#endregion
	}
}