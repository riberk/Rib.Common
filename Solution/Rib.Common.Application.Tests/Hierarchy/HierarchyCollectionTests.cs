namespace Rib.Common.Application.Hierarchy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Models.Hierarchy;
    using TsSoft.Expressions.Helpers.Collections;

    [TestClass]
    public class HierarchyCollectionTests
    {
        [NotNull] private HierarchyCollection<HierarchycalCacheModel, int> _collection;
        [NotNull] private HierarchycalCacheModel[] _models;
        [NotNull] private Dictionary<int, HierarchycalCacheModel> _modelsDict;
        [NotNull] private HierarchycalCacheModel[] _orderedModels;
        [NotNull] private Mock<ITreeHelper> _th;
        [NotNull] private HierarchycalCacheModel[] _treeModels;
        [NotNull] private Dictionary<int, HierarchycalCacheModel> _treeModelsDict;

        [TestInitialize]
        public void Init()
        {
            _models = new[]
            {
                new HierarchycalCacheModel(1, 1),
                new HierarchycalCacheModel(2, 2),
                new HierarchycalCacheModel(3, 2, 1),
                new HierarchycalCacheModel(4, 1, 1),
                new HierarchycalCacheModel(5, 1, 3),
                new HierarchycalCacheModel(6, 1, 5)
            };
            _modelsDict = _models.ToDictionary(x => x.Id);
            _th = new Mock<ITreeHelper>(MockBehavior.Strict);
            _orderedModels = new[]
            {
                _modelsDict[1],
                _modelsDict[4],
                _modelsDict[3],
                _modelsDict[5],
                _modelsDict[6],
                _modelsDict[2],
            };
            var c51 = new HierarchycalCacheModel(6, 1, 5);
            var c31 = new HierarchycalCacheModel(5, 1, 3)
            {
                Children = new List<HierarchycalCacheModel>() {c51}
            };
            var c11 = new HierarchycalCacheModel(3, 2, 1)
            {
                Children = new List<HierarchycalCacheModel>() {c31}
            };
            var c12 = new HierarchycalCacheModel(4, 1, 1);

            _treeModels = new[]
            {
                new HierarchycalCacheModel(1, 1)
                {
                    Children = new List<HierarchycalCacheModel> {c11, c12}
                },
                new HierarchycalCacheModel(2, 2),
                c11,
                c12,
                c31,
                c51
            };
            _treeModelsDict = _treeModels.ToDictionary(x => x.Id);


            _th.Setup(x => x.OrderByHierarchy(
                                              _models,
                                              It.IsAny<Func<HierarchycalCacheModel, int>>(),
                                              It.IsAny<Func<HierarchycalCacheModel, int?>>(),
                                              It.IsAny<Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>>>(),
                                              It.IsAny<Action<HierarchycalCacheModel, HierarchycalCacheModel>>(),
                                              It.IsAny<Func<HierarchycalCacheModel, int>>(),
                                              It.IsAny<Action<HierarchycalCacheModel>>(),
                                              null))
               .Returns((
                       IEnumerable<HierarchycalCacheModel> models,
                       Func<HierarchycalCacheModel, int> id,
                       Func<HierarchycalCacheModel, int?> parentId,
                       Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>> children,
                       Action<HierarchycalCacheModel, HierarchycalCacheModel> parentSetter,
                       Func<HierarchycalCacheModel, int> field,
                       Action<HierarchycalCacheModel> postprocessor,
                       IComparer<int> c
                       ) =>
               {
                   var parent = new HierarchycalCacheModel(1, 888);
                   var m = new HierarchycalCacheModel(100, 101)
                   {
                       Children = new List<HierarchycalCacheModel>(),
                       Order = 999
                   };
                   Assert.AreEqual(m.Id, id(m));
                   Assert.AreEqual(m.ParentId, parentId(m));
                   Assert.AreEqual(m.Children, children(m));
                   Assert.AreEqual(m.Order, field(m));
                   Assert.AreNotEqual(1, m.ParentId);
                   parentSetter(m, parent);
                   Assert.AreEqual(parent, m.Parent);
                   postprocessor(m);
                   return _orderedModels;
               });
            _th.Setup(x => x.ConstructTreeStructureStable(
                                                          _orderedModels,
                                                          It.IsAny<Func<HierarchycalCacheModel, int>>(),
                                                          It.IsAny<Func<HierarchycalCacheModel, int?>>(),
                                                          It.IsAny<Action<HierarchycalCacheModel, HierarchycalCacheModel>>(),
                                                          It.IsAny<Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>>>()))
               .Returns((
                       IEnumerable<HierarchycalCacheModel> models,
                       Func<HierarchycalCacheModel, int> id,
                       Func<HierarchycalCacheModel, int?> parentId,
                       Action<HierarchycalCacheModel, HierarchycalCacheModel> parentSetter,
                       Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>> children
                       ) =>
               {
                   var parent = new HierarchycalCacheModel(1, 888);
                   var m = new HierarchycalCacheModel(100, 101)
                   {
                       Children = new List<HierarchycalCacheModel>(),
                       Order = 999
                   };
                   Assert.AreEqual(m.Id, id(m));
                   Assert.AreEqual(m.ParentId, parentId(m));
                   Assert.AreEqual(m.Children, children(m));
                   Assert.AreNotEqual(1, m.ParentId);
                   parentSetter(m, parent);
                   Assert.AreEqual(parent, m.Parent);
                   return _treeModels;
               });
            _collection = new HierarchyCollection<HierarchycalCacheModel, int>(_models, _th.Object);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            var untypedCollection = (IEnumerable) _collection;
            var dict = new Dictionary<int, HierarchycalCacheModel>();
            foreach (HierarchycalCacheModel m in untypedCollection)
            {
                dict.Add(m.Id, m);
            }
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(_treeModelsDict[1], dict[1]);
            Assert.AreEqual(_treeModelsDict[2], dict[2]);

        }

        [TestMethod]
        public void CountTest() => Assert.AreEqual(2, _collection.Count);

        [TestMethod]
        public void LeafTest()
        {
            MockTryGetValue();
            Assert.AreEqual(_treeModelsDict[5], _collection[5]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void LeafNotFound()
        {
            MockTryGetValue();
            Assert.AreEqual(_treeModelsDict[5], _collection[999]);
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            MockTryGetValue();
            HierarchycalCacheModel m;
            Assert.IsTrue(_collection.TryGetValue(5, out m));
            Assert.AreEqual(_treeModelsDict[5], m);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TryGetValueMultipleResultsTest()
        {
            _th.Setup(x => x.Find(It.IsAny<IEnumerable<HierarchycalCacheModel>>(),
                                  It.IsAny<Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>>>(),
                                  It.IsAny<Func<HierarchycalCacheModel, bool>>()))
               .Returns(_treeModels);
            HierarchycalCacheModel m;
            _collection.TryGetValue(5, out m);
        }

        [TestMethod]
        public void FlatEnumerableTest()
        {
            var flat = _collection.FlatEnumerable().ToArray();
            for (int i = 0; i < flat.Length; i++)
            {
                Assert.AreEqual(_orderedModels[i], flat[i]);
            }
        }

        [TestCleanup]
        public void Clean()
        {
            _th.VerifyAll();
        }

        private void MockTryGetValue()
        {
            _th.Setup(x => x.Find(It.IsAny<IEnumerable<HierarchycalCacheModel>>(),
                                  It.IsAny<Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>>>(),
                                  It.IsAny<Func<HierarchycalCacheModel, bool>>()))
               .Returns((IEnumerable<HierarchycalCacheModel> models,
                         Func<HierarchycalCacheModel, ICollection<HierarchycalCacheModel>> children,
                         Func<HierarchycalCacheModel, bool> predicate) =>
               {
                   var hcm = new HierarchycalCacheModel(1, 1)
                   {
                       Children = new List<HierarchycalCacheModel>()
                   };
                   Assert.AreEqual(hcm.Children, children(hcm));
                   return _treeModels.Where(predicate);
               }).Verifiable();
        }

        public class HierarchycalCacheModel : IHierarchycalCacheModel<HierarchycalCacheModel, int>
        {
            /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.Object" />.</summary>
            public HierarchycalCacheModel(int id, int order, int? parentId = null)
            {
                Id = id;
                ParentId = parentId;
                Order = order;
            }

            /// <summary>
            ///     Ключ экземпляра
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            ///     Дочерние сущности
            /// </summary>
            public ICollection<HierarchycalCacheModel> Children { get; set; }

            /// <summary>
            ///     Родительская сущность
            /// </summary>
            public HierarchycalCacheModel Parent { get; set; }

            /// <summary>
            ///     Порядок
            /// </summary>
            public int Order { get; set; }

            /// <summary>
            ///     Идентификатор родителя
            /// </summary>
            public int? ParentId { get; set; }

            public IEnumerable<HierarchycalCacheModel> Parents
            {
                get
                {
                    var p = Parent;
                    while (p != null)
                    {
                        yield return p;
                        p = p.Parent;
                    }
                }
            }
        }
    }
}