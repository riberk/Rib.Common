namespace Rib.Common.Application.Services.Rest.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Helpers.Expressions;
    using TsSoft.Expressions.Helpers;
    using TsSoft.Expressions.Helpers.Extensions;
    using TsSoft.Expressions.OrderBy;

    internal class OrderCreator<T> : IOrderCreator<T>
            where T : class
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;
        [NotNull] private readonly IPathConvertRemover _convertRemover;
        [NotNull] private readonly IPathBuilder _expressionBuilder;
        [NotNull] private readonly IPropertyStringResolver _propertyStringResolver;

        public OrderCreator([NotNull] IPathBuilder expressionBuilder,
                            [NotNull] IPropertyStringResolver propertyStringResolver,
                            [NotNull] IPathConvertRemover convertRemover,
                            [NotNull] ICacherFactory cacherFactory)
        {
            if (expressionBuilder == null) throw new ArgumentNullException(nameof(expressionBuilder));
            if (propertyStringResolver == null) throw new ArgumentNullException(nameof(propertyStringResolver));
            if (convertRemover == null) throw new ArgumentNullException(nameof(convertRemover));
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _expressionBuilder = expressionBuilder;
            _propertyStringResolver = propertyStringResolver;
            _convertRemover = convertRemover;
            _cacherFactory = cacherFactory;
        }

        public IEnumerable<IOrderByClause<T>> Create(IReadOnlyCollection<KeyValuePair<string, bool>> orderProperties)
        {
            return orderProperties?.Select(Create) ?? Create(new KeyValuePair<string, bool>("arg.Id", false)).ToEnumerable();
        }

        private IOrderByClause<T> Create(KeyValuePair<string, bool> order)
        {
            var cache = _cacherFactory.Create<IOrderByClause<T>>(GetType().FullName);
            return cache.GetOrAdd($"{order.Key}|{order.Value}", s => CreateInternal(order));
        }

        private IOrderByClause<T> CreateInternal(KeyValuePair<string, bool> order)
        {
            var path = _expressionBuilder.Build(_propertyStringResolver.GetProperties(typeof(T), order.Key));
            return OrderByClause<T>.Create<T>(_convertRemover.RemoveLast(path), order.Value);
        }
    }
}