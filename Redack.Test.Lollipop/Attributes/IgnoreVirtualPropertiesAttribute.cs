using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Attributes
{
    class IgnoreVirtualPropertiesAttribute : AutoDataAttribute
    {
        public IgnoreVirtualPropertiesAttribute() :
            base(new Fixture().Customize(new IgnoreVirtualPropertiesCustomization()))
        {
        }
    }
}
