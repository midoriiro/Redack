using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class IgnoreVirtualPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualPropertiesSpecimen());
        }
    }
}
