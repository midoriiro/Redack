using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Attribute
{
    class IgnorePropertiesAttribute : AutoDataAttribute
    {
        public IgnorePropertiesAttribute(string[] propertiesName) :
            base(new Fixture().Customize(new IgnorePropertiesCustomization(propertiesName)))
        {
        }
    }
}
