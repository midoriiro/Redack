using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Specimen;

namespace Redack.Test.Lollipop.Customization
{
    public class CopyPropertyValueToAnother<T> : ICustomization where T : class
    {
        private readonly string _from;
        private readonly string _to;

        public CopyPropertyValueToAnother(string from, string to)
        {
            this._from = from;
            this._to = to;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnorePropertiesSpecimen(new string[]
            {
                this._from, this._to
            }));

            fixture.Customize<T>(b => b
                .Do(o =>
                {
                    o.GetType().GetProperty(this._from)?.SetValue(o, fixture.Create<string>());
                    o.GetType().GetProperty(this._to)?.SetValue(
                        o, o.GetType().GetProperty(this._from)?.GetValue(o));
                }));
        }
    }
}
