using System;
using System.Net.Mail;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Redack.DatabaseLayer.Test.Sugar.Specimen
{
    class EmailAddressSpecimen : ISpecimenBuilder
    {
        private readonly string _propertyName;

        public EmailAddressSpecimen(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && propertyInfo.Name == this._propertyName)
                return context.Create<MailAddress>().Address;

            return new NoSpecimen();
        }
    }
}
