using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customization;

namespace Redack.Test.Lollipop.Attribute
{
    class IgnoreVirtualPropertiesAttribute : AutoDataAttribute
    {
        public IgnoreVirtualPropertiesAttribute() :
            base(new Fixture().Customize(new IgnoreVirtualPropertiesCustomization()))
        {
        }
    }
}
