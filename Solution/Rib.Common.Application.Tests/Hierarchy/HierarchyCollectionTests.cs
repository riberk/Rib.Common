namespace Rib.Common.Application.Hierarchy
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Application.Models.Hierarchy;
    using TsSoft.EntityRepository.Interfaces;

    [TestClass]
    public class HierarchyCollectionTests
    {
        [TestMethod]
        public void HierarchyCollectionTest()
        {
            var models = new[]
            {
                new HierarchycalCacheModel
                {
                    Id = 1,
                    ParentId = null,
                },
                new HierarchycalCacheModel
                {
                    Id = 2,
                    ParentId = null
                },
                new HierarchycalCacheModel
                {
                    Id = 3,
                    ParentId = 1
                },
                new HierarchycalCacheModel
                {
                    Id = 4,
                    ParentId = 1
                },
                new HierarchycalCacheModel
                {
                    Id = 5,
                    ParentId = 3
                },
                new HierarchycalCacheModel
                {
                    Id = 6,
                    ParentId = 5
                }
            };

            //new HierarchyCollection(models, )
            Assert.Fail();
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void LeafTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void FlatEnumerableTest()
        {
            Assert.Fail();
        }

        public class HierarchycalCacheModel : IHierarchycalCacheModel<HierarchycalCacheModel, int>
        {
            /// <summary>
            /// Ключ экземпляра
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Дочерние сущности
            /// </summary>
            public ICollection<HierarchycalCacheModel> Children { get; set; }

            /// <summary>
            /// Родительская сущность
            /// </summary>
            public HierarchycalCacheModel Parent { get; set; }

            /// <summary>
            ///     Порядок
            /// </summary>
            public int Order { get; set; }

            /// <summary>
            /// Идентификатор родителя
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