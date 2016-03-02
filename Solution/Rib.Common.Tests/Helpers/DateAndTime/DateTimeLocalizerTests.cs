namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DateTimeLocalizerTests
    {
        [NotNull] private readonly string _russianTz;
        [NotNull]private readonly string _tokioTz;
        private MockRepository _mockFactory;

        public DateTimeLocalizerTests()
        {
            _russianTz = "Russian Standard Time";
            _tokioTz = "Tokyo Standard Time";
        }

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
        }

        [NotNull]
        private DateTimeLocalizer Create()
        {
            return new DateTimeLocalizer();
        }

        [TestMethod]
        public void Constructor() => new DateTimeLocalizer();

        [TestMethod]
        public void UtcToLocalTest()
        {
            var dt = new DateTime(2015, 1, 1, 20, 22, 20, DateTimeKind.Utc);

            var actual = Create().ToLocal(dt, _russianTz);

            Assert.AreEqual(dt.Date, actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(dt.Hour + 3, actual.Hour);
            Assert.AreEqual(DateTimeKind.Unspecified, actual.Kind);
        }

        [TestMethod]
        public void UtcToLocalTokioTest()
        {
            var dt = new DateTime(2015, 1, 1, 10, 22, 20, DateTimeKind.Utc);

            var actual = Create().ToLocal(dt, _tokioTz);

            Assert.AreEqual(dt.Date, actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(dt.Hour + 9, actual.Hour);
            Assert.AreEqual(DateTimeKind.Unspecified, actual.Kind);
        }

        [TestMethod]
        public void LocalToLocalTest()
        {
            var dt = new DateTime(2015, 1, 1, 20, 22, 20, DateTimeKind.Local);

            var actual = Create().ToLocal(dt, TimeZoneInfo.Local.Id);

            Assert.AreEqual(dt, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalToLocalTokioTest()
        {
            var dt = new DateTime(2015, 1, 1, 20, 22, 20, DateTimeKind.Local);

            Create().ToLocal(dt, _tokioTz);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullToLocalTest()
        {
            var dt = DateTime.Now;
            Create().ToLocal(dt, null);
        }

        [TestMethod]
        public void UnspecifiedToLocalTest()
        {
            var dt = new DateTime(2015, 1, 1, 20, 22, 20, DateTimeKind.Unspecified);

            var actual = Create().ToLocal(dt, _russianTz);

            Assert.AreEqual(dt.Date, actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(dt.Hour + 3, actual.Hour);
            Assert.AreEqual(DateTimeKind.Unspecified, actual.Kind);
        }

        [TestMethod]
        public void UnspecifiedToLocalTokioTest()
        {
            var dt = new DateTime(2015, 1, 1, 10, 22, 20, DateTimeKind.Unspecified);

            var actual = Create().ToLocal(dt, _tokioTz);

            Assert.AreEqual(dt.Date, actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(dt.Hour + 9, actual.Hour);
            Assert.AreEqual(DateTimeKind.Unspecified, actual.Kind);
        }

        [TestMethod]
        public void UtcTestUnspecifaiedRst()
        {
            var dt = new DateTime(2015, 1, 1, 0, 0, 20, DateTimeKind.Unspecified);

            var actual = Create().ToUtc(dt, _russianTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(21, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestMethod]
        public void UtcTestUnspecifaiedTokio()
        {
            var dt = new DateTime(2015, 1, 1, 0, 0, 20, DateTimeKind.Unspecified);

            var actual = Create().ToUtc(dt, _tokioTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(15, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestMethod]
        public void UtcTestLocalRst()
        {
            var dt = new DateTime(2015, 1, 1, 0, 0, 20, DateTimeKind.Local);

            var actual = Create().ToUtc(dt, _russianTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(21, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestMethod]
        public void UtcTestLocalTokio()
        {
            var dt = new DateTime(2015, 1, 1, 0, 0, 20, DateTimeKind.Local);

            var actual = Create().ToUtc(dt, _tokioTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(15, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestMethod]
        public void UtcTestParsedRst()
        {
            var dt = DateTime.Parse("2015-01-01T00:00:20+03:00");

            var actual = Create().ToUtc(dt, _russianTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(21, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestMethod]
        public void UtcTestParsedTokio()
        {
            var dt = DateTime.Parse("2015-01-01T00:00:20+03:00");

            var actual = Create().ToUtc(dt, _tokioTz);

            Assert.AreEqual(new DateTime(2014, 12, 31), actual.Date);
            Assert.AreNotEqual(dt.Hour, actual.Hour);
            Assert.AreEqual(dt.Minute, actual.Minute);
            Assert.AreEqual(dt.Second, actual.Second);
            Assert.AreEqual(15, actual.Hour);
            Assert.AreEqual(DateTimeKind.Utc, actual.Kind);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}