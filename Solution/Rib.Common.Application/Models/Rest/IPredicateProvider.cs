namespace Rib.Common.Application.Models.Rest
{
    using System;
    using System.Linq.Expressions;

    public interface IPredicateProvider<T>
    {
        Expression<Func<T, bool>> Predicate();
    }
}