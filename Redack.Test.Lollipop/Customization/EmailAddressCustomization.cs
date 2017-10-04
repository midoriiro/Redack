using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class EmailAddressCustomization<T> : ICustomization where T : class
    {
        private readonly string _propertyName;

        public EmailAddressCustomization(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new EmailAddressSpecimen(this._propertyName));
        }
    }
}
