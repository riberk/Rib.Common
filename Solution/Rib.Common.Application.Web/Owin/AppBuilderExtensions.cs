namespace Rib.Common.Application.Web.Owin
{
    using System;
    using global::Common.Logging;
    using global::Owin;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.Owin.Modules;
    using Rib.Common.Application.Wrappers;
    using Rib.Common.Helpers.CorrelationId;

    public static class AppBuilderExtensions
    {
        [NotNull]
        public static IAppBuilder UseInitializer([NotNull] this IAppBuilder appBuilder, [NotNull] IWrappersManager wrappersManager)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            if (wrappersManager == null) throw new ArgumentNullException(nameof(wrappersManager));
            return appBuilder.Use(typeof(InitializeWrappersMiddleware), wrappersManager);
        }

        [NotNull]
        public static IAppBuilder UseExceptionHandler([NotNull] this IAppBuilder appBuilder, [NotNull] ILog logger)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return appBuilder.Use(typeof(ExceptionHandlerMiddleware), logger);
        }

        [NotNull]
        public static IAppBuilder UseLogger([NotNull] this IAppBuilder appBuilder, [NotNull] ILog logger)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return appBuilder.Use(typeof(LoggerMiddleware), logger);
        }


        [NotNull]
        public static IAppBuilder UseCorrelationIdHeader([NotNull] this IAppBuilder appBuilder,
                                                         [NotNull] ICorrelationIdStore correlationIdStore)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            return appBuilder.Use(typeof(SetCorrelationIdHeaderMiddleware), correlationIdStore);
        }

        [NotNull]
        public static IAppBuilder SetOwinToCallContext([NotNull] this IAppBuilder appBuilder, [NotNull] string key)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            return appBuilder.Use(typeof(SetOwinContextToCallContextMiddleware), key);
        }

        [NotNull]
        public static IAppBuilder Use404RedirectPage([NotNull] this IAppBuilder appBuilder, [NotNull] string page)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            return appBuilder.Use(typeof(RedirectMiddleware), page);
        }
    }
}