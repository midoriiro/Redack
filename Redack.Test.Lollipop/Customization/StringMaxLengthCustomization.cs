using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class StringMaxLengthCustomization<T> : ICustomization where T : class
    {
        private readonly string _propertyName;
        private readonly int _length;

        public StringMaxLengthCustomization(string propertyName, int length)
        {
            this._propertyName = propertyName;
            this._length = length;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new StringMaxLengthSpecimen(this._propertyName, this._length));
        }
    }
}
