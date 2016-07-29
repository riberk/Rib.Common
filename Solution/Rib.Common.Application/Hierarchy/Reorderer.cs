namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Expressions;
    using Rib.Common.Models.Interfaces;
    using TsSoft.EntityRepository;
    using TsSoft.EntityRepository.Interfaces;
    using TsSoft.Expressions.OrderBy;
    using TsSoft.Expressions.SelectBuilder.Models;

    internal class Reorderer<T, TId> : IReorderer<T, TId>
            where T : class, IOrderedEntity, IEntityWithId<TId>
            where TId : struct, IComparable<TId>, IEquatable<TId>
    {
        [NotNull] private readonly IBaseRepository<T> _entityRepository;
        [NotNull] private readonly IOrderByClause<T>[] _orderByClauses;
        [NotNull] private readonly SelectExpression<T> _select;

        public Reorderer([NotNull] IBaseRepository<T> repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            _entityRepository = repository;
            _select = new SelectExpression<T>
            {
                Select = arg => new
                {
                    arg.Id,
                    arg.Order
                }
            };
            Expression<Func<T, int>> keySelector = arg => arg.Order;
            _orderByClauses = new IOrderByClause<T>[]
            {
                OrderByClause<T>.Create(keySelector.RemoveConvert())
            };
        }

        public async Task ReorderAsync(Expression<Func<T, bool>> orderingEntitiesPredicate, TId entityId, TId? idBefore)
        {
            if (orderingEntitiesPredicate == null) throw new ArgumentNullException(nameof(orderingEntitiesPredicate));
            var list = await GetListAsync(orderingEntitiesPredicate);
            var current = list.Single(x => x.Id.Equals(entityId));
            list.Remove(current);
            if (!idBefore.HasValue)
            {
                list.AddFirst(current);
            }
            else
            {
                var before = list.Single(x => x.Id.Equals(idBefore.Value));
                var beforeNode = list.Find(before);
                if (beforeNode == null)
                {
                    throw new InvalidOperationException($"Не найден элемент с ид {idBefore.Value}");
                }
                list.AddAfter(beforeNode, current);
            }
            await SaveAsync(orderingEntitiesPredicate, list);
        }

        /// <summary>
        ///     Нормализовать порядок в группе
        /// </summary>
        /// <param name="orderingEntitiesPredicate">Предикат получения сортируемых сущностей</param>
        public async Task NormalizeAsync(Expression<Func<T, bool>> orderingEntitiesPredicate)
        {
            if (orderingEntitiesPredicate == null) throw new ArgumentNullException(nameof(orderingEntitiesPredicate));
            await SaveAsync(orderingEntitiesPredicate, await GetListAsync(orderingEntitiesPredicate));
        }

        [NotNull, ItemNotNull]
        private async Task<LinkedList<T>> GetListAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate)
        {
            var allChildren = await _entityRepository.GetAsync(orderingEntitiesPredicate, _select, _orderByClauses);
            var list = new LinkedList<T>(allChildren.OrderBy(x => x.Order));
            return list;
        }

        private async Task SaveAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate, [NotNull] IEnumerable<T> list)
        {
            var dict = list.Select((x, i) => new KeyValuePair<TId, int>(x.Id, i)).ToDictionary(x => x.Key, x => x.Value);
            await _entityRepository.UpdateAsync(obj =>
            {
                int newOrder;
                if (!dict.TryGetValue(obj.Id, out newOrder))
                {
                    throw new KeyNotFoundException("Не удалось найти идентификатор");
                }
                obj.Order = newOrder;
            }, orderingEntitiesPredicate, null, false);
            await _entityRepository.SaveChangesAsync();
        }
    }
}