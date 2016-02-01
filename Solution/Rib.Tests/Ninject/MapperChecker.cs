namespace Rib.Tests.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject;
    using global::Ninject.Modules;
    using JetBrains.Annotations;

    public static class MapperChecker
    {
        [NotNull, ItemNotNull]
        public static IEnumerable<string> Check([NotNull] IEnumerable<INinjectModule> modules, [NotNull] IKernel kernel)
        {
            if (modules == null) throw new ArgumentNullException(nameof(modules));
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            var res = new List<string>();
            var methodInfos =
                typeof (AbstractMapperDependenciesChecker).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static |
                                                                      BindingFlags.FlattenHierarchy);
            var m = methodInfos.Single(x => x.Name == "CheckAlreadyRequestedMappersFromAssemblyWithModule");
            foreach (var ninjectModule in modules)
            {
                res.AddRange((IEnumerable<string>) m.MakeGenericMethod(ninjectModule.GetType()).Invoke(null, new object[] {kernel}));
            }
            return res;
        }
    }
}