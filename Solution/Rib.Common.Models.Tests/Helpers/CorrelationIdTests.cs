namespace Rib.Common.Models.Helpers
{
    using System;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CorrelationIdTests
    {
        [TestMethod]
        public void CorrelationIdTest() => new CorrelationId();

        [TestMethod]
        public void CorrelationIdTest1() => new CorrelationId(Guid.NewGuid());

        [TestMethod]
        public void TestProvidedMethods()
        {
            var i1 = Guid.NewGuid();
            var i2 = Guid.NewGuid();

            var c1 = new CorrelationId(i1);
            var c2 = new CorrelationId(i2);

            Assert.AreEqual(i1.CompareTo(i2), c1.CompareTo(c2));                    // Compare correlation id and correlation id
            Assert.AreEqual(i1.CompareTo(i2), c1.CompareTo(i2));                    // Compare correlation id and Guid
            Assert.AreEqual(i1.CompareTo((object)i2), c1.CompareTo((object)c2));    // Compare correlation id and object cid
            Assert.AreEqual(i1.CompareTo((object)i2), c1.CompareTo((object)i2));    // Compare correlation id and object Guid

            Assert.IsTrue(c1.Equals(new CorrelationId(i1)));                        // Equals correlation id and correlation id
            Assert.IsFalse(c1.Equals(new CorrelationId(i2)));                       // Not equals correlation id and correlation id
            Assert.IsTrue(c1.Equals((object)new CorrelationId(i1)));                // Equals correlation id and object correlation id
            Assert.IsFalse(c1.Equals((object)new CorrelationId(i2)));               // Not equals correlation id and object correlation id

            Assert.IsTrue(c1.Equals(i1));                                           // Equals correlation id and Guid
            Assert.IsFalse(c1.Equals(i2));                                          // Not equals correlation id and Guid
            Assert.IsFalse(c1.Equals((object)i1));                                  // Not equals correlation id and object Guid
            Assert.IsFalse(c1.Equals((object)i2));                                  // Not equals correlation id and object Guid

            Assert.AreEqual(i1.GetHashCode(), c1.GetHashCode());                    // GetHashCode
            Assert.AreNotEqual(i2.GetHashCode(), c1.GetHashCode());                 
            Assert.AreEqual(c1.GetHashCode(), c1.GetHashCode());                    
            Assert.AreNotEqual(c2.GetHashCode(), c1.GetHashCode());                 

            CollectionAssert.AreEqual(i1.ToByteArray(), c1.ToByteArray());          // To byte array
            CollectionAssert.AreNotEqual(i2.ToByteArray(), c1.ToByteArray());
            CollectionAssert.AreEqual(i2.ToByteArray(), c2.ToByteArray());
            CollectionAssert.AreNotEqual(i1.ToByteArray(), c2.ToByteArray());                 


            Assert.AreEqual(i1.ToString(), c1.ToString());                          // To string
            Assert.AreNotEqual(i2.ToString(), c1.ToString());                       
            Assert.AreEqual(i2.ToString(), c2.ToString());                          
            Assert.AreNotEqual(i1.ToString(), c2.ToString());                       
                                                                                    
            Assert.AreEqual(i1.ToString("N"), c1.ToString("N"));                    // To string with format
            Assert.AreNotEqual(i2.ToString("N"), c1.ToString("N"));                 
            Assert.AreEqual(i2.ToString("N"), c2.ToString("N"));                    
            Assert.AreNotEqual(i1.ToString("N"), c2.ToString("N"));                 
                                                                                    
            var c = CultureInfo.CurrentCulture;                                     
            Assert.AreEqual(i1.ToString("D", c), c1.ToString("D", c));              // To string with format and provider
            Assert.AreNotEqual(i2.ToString("D", c), c1.ToString("D", c));           
            Assert.AreEqual(i2.ToString("D", c), c2.ToString("D", c));              
            Assert.AreNotEqual(i1.ToString("D", c), c2.ToString("D", c));           
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareToException()
        {
            new CorrelationId().CompareTo("123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompareToNullArgument()
        {
            new CorrelationId().CompareTo((CorrelationId)null);
        }

        [TestMethod]
        public void TestImplicitConversion()
        {
            var i = Guid.NewGuid();
            CorrelationId c = i;
            Guid g = c;

            Assert.IsTrue(c.Equals(i));
            Assert.IsTrue(i.Equals(g));
            Assert.IsTrue(c.Equals(g));
        }

        [TestMethod]
        public void TestRefEquals()
        {
            var c = new CorrelationId();
            Assert.IsTrue(c.Equals(c));
            Assert.IsTrue(c.Equals((object)c));
            Assert.IsFalse(c.Equals((object)null));
            Assert.IsFalse(c.Equals((CorrelationId)null));
        }
    }
}