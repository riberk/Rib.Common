namespace TestForNinjectBindingsErrorNamed
{
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(C1), "1")]
    [BindTo(typeof(C2), "1")]
    public interface I1
    {
        
    }
}