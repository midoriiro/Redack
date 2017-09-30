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
        public void TestInsertEntity_ValidUser_Success()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Customize(new EntityCustomization());
            fixture.Customize(new OmitOnRecursionCustomization());

            var obj = fixture.Create<DummyEntity>();
            var sut = fixture.Create<Mock<IRepository<DummyEntity>>>();

            sut.Object.Insert(obj);
            sut.Object.Commit();

            sut.Verify(e => e.Insert(It.IsAny<DummyEntity>()), Times.Once);
            sut.Verify(e => e.Commit(), Times.Once);
        }
    }
}
