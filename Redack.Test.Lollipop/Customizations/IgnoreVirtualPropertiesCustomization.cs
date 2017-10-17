using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimens;

namespace Redack.Test.Lollipop.Customizations
{
    public class IgnoreVirtualPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualPropertiesSpecimen());
        }
    }
}
