using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar;
using Redack.DatabaseLayer.Test.Sugar.Customization;
using Xunit;

namespace Redack.DatabaseLayer.Test.Sugar
{   
    public class TestCustomization
    {
        private readonly Fixture _fixture;

        public TestCustomization()
        {
            this._fixture = new Fixture();
        }

        [Fact]
        public void TestObjectFill()
        {
            var obj = this._fixture.Create<DummyObject>();

            Assert.IsNotNull(obj.Property1);
            Assert.IsNotNull(obj.Property2);
        }

        [Fact]
        public void TestIgnoreProperties()
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
        public void TestCopyPropertyValueToAnother()
        {
            this._fixture.Customize(new CopyPropertyValueToAnother<DummyObject>(
                "Property1", "Property2"));
            var obj = this._fixture.Create<DummyObject>();

            Assert.AreEqual(obj.Property1, obj.Property2);
        }

        [Fact]
        public void TestIgnoreVirtualProperties()
        {
            this._fixture.Customize(new IgnoreVirtualPropertiesCustomization());
            var obj = this._fixture.Create<DummyObjectWithRecursion>();

            Assert.IsNotNull(obj.Property1);
            Assert.IsNull(obj.Property2);
            Assert.IsNull(obj.Property3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TestOmitOnRecursion(int recursionDepth)
        {
            this._fixture.Customize(new OmitOnRecursionCustomization(recursionDepth));
            var obj = this._fixture.Create<DummyObjectWithRecursion>();

            Assert.IsNotNull(obj.Property1);
            Assert.IsNotNull(obj.Property2);

            if(recursionDepth == 1)
                Assert.IsNull(obj.Property3);
            else
                Assert.IsNotNull(obj.Property3);
        }
    }
}
