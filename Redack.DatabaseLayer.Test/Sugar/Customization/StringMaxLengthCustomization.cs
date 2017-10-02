using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Redack.DatabaseLayer.Test.Sugar.Specimen;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class StringMaxLengthCustomization<T> : ICustomization where T : class
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
