using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.DatabaseLayer.Test.Data;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Redack.DomainLayer.Model;
using Xunit;

namespace Redack.DatabaseLayer.Test
{
    public class TestRepository : TestBase
    {
        [Theory, AutoData]
        public void TestInsert()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, sut.GetById(obj.Id));
        }

        [Theory, AutoData]
        public void TestInsert_ExistingEntity_Fail()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            sut.Insert(obj);

            Assert.ThrowsException<DbUpdateException>(() => sut.Commit());
        }

        [Theory, AutoData]
        public void TestUpdate()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            sut.Update(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, sut.GetById(obj.Id));
        }

        [Theory, AutoData]
        public void TestUpdate_NonExistingEntity_Fail()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
 
            sut.Update(obj);

            Assert.ThrowsException<DbUpdateConcurrencyException>(() => sut.Commit());
        }

        [Theory, AutoData]
        public void TestDelete()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            sut.Delete(obj);
            sut.Commit();

            Assert.AreEqual(0, sut.GetAll().Count);
            Assert.IsNull(sut.GetById(obj.Id));
        }

        [Theory, AutoData]
        public void TestDelete_NonExistingEntity_Fail()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            sut.Delete(obj);

            Assert.ThrowsException<DbUpdateConcurrencyException>(() => sut.Commit());
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
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Rollback();

            Assert.AreEqual(0, sut.GetAll().Count);
            Assert.IsNull(sut.GetById(obj.Id));
        }

        [Theory, AutoData]
        public void TestGetAll()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.CreateMany<DummyEntity>(3);

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            var dummyEntities = obj as IList<DummyEntity> ?? obj.ToList();

            foreach (var o in dummyEntities)
            {
                sut.Insert(o);
            }

            sut.Commit();

            var orderedEntities = obj.OrderBy(e => e.Id).ToList();
            var orderedResult = sut.GetAll().OrderBy(e => e.Id).ToList();

            Assert.AreEqual(3, sut.GetAll().Count);
            Assert.AreEqual(orderedEntities[0], orderedResult[0]);
            Assert.AreEqual(orderedEntities[1], orderedResult[1]);
            Assert.AreEqual(orderedEntities[2], orderedResult[2]);
        }

        [Theory, AutoData]
        public void TestGetAll_EmptyList()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            Assert.AreEqual(0, sut.GetAll().Count);
        }

        [Theory, AutoData]
        public void TestGetById()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            var result = sut.GetById(obj.Id);

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, result);
        }

        [Theory, AutoData]
        public void TestGetById_NonExistingId()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            var result = sut.GetById(fixture.Create<int>());

            Assert.IsNull(result);
        }

        [Theory, AutoData]
        public void TestGetOrInsert_Insert()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj1 = fixture.Create<DummyEntity>();
            var obj2 = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj1);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);

            var result = sut.GetOrInsert(obj2);

            Assert.AreEqual(2, sut.GetAll().Count);
            Assert.AreEqual(obj2, result);
        }

        [Theory, AutoData]
        public void TestGetOrInsert_Get()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);

            var request = new DummyEntity()
            {
                Id = obj.Id,
                Property1 = "sometext"
               
            };

            var result = sut.GetOrInsert(request);

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, result);
        }

        [Theory, AutoData]
        public void TestInsertOrUpdate_Insert()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj1 = fixture.Create<DummyEntity>();
            var obj2 = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj1);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);

            sut.InsertOrUpdate(obj2);

            Assert.AreEqual(2, sut.GetAll().Count);
        }

        [Theory, AutoData]
        public void TestInsertOrUpdate_Update()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj1 = fixture.Create<DummyEntity>();
            var obj2 = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj1);
            sut.Insert(obj2);
            sut.Commit();

            Assert.AreEqual(2, sut.GetAll().Count);

            obj2.Property1 = "sometext";

            sut.InsertOrUpdate(obj2);

            Assert.AreEqual(2, sut.GetAll().Count);
        }

        [Theory, AutoData]
        public void TestExists()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.IsTrue(sut.Exists(obj));
        }

        [Theory, AutoData]
        public void TestExists_NonExisttingEntity_Fail()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            Assert.AreEqual(0, sut.GetAll().Count);
            Assert.IsFalse(sut.Exists(obj));
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
