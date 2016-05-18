namespace Rib.Common.Application.Web.Razor
{
    using System;
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;
    using RazorEngine.Templating;
    using Rib.Common.Models.Metadata;

    [BindFrom(typeof(ITemplateManager))]
    public class TemplateManager : ITemplateManager
    {
        [NotNull] private readonly IViewsPathResolver _viewsPathResolver;

        public TemplateManager([NotNull] IViewsPathResolver viewsPathResolver)
        {
            if (viewsPathResolver == null) throw new ArgumentNullException(nameof(viewsPathResolver));
            _viewsPathResolver = viewsPathResolver;
        }

        /// <summary>
        ///     Resolves the template with the specified key.
        /// </summary>
        /// <param name="key">The key which should be resolved to a template source.</param>
        /// <returns>
        ///     The template content.
        /// </returns>
        public ITemplateSource Resolve(ITemplateKey key)
        {
            var fullPath = _viewsPathResolver.ResolveFullPath(key.Name);
            string template;
            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                template = sr.ReadToEnd();
            }
            return new LoadedTemplateSource(template);
        }

        /// <summary>
        ///     Get the key of a template.
        ///     This method has to be implemented so that the manager can control the
        ///     <see cref="T:RazorEngine.Templating.ITemplateKey" /> implementation.
        ///     This way the cache api can rely on the unique string given by
        ///     <see cref="M:RazorEngine.Templating.ITemplateKey.GetUniqueKeyString" />.
        /// </summary>
        /// <remarks>
        ///     For example one template manager reads all template from a single folder, then the
        ///     <see cref="M:RazorEngine.Templating.ITemplateKey.GetUniqueKeyString" /> can simply return the template name.
        ///     Another template manager can read from different folders depending whether we include a layout or including a
        ///     template.
        ///     In that situation the <see cref="M:RazorEngine.Templating.ITemplateKey.GetUniqueKeyString" /> has to take that into
        ///     account so that templates with the same name can not be confused.
        /// </remarks>
        /// <param name="name">The name of the template</param>
        /// <param name="resolveType">how the template is resolved</param>
        /// <param name="context">
        ///     gets the context for the current resolve operation.
        ///     Which template is resolving another template? (null = we search a global template)
        /// </param>
        /// <returns>
        ///     the key for the template
        /// </returns>
        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            return new NameOnlyTemplateKey(name, resolveType, context);
        }

        /// <summary>
        ///     Adds a template dynamically to the current manager.
        /// </summary>
        /// <param name="key" />
        /// <param name="source" />
        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            throw new NotImplementedException();
        }
    }
}