namespace Rib.Tests.Ninject
{
    using System;
    using System.Linq;
    using System.Reflection;
    using global::Ninject;
    using JetBrains.Annotations;

    public static class TestKernelCreator
    {
        [NotNull]
        public static IKernel Create([NotNull] Func<IKernel> factory)
        {
            IKernel kernel;
            try
            {
                kernel = factory();
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new InvalidOperationException(
                    $"Не удалось создать ядро, не были загружены типы: {string.Join(";", ex.LoaderExceptions.Select(e => e.ToString()))}");
            }
            catch (TypeInitializationException ex)
            {
                var exception = ex.InnerException as ReflectionTypeLoadException;
                if (exception != null)
                {
                    throw new InvalidOperationException(
                        $"Не удалось создать ядро, не были загружены типы: {string.Join(";", exception.LoaderExceptions.Select(e => e.ToString()))}");
                }
                throw new InvalidOperationException(ex.ToString(), ex);
            }
            if (kernel == null)
            {
                throw new NullReferenceException(nameof(kernel));
            }
            return kernel;
        }
    }
}