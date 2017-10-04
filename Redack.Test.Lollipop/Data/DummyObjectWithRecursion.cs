namespace Redack.Test.Lollipop.Data
{
    public class DummyObjectWithRecursion : DummyObject
    {
        public virtual DummyObjectWithRecursion Property3 { get; set; }
    }
}