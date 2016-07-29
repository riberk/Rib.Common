namespace Rib.Common.Application
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rib.Common.Application.Hierarchy;
    using Rib.Common.Application.Rest;
    using Rib.Common.Application.Wrappers;
    using Rib.Tests.Mock;
    using TsSoft.EntityRepository.Interfaces;

    [TestClass]
    public class NullArgumentTest
    {
        [TestMethod]
        public void AllNullArgumentThrow()
        {
            var res =
                    ArgumentsVerifier.Builder(typeof(IWrappersDisposer))
                            //Потому что инжектится ApplicationUserStore, а его невозможно создать моком
                                     .ToVerifier()
                                     .CheckAllVoidCtorsCreateObject()
                                     .CheckNullArgumentsOnConstructors()
                                     .CheckNullArgumentsOnConstructors<RestUpdateService<string, string>>()
                                     .CheckNullArgumentsOnConstructors<RestService<E, string, string, string, int>>()
                                     .CheckNullArgumentsOnConstructors<Reorderer<ReordererTests.Entity, int>>()
                                     .CheckNullArgumentsOnConstructors<HierarchyCache<HierarchyCacheTests.E, HierarchyCacheTests.Model, int>>()
                                     .CheckNullArgumentsOnConstructors<HierarchyCollection<HierarchyCollectionTests.HierarchycalCacheModel, int>>()
                                     .Errors;
            if (res.Any())
            {
                Assert.Fail(string.Join("\r\n\r\n", res));
            }
        }

        public class E : IEntityWithId<int>
        {
            /// <summary>Ключ экземпляра</summary>
            public int Id { get; set; }
        }
    }
}