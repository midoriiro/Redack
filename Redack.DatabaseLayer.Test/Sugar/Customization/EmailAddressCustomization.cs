using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar.Specimen;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class EmailAddressCustomization<T> : ICustomization where T : class
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
