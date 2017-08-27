using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UrlShortener.DataAccess.Tests.Helpers;
using UrlShortener.Models;

namespace UrlShortener.DataAccess.Tests
{
	[TestClass()]
	public class RepositoryTests
	{
		[TestMethod()]
		public void Insert_WithValidEntity_ShouldAddToDbSet()
		{
			//Arrange
			var mockDbContext = new Mock<IUrlShortenerContext>();
			var mockDbSet = new Mock<DbSet<Link>>();

			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);
			var link = new Link();

			//Act
			repository.Insert(link);

			//Assert
			mockDbSet.Verify(x => x.Add(link));
		}

		[TestMethod()]
		public void Get_WithNoParameter_ShouldReturnAll()
		{
			//Arrange

			var links = new List<Link>
			{
				new Link(),
				new Link(),
				new Link(),
				new Link()
			};
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			var list = repository.Get();

			//Assert
			list.Count().ShouldBeEquivalentTo(links.Count(), because: "Get should return all the entities.");
		}

		[TestMethod()]
		public void Get_WithOrderByParameter_ShouldReturnOrderedList()
		{
			//Arrange
			var links = new List<Link>
			{
				new Link(){ Id = 2 },
				new Link(){ Id = 4 },
				new Link(){ Id = 1 },
				new Link(){ Id = 3 }
			};

			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			var list = repository.Get(orderBy: x => x.OrderBy(y => y.Id));

			//Assert
			list.First().Id.ShouldBeEquivalentTo(1, because: "Orderby parameter should return the sorted entities.");
		}
		[TestMethod()]
		public void Get_WithFilterParameter_ShouldReturnFilteredList()
		{
			//Arrange
			var links = new List<Link>
			{
				new Link(){ Id = 1 },
				new Link(){ Id = 2 },
				new Link(){ Id = 3 },
				new Link(){ Id = 4 }
			};
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			var list = repository.Get(filter: x => x.Id > 2);

			//Assert
			list.Count().ShouldBeEquivalentTo(2, because: "filter parameter should return the filtered entities.");
		}
		[TestMethod()]
		public void Get_WithOrderByAndFilter_ShouldReturnFilteredOrderedList()
		{
			//Arrange

			var links = new List<Link>
			{
				new Link(){ Id = 4 },
				new Link(){ Id = 2 },
				new Link(){ Id = 1 },
				new Link(){ Id = 3 }
			};
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			var list = repository.Get(filter: x => x.Id > 2, orderBy: x => x.OrderBy(y => y.Id));

			//Assert
			list.First().Id.ShouldBeEquivalentTo(3, because: "entities<2 are filtered and then orderby id");
		}
		[TestMethod()]
		public void GetById_WithValidId_ShouldReturnCorrectEntity()
		{
			//Arrange

			var links = new List<Link>
			{
				new Link(){ Id = 4 , LongUrl = "L4" },
				new Link(){ Id = 2 , LongUrl = "L2" },
				new Link(){ Id = 1 , LongUrl = "L1" },
				new Link(){ Id = 3 , LongUrl = "L3" }
			};
			var entity = new Link();
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links, x => links[3]);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			var link = repository.GetByID(3);

			//Assert
			link.LongUrl.ShouldBeEquivalentTo("L3", because: "3 is passed as id");
		}
		[TestMethod()]
		public void DeleteById_WithValidId_ShouldRemoveEntity()
		{
			//Arrange
			var links = new List<Link>
			{
				new Link(){ Id = 4 },
				new Link(){ Id = 2 },
				new Link(){ Id = 1 },
				new Link(){ Id = 3 }
			};
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links, x => links[1]);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			repository.DeleteById(2);

			//Assert
			links.Count().ShouldBeEquivalentTo(3, because: "one entity out of four entities is deleted");
		}
		[TestMethod()]
		public void Delete_WithEntity_ShouldRemoveEntity()
		{
			//Arrange
			var links = new List<Link>
			{
				new Link(){ Id = 4 },
				new Link(){ Id = 2 },
				new Link(){ Id = 1 },
				new Link(){ Id = 3 }
			};
			var mockDbSet = TestHelpers.CreateMockDbSet<Link>(links);
			var mockDbContext = new Mock<IUrlShortenerContext>();
			mockDbContext.Setup(x => x.Set<Link>()).Returns(mockDbSet.Object);
			var repository = new Repository<Link>(mockDbContext.Object);

			//Act
			repository.Delete(links[2]);

			//Assert
			links.Count().ShouldBeEquivalentTo(3, because: "one entity out of four entities is deleted");
		}
	}
}