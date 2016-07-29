namespace Rib.Common.Application.Web.WebApi.TypeFormatters
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Rib.Common.Models.Interfaces;

    public class StreramMediaTypeFormatter : MediaTypeFormatter
    {
        public StreramMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
        }
        /// <summary>
        ///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserializean object
        ///     of the specified type.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type;
        ///     otherwise, false.
        /// </returns>
        /// <param name="type">The type to deserialize.</param>
        public override bool CanReadType(Type type)
        {
            return type == typeof(IUploadedFile);
        }

        /// <summary>
        ///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serializean object of
        ///     the specified type.
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

        public override async Task<object> ReadFromStreamAsync(Type type,
                                                               Stream readStream,
                                                               HttpContent content,
                                                               IFormatterLogger formatterLogger,
                                                               CancellationToken cancellationToken)
        {
            var uf = new UploadedFile(StrHelper.UnquoteToken(content.Headers.ContentDisposition.FileName),
                                      await content.ReadAsStreamAsync());
            return uf;
        }


        private class UploadedFile: IUploadedFile
        {
            /// <summary>»нициализирует новый экземпл€р класса <see cref="UploadedFile" />.</summary>
            public UploadedFile(string fileName, Stream file)
            {
                FileName = fileName;
                File = file;
            }
            public string FileName { get; }
            public Stream File { get; }
        }
    }
}