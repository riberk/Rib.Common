namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Runtime.Caching;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MemoryCacherTests
    {
        [NotNull] private Mock<ICachePolicyFactory> _itemPolicyMock;
        [NotNull] private MockRepository _mockFactory;
        private Mock<IObjectCacheFactory> _objectCacheFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _itemPolicyMock = _mockFactory.Create<ICachePolicyFactory>();
            _objectCacheFactory = _mockFactory.Create<IObjectCacheFactory>();
        }

        [NotNull]
        private static string FullKey(string prefix, string key) => MemoryCacher.FullKey(prefix, key);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new MemoryCacher<string>(null, _objectCacheFactory.Object, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument2() => new MemoryCacher<string>(_itemPolicyMock.Object, _objectCacheFactory.Object, null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument3() => new MemoryCacher<string>(_itemPolicyMock.Object, null, "123");

        [TestMethod]
        public void Constructor() => new MemoryCacher<string>(_itemPolicyMock.Object, _objectCacheFactory.Object, "321");

        [TestMethod]
        public void GetOrAddSimpleTest()
        {
            _itemPolicyMock.Setup(x => x.Create<One>()).Returns(new CacheItemPolicy()).Verifiable();
            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            var exp = new One();
            var key = "1sdrsdhsdrhsdr";
            Func<string, One> valueFactory = s =>
            {
                Assert.AreEqual(key, s);
                return exp;
            };
            _objectCacheFactory.Setup(x => x.Create()).Returns(MemoryCache.Default).Verifiable();
            var actual1 = cacher.GetOrAdd(key, valueFactory);
            var actual2 = cacher.GetOrAdd(key, s =>
            {
                Assert.Fail("Дважды вызвана фабрика");
                return null;
            });

            Assert.AreEqual(exp, actual1);
            Assert.AreEqual(exp, actual2);
            Assert.AreEqual(actual1, actual2);
        }

        [TestMethod]
        public void GetOrAddWithDifferentInstance()
        {
            _itemPolicyMock.Setup(x => x.Create<One>()).Returns(new CacheItemPolicy()).Verifiable();
            var cacher1 = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            var cacher2 = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            var exp = new One();
            var key = "1sdrsdhsryjtujuw54hrt5nfgmdrhsdr";
            Func<string, One> valueFactory = s =>
            {
                Assert.AreEqual(key, s);
                return exp;
            };
            _objectCacheFactory.Setup(x => x.Create()).Returns(MemoryCache.Default).Verifiable();
            var actual1 = cacher1.GetOrAdd(key, valueFactory);
            var actual2 = cacher2.GetOrAdd(key, s =>
            {
                Assert.Fail("Дважды вызвана фабрика");
                return null;
            });
            Assert.AreEqual(exp, actual1);
            Assert.AreEqual(exp, actual2);
            Assert.AreEqual(actual1, actual2);
        }

        [TestMethod]
        public void GetOrAddWithDifferentInstanceNullValue()
        {
            _itemPolicyMock.Setup(x => x.Create<One>()).Returns(new CacheItemPolicy()).Verifiable();
            var cacher1 = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            var cacher2 = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            var key = "1sdrsdsdrgsdrgsrdghdfhfgjkghklhsdrhsdr";
            Func<string, One> valueFactory = s =>
            {
                Assert.AreEqual(key, s);
                return null;
            };
            _objectCacheFactory.Setup(x => x.Create()).Returns(MemoryCache.Default).Verifiable();
            var actual1 = cacher1.GetOrAdd(key, valueFactory);
            var actual2 = cacher2.GetOrAdd(key, s =>
            {
                Assert.Fail("Дважды вызвана фабрика");
                return null;
            });
            Assert.IsNull(actual1);
            Assert.IsNull(actual2);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void GetOrAddWithCastExceptionTest()
        {
            const string prefix = "123";
            const string key = "321";

            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, prefix);

            var mCache = MemoryCache.Default;
            _objectCacheFactory.Setup(x => x.Create()).Returns(mCache).Verifiable();
            var fullKey = FullKey(prefix, key);
            mCache.Add(fullKey, "123", new CacheItemPolicy());
            cacher.GetOrAdd(key, s => new One());
        }

        [TestMethod]
        public void GetOrAddWithNullTest()
        {
            _itemPolicyMock.Setup(x => x.Create<One>()).Returns(new CacheItemPolicy()).Verifiable();
            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name);
            Func<string, One> valueFactory = s => null;
            const string key = "100sfgsdgrsdg";
            _objectCacheFactory.Setup(x => x.Create()).Returns(MemoryCache.Default).Verifiable();
            var actual1 = cacher.GetOrAdd(key, valueFactory);
            var actual2 = cacher.GetOrAdd(key, s =>
            {
                Assert.Fail("Дважды вызвана фабрика");
                return null;
            });

            Assert.IsNull(actual1);
            Assert.IsNull(actual2);
        }

        [TestMethod]
        public void RemoveTest()
        {
            const string prefix = "123123af";
            const string key = "321awdaw";

            var mCache = MemoryCache.Default;
            _objectCacheFactory.Setup(x => x.Create()).Returns(mCache).Verifiable();
            var fullKey = FullKey(prefix, key);
            var exp = new One();
            var resExp = mCache.AddOrGetExisting(fullKey, exp, new CacheItemPolicy());
            var actual = mCache.Get(fullKey);
            Assert.IsNull(resExp);
            Assert.AreEqual(exp, actual);

            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, prefix);
            cacher.Remove(key);
            var after = mCache.Get(fullKey);
            Assert.IsNull(after);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void RemoveArgumentNullTest() => new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, "12414").Remove(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void MemoryCacherOnRemoveArgumentNullTest() => new Testcache().Remove(null);

        [TestMethod]
        public void MemoryCacherOnRemoveTest()
        {
            const string key = "dsfgsdsdfgh";
            var cache = new Testcache();
            var raised = false;
            MemoryCacher.CacheItemRemoved += (sender, args) =>
            {
                Assert.IsNotNull(sender);
                Assert.AreEqual(cache, sender);
                Assert.IsNotNull(args);
                Assert.AreEqual(key, args.FullKey);
                raised = true;
            };
            cache.Remove(key);
            Assert.IsTrue(raised);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void MemoryCacherOnRemoveArgumentNull1Test()
        {
            var cache = new Testcache();
            cache.NullRemove();
        }

        [TestMethod]
        public void MemoryCacherOnAddTest()
        {
            const string key = "dsfgsdsdfgh";
            var cache = new Testcache();
            var raised = false;
            MemoryCacher.CacheItemAdded += (sender, args) =>
            {
                Assert.IsNotNull(sender);
                Assert.AreEqual(cache, sender);
                Assert.IsNotNull(args);
                Assert.AreEqual(key, args.FullKey);
                raised = true;
            };
            cache.Add(key);
            Assert.IsTrue(raised);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemoryCacherOnAddArgumentNull1Test()
        {
            var cache = new Testcache();
            cache.NullAdd();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetOrAddNullArgument1()
                => new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name).GetOrAdd(null, s => new One());

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetOrAddNullArgument2()
                => new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name).GetOrAdd("123", null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TryGetNullArgument()
        {
            One v;
            new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, typeof (One).Name).TryGet(null, out v);
        }

        [TestMethod]
        public void TryGetNullResultTest()
        {
            const string prefix = "123123aawdawdawdf";
            const string key = "321awdawawdawd";

            var mCache = MemoryCache.Default;
            _objectCacheFactory.Setup(x => x.Create()).Returns(mCache).Verifiable();

            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, prefix);
            One actual;
            var tryGetResult = cacher.TryGet(key, out actual);
            Assert.AreEqual(CacheTryGetResult.NotFound, tryGetResult);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TryGetEmptyResultTest()
        {
            const string prefix = "123123aawdawdawdf";
            const string key = "321awdawawdawd";

            var mCache = MemoryCache.Default;
            _objectCacheFactory.Setup(x => x.Create()).Returns(mCache).Verifiable();
            var fullKey = FullKey(prefix, key);
            mCache.Add(fullKey, Testcache.EmptyObjectValue, DateTimeOffset.MaxValue);
            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, prefix);
            One actual;
            var tryGetResult = cacher.TryGet(key, out actual);
            Assert.AreEqual(CacheTryGetResult.Empty, tryGetResult);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TryGetFoundResultTest()
        {
            const string prefix = "123123aawdawdsegsegsegawdf";
            const string key = "321awdawawdasefgsfgsegsegwd";

            var mCache = MemoryCache.Default;
            _objectCacheFactory.Setup(x => x.Create()).Returns(mCache).Verifiable();
            var fullKey = FullKey(prefix, key);
            var exp = new One();
            mCache.Add(fullKey, exp, DateTimeOffset.MaxValue);
            var cacher = new MemoryCacher<One>(_itemPolicyMock.Object, _objectCacheFactory.Object, prefix);
            One actual;
            var tryGetResult = cacher.TryGet(key, out actual);
            Assert.AreEqual(CacheTryGetResult.Found, tryGetResult);
            Assert.AreEqual(exp, actual);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }

        public class One
        {
        }

        public class Testcache : MemoryCacher
        {
            [NotNull]
            public static object EmptyObjectValue => EmptyObject;

            public void Remove(string key)
            {
                OnCacheItemRemove(this, new CacheEventArgs(key));
            }

            public void NullRemove()
            {
                OnCacheItemRemove(this, null);
            }

            public void Add(string key)
            {
                OnCacheItemAdd(this, new CacheEventArgs(key));
            }

            public void NullAdd()
            {
                OnCacheItemAdd(this, null);
            }
        }
    }
}