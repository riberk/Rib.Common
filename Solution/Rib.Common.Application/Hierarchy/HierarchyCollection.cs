namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Application.Models.Hierarchy;
    using TsSoft.Expressions.Helpers.Collections;

    internal class HierarchyCollection<TModel, TId> : IHierarchyCollection<TModel, TId>
        where TModel : class, IHierarchycalCacheModel<TModel, TId>
        where TId : struct
    {
        [NotNull] private readonly IReadOnlyCollection<TModel> _ordered;
        [NotNull] private readonly IReadOnlyCollection<TModel> _tree;
        [NotNull] private readonly ITreeHelper _treeHelper;

        public HierarchyCollection(IEnumerable<TModel> models,
                                   [NotNull] ITreeHelper treeHelper)
        {
            if (treeHelper == null) throw new ArgumentNullException(nameof(treeHelper));
            _treeHelper = treeHelper;
            var ordered = _treeHelper.OrderByHierarchy(
                models,
                m => m.Id,
                m => m.ParentId,
                m => m.Children,
                (m, p) => m.Parent = p,
                m => m.Order,
                m => { });
            _ordered = new List<TModel>(ClearNavigation(ordered));
            var tree = _treeHelper.ConstructTreeStructureStable(
                _ordered,
                m => m.Id,
                m => m.ParentId,
                (m, p) => m.Parent = p,
                m => m.Children).Where(x => !x.ParentId.HasValue);
            _tree = new List<TModel>(tree);
        }

        /// <summary>
        ///     Возвращает перечислитель, выполняющий перебор элементов в коллекции.
        /// </summary>
        /// <returns>
        ///     Перечислитель, который можно использовать для итерации по коллекции.
        /// </returns>
        public IEnumerator<TModel> GetEnumerator()
        {
            return _tree.GetEnumerator();
        }

        /// <summary>
        ///     Возвращает перечислитель, который осуществляет итерацию по коллекции.
        /// </summary>
        /// <returns>
        ///     Объект <see cref="T:System.Collections.IEnumerator" />, который может использоваться для перебора коллекции.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Получает количество элементов коллекции.
        /// </summary>
        /// <returns>
        ///     Количество элементов коллекции.
        /// </returns>
        public int Count => _tree.Count;

        public TModel this[TId id]
        {
            get
            {
                TModel model;
                if (!TryGetValue(id, out model))
                {
                    throw new KeyNotFoundException();
                }
                return model;
            }
        }

        public bool TryGetValue(TId id, out TModel model)
        {
            var res = _treeHelper.Find(_tree, m => m.Children, m => m.Id.Equals(id)).ToArray();
            if (res.Length > 1)
            {
                throw new InvalidOperationException($"В иерархии несколько сущностей с Id {id}");
            }
            if (res.Length == 0)
            {
                model = default(TModel);
                return false;
            }
            model = res[0];
            return true;
        }

        public IEnumerable<TModel> FlatEnumerable()
        {
            return _ordered;
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TModel> ClearNavigation([NotNull] IEnumerable<TModel> source)
        {
            var s = source.ToList();
            foreach (var model in s)
            {
                model.Children?.Clear();
                model.Parent = null;
                yield return model;
            }
        }
    }
}