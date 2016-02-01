namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.IO;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Metadata;
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///     Получатель информации о сборке
    /// </summary>
    internal class AssemblyInfoRetriever : IAssemblyInfoRetriever
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;
        [NotNull] private readonly ILog _log;

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="AssemblyInfoRetriever" />.
        /// </summary>
        public AssemblyInfoRetriever([NotNull] ICacherFactory cacherFactory, [NotNull] ILog log)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            if (log == null) throw new ArgumentNullException(nameof(log));
            _cacherFactory = cacherFactory;
            _log = log;
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
            _log.Trace($"Create assembly info for {assembly.FullName}");
            var buildDate = RetrieveLinkerTimestamp(assembly);
            _log.Trace($"Build date {assembly.FullName}: {buildDate}");
            var version = assembly.GetName().Version;
            var buildVersion = version?.ToString() ?? "NoVersion";
            _log.Trace($"Version {assembly.FullName}: {buildVersion}");
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