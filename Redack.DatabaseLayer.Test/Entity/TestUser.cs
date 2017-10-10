using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;
using Redack.Test.Lollipop;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Data;
using Redack.Test.Lollipop.Entity;
using System.Data.Entity.Validation;
using Redack.Test.Lollipop.Configuration;
using Xunit;

namespace Redack.DatabaseLayer.Test.Entity
{
    public class TestUser : TestBase
    {
        [Theory, AutoData]
        public void TestInsert_ValidUser_Success()
        {
            var fixture = new Fixture();
            fixture.Customize(new ValidUserCustomization());

            var obj = fixture.Create<User>();

            var context = new RedackDbContext();
            var sut = new Repository<User>(context);

            sut.Insert(obj);
            sut.Commit();

            Assert.AreEqual(1, sut.GetAll().Count);
            Assert.AreEqual(obj, sut.GetById(obj.Id));
        }

        [Theory, AutoData]
        public void TestCreate_ValidUser_Fail()
        {
            var fixture = new Fixture();
            fixture.Customize(new OmitOnRecursionCustomization());
            fixture.Customize(new IgnorePropertiesCustomization(new []
            {
                "Messages",
                "Group",
                "Permissions"
            }));

            var obj = fixture.Create<User>();

            var context = new RedackDbContext();
            var sut = new Repository<User>(context);

            sut.Insert(obj);

            Assert.ThrowsException<DbEntityValidationException>(() => sut.Commit());
        }
    }
}
