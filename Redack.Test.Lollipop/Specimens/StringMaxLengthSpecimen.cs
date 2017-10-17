using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Redack.Test.Lollipop.Specimens
{
    public class StringMaxLengthSpecimen : ISpecimenBuilder
    {
        private readonly string _propertyName;
        private readonly int _length;

        public StringMaxLengthSpecimen(string propertyName, int length)
        {
            this._propertyName = propertyName;
            this._length = length;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && propertyInfo.Name == this._propertyName)
                return context.Create<string>().Substring(0, this._length);

            return new NoSpecimen();
        }
    }
}
