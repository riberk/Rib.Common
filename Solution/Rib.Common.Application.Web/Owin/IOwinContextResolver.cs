namespace Rib.Common.Application.Web.Owin
{
    using Microsoft.Owin;

    public interface IOwinContextResolver
    {
        IOwinContext Current { get; }
    }
}