namespace Rib.Tests.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject;
    using JetBrains.Annotations;
    using Moq;

    public class NinjectChecker
    {
        [NotNull] private readonly List<Exception> _errors = new List<Exception>();
        [NotNull] private readonly HashSet<Type> _interfaces;
        [NotNull] private readonly IKernel _kernel;
        [NotNull] private readonly MockRepository _mockFactory;

        public NinjectChecker([NotNull] Assembly assembly, [NotNull] IKernel kernel)
        {
            _kernel = kernel;
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            _interfaces = new HashSet<Type>(assembly.GetTypes().Where(x => x.IsInterface));
            _mockFactory = new MockRepository(MockBehavior.Loose);
        }

        [NotNull]
        public IReadOnlyCollection<Exception> Errors => _errors;

        [NotNull]
        public NinjectChecker Exclude<T>()
        {
            return Exclude(typeof (T));
        }
        [NotNull]
        public NinjectChecker Exclude(Type t)
        {
            _interfaces.Remove(t);
            if (!t.IsGenericTypeDefinition)
            {
                var obj = typeof (MockRepository).GetMethod("Create", new Type[0]).MakeGenericMethod(t).Invoke(_mockFactory, new object[0]);
                var value = obj.GetType().GetProperties().Single(x => x.Name == "Object" && x.PropertyType == t).GetValue(obj);
                _kernel.Bind(t).ToConstant(value);
            }
            return this;
        }
        [NotNull]
        public NinjectChecker CheckAllInterfacesCanCreate()
        {
            foreach (var @interface in _interfaces)
            {
                try
                {
                    _kernel.Get(@interface);
                }
                catch (Exception e)
                {
                    _errors.Add(e);
                }
            }
            return this;
        }
    }
}