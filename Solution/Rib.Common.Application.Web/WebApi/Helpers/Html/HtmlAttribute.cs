namespace Rib.Common.Application.Web.WebApi.Helpers.Html
{
    using System;
    using JetBrains.Annotations;

    public class HtmlAttribute : Attribute
    {
        /// <summary>
        ///     »нициализирует новый экземпл€р класса <see cref="HtmlAttribute" />.
        /// </summary>
        public HtmlAttribute([NotNull] string title)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            Title = title;
        }


        [NotNull]
        public string Title { get; }

        public string Path { get; set; }
    }
}