using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Redack.DatabaseLayer.Test.Sugar.Specimen
{
    class ValidEmailAddress : ISpecimenBuilder
    {
        private readonly string _propertyName;

        public ValidEmailAddress(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && propertyInfo.Name == this._propertyName)
                return new MailAddressGenerator();

            return new NoSpecimen();
        }
    }
}
