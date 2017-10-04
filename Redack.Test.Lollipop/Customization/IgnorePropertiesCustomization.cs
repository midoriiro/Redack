using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class IgnorePropertiesCustomization : ICustomization
    {
        private readonly string[] _propertiesName;

        public IgnorePropertiesCustomization(string[] propertiesName)
        {
            _propertiesName = propertiesName;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnorePropertiesSpecimen(this._propertiesName));
        }
    }
}
