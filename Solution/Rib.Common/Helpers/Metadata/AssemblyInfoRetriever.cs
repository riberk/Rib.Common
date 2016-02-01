namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.IO;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Получатель информации о сборке
    /// </summary>
    internal class AssemblyInfoRetriever : IAssemblyInfoRetriever
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AssemblyInfoRetriever" />.
        /// </summary>
        public AssemblyInfoRetriever([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        /// <summary>
        ///     Получить информацию о сборке
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Информация о сборке</returns>
        public IAssemblyInfo Retrieve(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return _cacherFactory.Create<IAssemblyInfo>().GetOrAdd(assembly.FullName, s => CreateAssemblyInfo(assembly));
        }

        private IAssemblyInfo CreateAssemblyInfo([NotNull] Assembly assembly)
        {
            var buildDate = RetrieveLinkerTimestamp(assembly);
            var version = assembly.GetName().Version;
            var buildVersion = version?.ToString() ?? "NoVersion";
            return new AssemblyInfo(buildVersion, buildDate);
        }

        private static DateTime RetrieveLinkerTimestamp([NotNull] Assembly assembly)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var b = new byte[2048];

            using (var fs = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
            {
                fs.Read(b, 0, 2048);
            }

            var i = BitConverter.ToInt32(b, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(b, i + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }

        /// <summary>
        ///     Информация о сборке
        /// </summary>
        private class AssemblyInfo : IAssemblyInfo
        {
            /// <summary>
            ///     Инициализирует новый экземпляр класса <see cref="AssemblyInfo" />.
            /// </summary>
            public AssemblyInfo(string version, DateTime buildAt)
            {
                Version = version;
                BuildAt = buildAt;
            }

            /// <summary>
            ///     Версия
            /// </summary>
            public string Version { get; }

            /// <summary>
            ///     Дата и время сборки
            /// </summary>
            public DateTime BuildAt { get; }
        }
    }
}