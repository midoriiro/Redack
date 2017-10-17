using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Attributes
{
    class IgnorePropertiesAttribute : AutoDataAttribute
    {
        public IgnorePropertiesAttribute(string[] propertiesName) :
            base(new Fixture().Customize(new IgnorePropertiesCustomization(propertiesName)))
        {
        }
    }
}
