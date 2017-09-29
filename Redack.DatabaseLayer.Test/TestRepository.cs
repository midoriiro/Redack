using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.DataAccess;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using Redack.DatabaseLayer.Test.Sugar.Entity;
using Redack.DomainLayer.Model;
using Xunit;

namespace Redack.DatabaseLayer.Test
{
    public class TestRepository
    {
        [Theory, AutoData]
        public void TestInsertEntity_ValidUser_Success()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Customize(new EntityCustomization());
            fixture.Customize(new ValidUserCustomization());

            var user = fixture.Create<User>();
            var context = fixture.Freeze<Mock<RedackDbContext>>();
            var sut = fixture.Create<Mock<IRepository<User>>>();

            sut.Object.Insert(user);
            sut.Object.Commit();

            sut.Verify(e => e.Insert(It.IsAny<User>()), Times.Once);
            sut.Verify(e => e.Commit(), Times.Once);
        }
    }
}
