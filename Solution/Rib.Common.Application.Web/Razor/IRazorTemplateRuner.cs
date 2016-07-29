namespace Rib.Common.Application.Web.Razor
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(RazorTemplateRuner))]
    public interface IRazorTemplateRuner
    {
        [NotNull]
        string Run([NotNull] string view, [NotNull] string title, Type modelType, object model);

        [NotNull]
        string Run<T>([NotNull] string view, T model);
    }
}