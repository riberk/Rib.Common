namespace Rib.Common.Application.Web.Razor
{
    using System;
    using JetBrains.Annotations;
    using RazorEngine.Templating;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Models.Metadata;

    [BindFrom(typeof(IActivator))]
    internal class RazorActivator : IActivator
    {
        [NotNull] private readonly IResolver _resolver;

        public RazorActivator([NotNull] IResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }

        /// <summary>
        ///     Creates an instance of the specifed template.
        /// </summary>
        /// <param name="context">The instance context.</param>
        /// <returns>
        ///     An instance of <see cref="T:RazorEngine.Templating.ITemplate" />.
        /// </returns>
        public ITemplate CreateInstance(InstanceContext context)
        {
            return _resolver.Get(context.TemplateType) as ITemplate;
        }
    }
}