using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Entity;
using Redack.DomainLayer.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.DynamicProxy;
using Redack.DatabaseLayer.DataAccess;
using Redack.Test.Lollipop;
using Redack.Test.Lollipop.Data;
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

            var context = new DummyDbContext();
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

            var context = new DummyDbContext();
            var sut = new Repository<User>(context);

            sut.Insert(obj);

            Assert.ThrowsException<DbEntityValidationException>(() => sut.Commit());
        }
    }
}
