namespace Rib.Common.Application.Models.Rest
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using TsSoft.Expressions.Helpers;

    public abstract class CriteriaBase<T> : IPredicateProvider<T>
    {
        [NotNull]
        protected IPredicateBuilder<T> CreateBuilder()
        {
            return PredicateBuilder.True<T>().ToBuilder();
        }

        public abstract Expression<Func<T, bool>> Predicate();
    }
}