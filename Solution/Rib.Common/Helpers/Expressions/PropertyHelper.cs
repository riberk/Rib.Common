namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;

    internal class PropertyHelper : IPropertyHelper
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public PropertyHelper([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        public object Get(PropertyInfo pi, object o)
        {
            if (pi == null) throw new ArgumentNullException(nameof(pi));
            if (o == null) throw new ArgumentNullException(nameof(o));

            return _cacherFactory.Create<Delegate>($"{GetType().FullName}::Get").GetOrAdd($"{pi.DeclaringType.FullName}|{pi.Name}", s =>
            {
                var p = Expression.Parameter(pi.DeclaringType);
                var lambda = Expression.Lambda(Expression.Property(p, pi), p);
                return lambda.Compile();
            }).DynamicInvoke(o);
        }

        public void Set(PropertyInfo pi, object o, object val)
        {
            if (pi == null) throw new ArgumentNullException(nameof(pi));
            if (o == null) throw new ArgumentNullException(nameof(o));

            _cacherFactory.Create<Delegate>($"{GetType().FullName}::Set").GetOrAdd($"{pi.DeclaringType.FullName}|{pi.Name}", s =>
            {
                var p = Expression.Parameter(pi.DeclaringType);
                var p2 = Expression.Parameter(pi.PropertyType);
                var property = Expression.Property(p, pi);
                var lambda = Expression.Lambda(Expression.Assign(property, p2), p, p2);
                return lambda.Compile();
            }).DynamicInvoke(o, val);
        }
    }
}