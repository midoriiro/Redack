using System.Net.Mail;
using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Customization;
using Redack.Test.Lollipop.Data;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Redack.Test.Lollipop
{
    public class TestCustomization
    {
        private readonly Fixture _fixture;

        public TestCustomization()
        {
            this._fixture = new Fixture();
        }

        [Fact]
        public void ObjectFill()
        {
            var obj = this._fixture.Create<DummyObject>();

            Assert.IsNotNull(obj.Property1);
            Assert.IsNotNull(obj.Property2);
        }

        [Fact]
        public void IgnoreProperties()
        {
            this._fixture.Customize(new IgnorePropertiesCustomization(new string[]
            {
                "Property1",
                "Property2"
            }));

            var obj = this._fixture.Create<DummyObject>();

            Assert.IsNull(obj.Property1);
            Assert.IsNull(obj.Property2);
        }

        [Fact]
        public void CopyPropertyValueToAnother()
        {
            this._fixture.Customize(new CopyPropertyValueToAnother<DummyObject>(
                "Property1", "Property2"));
            var obj = this._fixture.Create<DummyObject>();

            Assert.AreEqual(obj.Property1, obj.Property2);
        }

        [Fact]
        public void IgnoreVirtualProperties()
        {
            this._fixture.Customize(new IgnoreVirtualPropertiesCustomization());
            var obj = this._fixture.Create<DummyObjectWithRecursion>();

            Assert.IsNull(obj.Property2);
            Assert.IsNull(obj.Property3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void OmitOnRecursion(int recursionDepth)
        {
            this._fixture.Customize(new OmitOnRecursionCustomization(recursionDepth));
            var obj = this._fixture.Create<DummyObjectWithRecursion>();

            if(recursionDepth == 1)
                Assert.IsNull(obj.Property3);
            else
                Assert.IsNotNull(obj.Property3);
        }

        [Fact]
        public void EmailAddress()
        {
            this._fixture.Customize(new EmailAddressCustomization<DummyObject>("Property1"));
            var obj = this._fixture.Create<DummyObject>();

            try
            {
                var email = new MailAddress(obj.Property1);

                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Fact]
        public void StringMaxLength()
        {
            this._fixture.Customize(new StringMaxLengthCustomization<DummyObject>("Property1", 10));
            var obj = this._fixture.Create<DummyObject>();

            Assert.AreEqual(10, obj.Property1.Length);
        }

        [Fact]
        public void PropertiesHasSameValues()
        {
            this._fixture.Customize(new PropertiesHasSameValuesCustomization<string>("Property1"));
            this._fixture.Customize(new OmitOnRecursionCustomization(2));

            var obj = this._fixture.Create<DummyObjectWithRecursion>();

            Assert.AreEqual(obj.Property1, obj.Property3.Property1);
        }
    }
}
