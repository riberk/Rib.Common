namespace Rib.Common.Application.Web.Razor
{
    using System;
    using System.Collections.Generic;
    using RazorEngine;
    using RazorEngine.Templating;

    internal class RazorTemplateRuner : IRazorTemplateRuner
    {
        public string Run(string view, string title, Type modelType, object model)
        {
            if (string.IsNullOrWhiteSpace(view)) throw new ArgumentNullException(nameof(view));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));

            if (!Engine.Razor.IsTemplateCached(view, modelType))
            {
                Engine.Razor.Compile(view, modelType);
            }

            var dynamicViewBag = new DynamicViewBag(new Dictionary<string, object> { { "Title", title } });
            var res = Engine.Razor.Run(view, modelType, model, dynamicViewBag);
            if (string.IsNullOrWhiteSpace(res))
            {
                throw new InvalidOperationException($"Шаблон {view} для типа {modelType} вернул null или пустую строку");
            }
            return res;
        }

        public string Run<T>(string view, T model)
        {
            if (string.IsNullOrWhiteSpace(view)) throw new ArgumentNullException(nameof(view));
            if (!Engine.Razor.IsTemplateCached(view, typeof(T)))
            {
                Engine.Razor.Compile(view, typeof(T));
            }
            var res = Engine.Razor.Run(view, typeof(T), model);
            if (string.IsNullOrWhiteSpace(res))
            {
                throw new InvalidOperationException($"Шаблон {view} для типа {typeof(T)} вернул null или пустую строку");
            }
            return res;
        }
    }
}