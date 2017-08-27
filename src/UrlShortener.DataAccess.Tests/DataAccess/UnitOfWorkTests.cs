using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using UrlShortener.Core.Models;

namespace UrlShortener.DataAccess.Tests
{
	[TestClass()]
	public class UnitOfWorkTests
	{
		[TestMethod()]
		public void Save_Always_ShouldSaveDbContext()
		{
			//Arrange
			var mockDbContext = new Mock<IUrlShortenerContext>();
			var mocklinkRepository = new Mock<IRepository<Link>>();
			var unitOfWork = new UnitOfWork(mockDbContext.Object, mocklinkRepository.Object);
			
			//Act
			unitOfWork.Save();

			//Assert
			mockDbContext.Verify(x => x.SaveChanges(),failMessage: "Save in UnitOfWork should call DbContext save.");
		}

		[TestMethod()]
		public void Dispose_Always_ShouldDisposeDbContext()
		{
			//Arrange
			var mockDbContext = new Mock<IUrlShortenerContext>();
			var mocklinkRepository = new Mock<IRepository<Link>>();

			var unitOfWork = new UnitOfWork(mockDbContext.Object, mocklinkRepository.Object);

			//Act
			unitOfWork.Dispose();

			//Assert
			mockDbContext.Verify(x => x.Dispose(), failMessage: "Dispose in UnitOfWork should call DbContext Dispose.");
		}
	}
}