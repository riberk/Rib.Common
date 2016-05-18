namespace Rib.Common.Application.Web.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;

    public class DependencyResolver : IDependencyResolver
    {
        [NotNull] private readonly IResolver _resolver;

        public DependencyResolver([NotNull] IResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
        }

        /// <summary> Starts a resolution scope. </summary>
        /// <returns>The dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            return new DependencyScope(_resolver);
        }

        /// <summary>Retrieves a service from the scope.</summary>
        /// <returns>The retrieved service.</returns>
        /// <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            return _resolver.TryGet(serviceType);
        }

        /// <summary>Retrieves a collection of services from the scope.</summary>
        /// <returns>The retrieved collection of services.</returns>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.GetAll(serviceType);
        }

        internal class DependencyScope : IDependencyScope
        {
            [NotNull] private readonly IResolver _resolver;

            public DependencyScope([NotNull] IResolver resolver)
            {
                if (resolver == null) throw new ArgumentNullException(nameof(resolver));
                _resolver = resolver;
            }

            public object GetService(Type serviceType)
            {
                return _resolver.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return _resolver.GetAll(serviceType);
            }

            public void Dispose()
            {
            }
        }
    }
}