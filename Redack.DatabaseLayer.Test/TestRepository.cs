using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.DatabaseLayer.Test.Data;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using Redack.DatabaseLayer.Test.Sugar.Entity;
using Xunit;

namespace Redack.DatabaseLayer.Test
{
    public class TestRepository
    {
        [Theory, AutoData]
        public void TestInsert()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();
            var dbset = new Mock<DbSet<DummyEntity>>();

            var context = new Mock<IDbContext>();
            context.Setup(e => e.Set<DummyEntity>()).Returns(dbset.Object);

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Insert(obj);

            dbset.Verify(e => e.Add(It.IsAny<DummyEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public void TestUpdate()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new Mock<IDbContext>();

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Update(obj);

            context.Verify(e => e.SetEntityState(It.IsAny<DummyEntity>(), EntityState.Modified), Times.Once);
        }

        [Theory, AutoData]
        public void TestDelete()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();
            var dbset = new Mock<DbSet<DummyEntity>>();

            var context = new Mock<IDbContext>();
            context.Setup(e => e.Set<DummyEntity>()).Returns(dbset.Object);

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Delete(obj);

            dbset.Verify(e => e.Attach(It.IsAny<DummyEntity>()), Times.Once);
            dbset.Verify(e => e.Remove(It.IsAny<DummyEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public void TestCommit()
        {
            var context = new Mock<IDbContext>();

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Commit();

            context.Verify(e => e.SaveChanges(), Times.Once);
        }

        [Theory, AutoData]
        public void TestRollback()
        {
            var context = new Mock<IDbContext>();

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Rollback();

            context.Verify(e => e.Rollback(), Times.Once);
        }

        [Theory, AutoData]
        public void TestGetAll()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.CreateMany<DummyEntity>(3).AsQueryable();

            var dbset = new Mock<DbSet<DummyEntity>>();
            dbset.As<IQueryable<DummyEntity>>().Setup(e => e.Provider).Returns(obj.Provider);
            dbset.As<IQueryable<DummyEntity>>().Setup(e => e.Expression).Returns(obj.Expression);
            dbset.As<IQueryable<DummyEntity>>().Setup(e => e.ElementType).Returns(obj.ElementType);
            dbset.As<IQueryable<DummyEntity>>().Setup(e => e.GetEnumerator()).Returns(obj.GetEnumerator);

            var context = new Mock<IDbContext>();
            context.Setup(e => e.Set<DummyEntity>()).Returns(dbset.Object);

            var sut = new Repository<DummyEntity>(context.Object);
            var allObj = sut.GetAll();

            Assert.AreEqual(3, allObj.Count);
        }

        [Theory, AutoData]
        public void TestGetById()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();
            var dbset = new Mock<DbSet<DummyEntity>>();

            var context = new Mock<IDbContext>();
            context.Setup(e => e.Set<DummyEntity>()).Returns(dbset.Object);

            var sut = new Repository<DummyEntity>(context.Object);
            sut.GetById(obj.Id);

            dbset.Verify(e => e.Find(It.IsAny<int>()), Times.Once);
        }

        [Theory, AutoData]
        public void TestDispose()
        {
            var context = new Mock<IDbContext>();

            using (var sut = new Repository<DummyEntity>(context.Object)) {}

            context.Verify(e => e.Dispose(), Times.Once);
        }
    }
}
