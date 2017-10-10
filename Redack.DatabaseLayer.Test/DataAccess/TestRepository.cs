using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.Test.Lollipop.Data;
using Redack.Test.Lollipop.Customization;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.DataAccess
{
    public class TestRepository : TestBase
    {
        [Theory, AutoData]
        public void Insert_WithValidEntity()
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
        public void Insert_WithExistingEntity()
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
        public void Update_WithValidEntity()
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
        public void Update_WithNonExistingEntity()
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
        public void Delete_WithValidEntity()
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
        public void Delete_NonExistingEntity()
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
        public void Commit()
        {
            var context = new Mock<IDbContext>();

            var sut = new Repository<DummyEntity>(context.Object);
            sut.Commit();

            context.Verify(e => e.SaveChanges(), Times.Once);
        }

        [Theory, AutoData]
        public async void CommitAsync()
        {
            var context = new Mock<IDbContext>();

            var sut = new Repository<DummyEntity>(context.Object);
            await sut.CommitAsync();

            context.Verify(e => e.SaveChangesAsync(), Times.Once);
        }

        [Theory, AutoData]
        public void Rollback()
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
        public void GetAll_WithValidEntitiesList()
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
        public void GetAll_WithEmptyEntitiesList()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            Assert.AreEqual(0, sut.GetAll().Count);
        }

        [Theory, AutoData]
        public async void GetAllAsync_WithValidEntitiesList()
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

            var orderedResult = await sut.GetAllAsync();
            orderedResult = orderedResult.OrderBy(e => e.Id).ToList();

            Assert.AreEqual(3, sut.GetAll().Count);
            Assert.AreEqual(orderedEntities[0], orderedResult[0]);
            Assert.AreEqual(orderedEntities[1], orderedResult[1]);
            Assert.AreEqual(orderedEntities[2], orderedResult[2]);
        }

        [Theory, AutoData]
        public async void GetAllAsync_WithEmptyEntitiesList()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            var result = await sut.GetAllAsync();

            Assert.AreEqual(0, result.Count);
        }

        [Theory, AutoData]
        public void GetById_WithExistingId()
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
        public void GetById_WithNonExistingId()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            var result = sut.GetById(fixture.Create<int>());

            Assert.IsNull(result);
        }

        [Theory, AutoData]
        public async void GetByIdAsync_WithExistingId()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            var result = await sut.GetByIdAsync(obj.Id);

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, result);
        }

        [Theory, AutoData]
        public async void GetByIdAsync_WithNonExistingId()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            var result = await sut.GetByIdAsync(fixture.Create<int>());

            Assert.IsNull(result);
        }

        [Theory, AutoData]
        public void GetOrInsert_WithEntityInsert()
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
            sut.Commit();

            Assert.AreEqual(2, sut.GetAll().Count);
            Assert.AreEqual(obj2, result);
        }

        [Theory, AutoData]
        public void GetOrInsert_WithEntityGet()
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
        public void InsertOrUpdate_WithEntityInsert()
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
            sut.Commit();

            Assert.AreEqual(2, sut.GetAll().Count);
        }

        [Theory, AutoData]
        public void InsertOrUpdate_WithEntityUpdate()
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
        public void Exists_WithExistingEntity()
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
        public void Exists_WithNonExistingEntity()
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
        public async void ExistsAsync_WithExistingEntity()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.IsTrue(await sut.ExistsAsync(obj));
        }

        [Theory, AutoData]
        public async void ExistsAsync_NonExisttingEntity()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);

            Assert.AreEqual(0, sut.GetAll().Count);
            Assert.IsFalse(await sut.ExistsAsync(obj));
        }

        [Theory, AutoData]
        public void All_WithValidEntity()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.All().Count());
        }

        [Theory, AutoData]
        public void Dispose()
        {
            var context = new Mock<IDbContext>();

            using (var sut = new Repository<DummyEntity>(context.Object)) {}

            context.Verify(e => e.Dispose(), Times.Once);
        }

        [Theory, AutoData]
        public void Query_WithValidEntity()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();

            var context = new DummyDbContext();

            var sut = new Repository<DummyEntity>(context);
            sut.Insert(obj);
            sut.Commit();

            var query = sut.Query(e => e.Id == obj.Id);

            Assert.AreEqual(1, query.Count());
        }
    }
}
