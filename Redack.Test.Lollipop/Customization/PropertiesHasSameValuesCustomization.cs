using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class PropertiesHasSameValuesCustomization<TValue> : ICustomization
    {
        private readonly string _propertyName;
        private readonly TValue _value;

        public PropertiesHasSameValuesCustomization(string propertyName)
        {
            this._propertyName = propertyName;

            Fixture fixture = new Fixture();
            this._value = fixture.Create<TValue>();
        }

        public PropertiesHasSameValuesCustomization(string propertyName, TValue value)
        {
            this._propertyName = propertyName;
            this._value = value;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new PropertiesHasSameValuesSpecimen<TValue>(this._propertyName, this._value));   
        }
    }
}
