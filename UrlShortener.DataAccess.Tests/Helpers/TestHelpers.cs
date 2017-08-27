using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace UrlShortener.DataAccess.Tests.Helpers
{
	public static class TestHelpers
	{
		public static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(IList<TEntity> data, Func<object[], TEntity> find = null)
			where TEntity : class, new()
		{
			var source = data.AsQueryable();
			var mock = new Mock<DbSet<TEntity>> { CallBase = true };
			mock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(source.Expression);
			mock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(source.ElementType);
			mock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
			mock.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(source.Provider);
			mock.As<IDbSet<TEntity>>().Setup(m => m.Create()).Returns(new TEntity());
			mock.As<IDbSet<TEntity>>().Setup(m => m.Add(It.IsAny<TEntity>())).Returns<TEntity>(i => { data.Add(i); return i; });
			mock.As<IDbSet<TEntity>>().Setup(m => m.Remove(It.IsAny<TEntity>())).Returns<TEntity>(i => { data.Remove(i); return i; });
			if (find != null) mock.As<IDbSet<TEntity>>().Setup(m => m.Find(It.IsAny<object[]>())).Returns(find);
			return mock;
		}
	}
}
