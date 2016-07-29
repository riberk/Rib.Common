namespace Rib.Common.Application.Web.WebApi.TypeFormatters
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Expressions;

    public class MultipartMediaTypeFormatter : MediaTypeFormatter
    {
        [NotNull]
        private readonly MediaTypeFormatterCollection _mediaTypeFormatterCollection;
        [NotNull]
        private readonly IPropertyHelper _propertyHelper;

        public MultipartMediaTypeFormatter([NotNull] MediaTypeFormatterCollection mediaTypeFormatterCollection,
                                           [NotNull] IPropertyHelper propertyHelper)
        {
            if (mediaTypeFormatterCollection == null) throw new ArgumentNullException(nameof(mediaTypeFormatterCollection));
            if (propertyHelper == null) throw new ArgumentNullException(nameof(propertyHelper));
            _mediaTypeFormatterCollection = mediaTypeFormatterCollection;
            _propertyHelper = propertyHelper;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        /// <summary>
        ///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserializean object of the
        ///     specified type.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type; otherwise,
        ///     false.
        /// </returns>
        /// <param name="type">The type to deserialize.</param>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        ///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serializean object of the
        ///     specified type.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize the type; otherwise,
        ///     false.
        /// </returns>
        /// <param name="type">The type to serialize.</param>
        public override bool CanWriteType(Type type)
        {
            return false;
        }

        /// <summary>
        ///     Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.
        /// </returns>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <exception cref="T:System.NotSupportedException">Derived types need to support reading.</exception>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.
        /// </returns>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <param name="cancellationToken">The token to cancel the operation.</param>
        public override async Task<object> ReadFromStreamAsync(Type type,
                                                               Stream readStream,
                                                               HttpContent content,
                                                               IFormatterLogger formatterLogger,
                                                               CancellationToken cancellationToken)
        {
            if (!content.IsMimeMultipartContent())
            {
                throw new FormatException("Only multipart/form-data supported");
            }
            var allProps = type.GetProperties().ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);
            var instance = Activator.CreateInstance(type);
            var mmsp = await content.ReadAsMultipartAsync();
            foreach (var httpContent in mmsp.Contents)
            {
                var pName = StrHelper.UnquoteToken(httpContent.Headers.ContentDisposition.Name);
                PropertyInfo pi;
                if (!allProps.TryGetValue(pName, out pi))
                {
                    continue;
                }

                object value;
                if (httpContent.Headers.ContentType == null)
                {
                    var converter = TypeDescriptor.GetConverter(pi.PropertyType);
                    value = converter.ConvertFromString(await httpContent.ReadAsStringAsync());
                }
                else
                {
                    var reader = _mediaTypeFormatterCollection.FindReader(pi.PropertyType, httpContent.Headers.ContentType);
                    if (reader == null)
                    {
                        throw new InvalidOperationException(
                                $"Could not found reader for type {pi.PropertyType} with ContentTye {httpContent.Headers.ContentType}");
                    }
                    value = await reader.ReadFromStreamAsync(pi.PropertyType,
                                                             await httpContent.ReadAsStreamAsync(),
                                                             httpContent,
                                                             formatterLogger,
                                                             cancellationToken);
                }
                _propertyHelper.Set(pi, instance, value);
            }
            return instance;
        }
    }
}
