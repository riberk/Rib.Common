namespace Rib.Common.Helpers.Configuration.Services
{
    using System;
    using JetBrains.Annotations;

    public interface IConfigurationTypeResolver
    {
        [NotNull]
        Type Resolve();
    }
}