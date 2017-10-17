using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Redack.Test.Lollipop.Specimens
{
    public class PropertiesHasSameValuesSpecimen<TValue> : ISpecimenBuilder
    {
        private readonly string _propertyName;
        private readonly TValue _value;

        public PropertiesHasSameValuesSpecimen(string propertyName, TValue value)
        {
            this._propertyName = propertyName;
            this._value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && this._propertyName == propertyInfo.Name)
                return this._value;

            return new NoSpecimen();
        }
    }
}
