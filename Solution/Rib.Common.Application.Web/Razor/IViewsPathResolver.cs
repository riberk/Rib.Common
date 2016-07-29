namespace Rib.Common.Application.Web.Razor
{
    public interface IViewsPathResolver
    {
        string ResolveFullPath(string path);
    }
}