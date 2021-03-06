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
        [NotNull] private readonly IReadRepository<T> _readRepository;
        [NotNull] private readonly IUpdateRepository<T> _updateRepository;
        [NotNull] private readonly IOrderByClause<T>[] _orderByClauses;
        [NotNull] private readonly SelectExpression<T> _select;

        public Reorderer([NotNull] IReadRepository<T> readRepository, [NotNull] IUpdateRepository<T> updateRepository)
        {
            if (readRepository == null) throw new ArgumentNullException(nameof(readRepository));
            if (updateRepository == null) throw new ArgumentNullException(nameof(updateRepository));
            _readRepository = readRepository;
            _updateRepository = updateRepository;
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
                list.AddAfter(beforeNode, current);
            }
            await SaveAsync(orderingEntitiesPredicate, list);
        }

        /// <summary>
        ///     ������������� ������� � ������
        /// </summary>
        /// <param name="orderingEntitiesPredicate">�������� ��������� ����������� ���������</param>
        public async Task NormalizeAsync(Expression<Func<T, bool>> orderingEntitiesPredicate)
        {
            if (orderingEntitiesPredicate == null) throw new ArgumentNullException(nameof(orderingEntitiesPredicate));
            await SaveAsync(orderingEntitiesPredicate, await GetListAsync(orderingEntitiesPredicate));
        }

        [NotNull, ItemNotNull]
        private async Task<LinkedList<T>> GetListAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate)
        {
            var allChildren = await _readRepository.GetAsync(orderingEntitiesPredicate, _select, _orderByClauses);
            var list = new LinkedList<T>(allChildren.OrderBy(x => x.Order));
            return list;
        }

        private async Task SaveAsync([NotNull] Expression<Func<T, bool>> orderingEntitiesPredicate, [NotNull] IEnumerable<T> list)
        {
            var dict = list.Select((x, i) => new KeyValuePair<TId, int>(x.Id, i+1)).ToDictionary(x => x.Key, x => x.Value);
            await _updateRepository.UpdateAsync(obj =>
            {
                int newOrder;
                if (!dict.TryGetValue(obj.Id, out newOrder))
                {
                    throw new KeyNotFoundException("�� ������� ����� �������������");
                }
                obj.Order = newOrder;
            }, orderingEntitiesPredicate, null, false);
            await _updateRepository.SaveChangesAsync();
        }
    }
}