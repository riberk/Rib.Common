namespace Rib.Common.Ef.Enums
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.Expressions;
    using Rib.Common.Helpers.Metadata.Enums;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

    [TestClass]
    public class EnumEntityInitializerTests
    {
        public enum E1
        {
            None = 0,

            First = 1,
        }

        public enum E2
        {
            One = 15,

            Two = 20,
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumEntityInitializerTestCtorNull() => new EnumEntityInitializer(null, new Mock<INewMaker>().Object);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumEntityInitializerTestCtorNull2() => new EnumEntityInitializer(new Mock<IEnumAttributeReader>().Object, null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EnumEntityInitializerTestNull()
                => await new EnumEntityInitializer(new Mock<IEnumAttributeReader>().Object, new Mock<INewMaker>().Object)
                                 .FillEnumTablesAsync(null, Enumerable.Empty<Type>());

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EnumEntityInitializerTestNull2()
                => await new EnumEntityInitializer(new Mock<IEnumAttributeReader>().Object, new Mock<INewMaker>().Object)
                                 .FillEnumTablesAsync(await GetClearCtxAsync(), null);

        [TestMethod]
        public async Task EnumEntityInitializerTest()
        {
            var factory = new MockRepository(MockBehavior.Strict);
            var initializer = CreateAndMock(factory);
            var ctx = await GetClearCtxAsync();

            Assert.AreEqual(0, await ctx.E1Entities.CountAsync());
            Assert.AreEqual(0, await ctx.E2Entities.CountAsync());

            await initializer.FillEnumTablesAsync(ctx, new[] {typeof(E1Entity), typeof(E2Entity)});

            var e1result = await ctx.E1Entities.ToDictionaryAsync(x => x.Id, x => x);
            var e2result = await ctx.E2Entities.ToDictionaryAsync(x => x.Id, x => x);

            Assert.AreEqual(2, e1result.Count);

            Assert.AreEqual("123", e1result[E1.None].Title);
            Assert.AreEqual("None", e1result[E1.None].Name);
            Assert.IsFalse(e1result[E1.None].IsDeleted);

            Assert.AreEqual("fff", e1result[E1.First].Title);
            Assert.AreEqual("First", e1result[E1.First].Name);
            Assert.IsFalse(e1result[E1.First].IsDeleted);

            Assert.AreEqual(2, e2result.Count);

            Assert.AreEqual("o", e2result[E2.One].Title);
            Assert.AreEqual("One", e2result[E2.One].Name);
            Assert.IsFalse(e2result[E2.One].IsDeleted);

            Assert.AreEqual("t", e2result[E2.Two].Title);
            Assert.AreEqual("Two", e2result[E2.Two].Name);
            Assert.IsFalse(e2result[E2.Two].IsDeleted);

            factory.VerifyAll();
        }

        [TestMethod]
        public async Task EnumEntityInitializerWithExistsTest()
        {
            var factory = new MockRepository(MockBehavior.Strict);
            var initializer = CreateAndMock(factory);
            var ctx = await GetClearCtxAsync();

            Assert.AreEqual(0, await ctx.E1Entities.CountAsync());
            Assert.AreEqual(0, await ctx.E2Entities.CountAsync());

            ctx.E1Entities.Add(new E1Entity
            {
                Id = E1.None,
            });
            ctx.E1Entities.Add(new E1Entity
            {
                Id = E1.First,
            });
            ctx.E2Entities.Add(new E2Entity
            {
                Id = E2.One,
            });
            await ctx.SaveChangesAsync();
            await initializer.FillEnumTablesAsync(ctx, new[] { typeof(E1Entity), typeof(E2Entity) });

            var e1result = await ctx.E1Entities.ToDictionaryAsync(x => x.Id, x => x);
            var e2result = await ctx.E2Entities.ToDictionaryAsync(x => x.Id, x => x);

            Assert.AreEqual(2, e1result.Count);

            Assert.AreEqual(null, e1result[E1.None].Title);
            Assert.AreEqual(null, e1result[E1.None].Name);
            Assert.IsFalse(e1result[E1.None].IsDeleted);

            Assert.AreEqual(null, e1result[E1.First].Title);
            Assert.AreEqual(null, e1result[E1.First].Name);
            Assert.IsFalse(e1result[E1.First].IsDeleted);

            Assert.AreEqual(2, e2result.Count);

            Assert.AreEqual(null, e2result[E2.One].Title);
            Assert.AreEqual(null, e2result[E2.One].Name);
            Assert.IsFalse(e2result[E2.One].IsDeleted);

            Assert.AreEqual("t", e2result[E2.Two].Title);
            Assert.AreEqual("Two", e2result[E2.Two].Name);
            Assert.IsFalse(e2result[E2.Two].IsDeleted);

            factory.Verify();
        }

        [TestMethod]
        public async Task EnumEntityInitializerWithNeedDeleteTest()
        {
            var factory = new MockRepository(MockBehavior.Strict);
            var initializer = CreateAndMock(factory);
            var ctx = await GetClearCtxAsync();

            Assert.AreEqual(0, await ctx.E1Entities.CountAsync());
            Assert.AreEqual(0, await ctx.E2Entities.CountAsync());

            ctx.E1Entities.Add(new E1Entity
            {
                Id = E1.None,
            });
            ctx.E1Entities.Add(new E1Entity
            {
                Id = E1.First,
            });
            ctx.E2Entities.Add(new E2Entity
            {
                Id = E2.One,
            });
            ctx.E2Entities.Add(new E2Entity
            {
                Id = E2.Two,
            });
            var notExistEnumValue = (E2)99;
            ctx.E2Entities.Add(new E2Entity
            {
                Id = notExistEnumValue,
                IsDeleted = false
            });
            await ctx.SaveChangesAsync();
            await initializer.FillEnumTablesAsync(ctx, new[] { typeof(E1Entity), typeof(E2Entity) });

            var e1result = await ctx.E1Entities.ToDictionaryAsync(x => x.Id, x => x);
            var e2result = await ctx.E2Entities.ToDictionaryAsync(x => x.Id, x => x);

            Assert.AreEqual(2, e1result.Count);

            Assert.AreEqual(null, e1result[E1.None].Title);
            Assert.AreEqual(null, e1result[E1.None].Name);
            Assert.IsFalse(e1result[E1.None].IsDeleted);

            Assert.AreEqual(null, e1result[E1.First].Title);
            Assert.AreEqual(null, e1result[E1.First].Name);
            Assert.IsFalse(e1result[E1.First].IsDeleted);

            Assert.AreEqual(3, e2result.Count);

            Assert.AreEqual(null, e2result[E2.One].Title);
            Assert.AreEqual(null, e2result[E2.One].Name);
            Assert.IsFalse(e2result[E2.One].IsDeleted);

            Assert.AreEqual(null, e2result[E2.Two].Title);
            Assert.AreEqual(null, e2result[E2.Two].Name);
            Assert.IsFalse(e2result[E2.Two].IsDeleted);

            Assert.AreEqual(null, e2result[notExistEnumValue].Title);
            Assert.AreEqual(null, e2result[notExistEnumValue].Name);
            Assert.IsTrue(e2result[notExistEnumValue].IsDeleted);

            factory.Verify();
        }

        private async Task<Ctx> GetClearCtxAsync()
        {
            var ctx = new Ctx("Data Source=CurrentServer;Initial Catalog=Rib.Common.Ef.Enums.Tests;Integrated Security=SSPI;");
            (await ctx.E1Entities.ToListAsync()).ForEach(x => ctx.E1Entities.Remove(x));
            (await ctx.E2Entities.ToListAsync()).ForEach(x => ctx.E2Entities.Remove(x));
            await ctx.SaveChangesAsync();
            return ctx;
        }

        private EnumEntityInitializer CreateAndMock(MockRepository factory)
        {
            var enumAttrReader = factory.Create<IEnumAttributeReader>();
            var newMaker = factory.Create<INewMaker>();

            enumAttrReader.Setup(r => r.Attribute<DescriptionAttribute, E1>(E1.None))
                          .Returns(new DescriptionAttribute("123"));

            enumAttrReader.Setup(r => r.Attribute<DescriptionAttribute, E1>(E1.First))
                          .Returns(new DescriptionAttribute("fff"));

            enumAttrReader.Setup(r => r.Attribute<DescriptionAttribute, E2>(E2.One))
                          .Returns(new DescriptionAttribute("o"));

            enumAttrReader.Setup(r => r.Attribute<DescriptionAttribute, E2>(E2.Two))
                          .Returns(new DescriptionAttribute("t"));

            newMaker.Setup(x => x.Create<E1Entity>()).Returns(() => new E1Entity()).Verifiable();
            newMaker.Setup(x => x.Create<E2Entity>()).Returns(() => new E2Entity()).Verifiable();

            return new EnumEntityInitializer(enumAttrReader.Object, newMaker.Object);
        }


        public class Ctx : DbContext
        {
            public Ctx(string nameOrConnectionString) : base(nameOrConnectionString)
            {
            }

            public IDbSet<E1Entity> E1Entities { get; set; }
            public IDbSet<E2Entity> E2Entities { get; set; }
        }

        public class E1Entity : IEnumEntity<E1>
        {
            /// <summary>Ключ экземпляра</summary>
            public E1 Id { get; set; }

            /// <summary>Является ли сущность удалённой</summary>
            public bool IsDeleted { get; set; }

            /// <summary>Enum field name</summary>
            public string Name { get; set; }

            /// <summary>Title of enum value</summary>
            public string Title { get; set; }
        }

        public class E2Entity : IEnumEntity<E2>
        {
            /// <summary>Ключ экземпляра</summary>
            public E2 Id { get; set; }

            /// <summary>Является ли сущность удалённой</summary>
            public bool IsDeleted { get; set; }

            /// <summary>Enum field name</summary>
            public string Name { get; set; }

            /// <summary>Title of enum value</summary>
            public string Title { get; set; }
        }
    }
}