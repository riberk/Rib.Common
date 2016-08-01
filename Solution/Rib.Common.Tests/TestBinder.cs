namespace Rib.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.DependencyInjection;
    using Rib.Common.Models.Binding;

    public class TestBinder
    {
        [NotNull] private readonly IEnumerable<IBindInfo> _bindings;
        [NotNull] private readonly HashSet<Type> _genericInterfaces;
        [NotNull] private readonly IBinderHelper _helper;
        [NotNull] private readonly HashSet<Type> _interfaces;

        public TestBinder([NotNull] IBinderHelper helper, [NotNull] Assembly asm)
        {
            if (helper == null) throw new ArgumentNullException(nameof(helper));
            if (asm == null) throw new ArgumentNullException(nameof(asm));
            var types = asm.GetTypes();
            _bindings = helper.ReadFromTypes(types);
            var enumerable = types.Where(x => x.IsInterface).ToArray();
            _interfaces = new HashSet<Type>(enumerable.Where(x => !x.IsGenericType));
            _genericInterfaces = new HashSet<Type>(enumerable.Where(x => x.IsGenericType).Select(x => x.GetGenericTypeDefinition()));
        }

        public TestBinder Exclude<T>()
        {
            _interfaces.Remove(typeof(T));
            return this;
        }

        public TestBinder IncludeGeneric<T>()
        {
            var t = typeof(T);
            if (!t.IsGenericType || !_genericInterfaces.Contains(t.GetGenericTypeDefinition()))
            {
                throw new ArgumentException();
            }
            _interfaces.Add(typeof(T));
            return this;
        }

        public string[] Check()
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}