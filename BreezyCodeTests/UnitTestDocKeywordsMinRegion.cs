using System;
using System.Collections.Generic;
using System.Linq;

using BreezyCode.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestDocKeywordsMinRegion
    {
        [TestMethod]
        public void TestDocKeywordsMinRegionEdgeInputs()
        {
            // Empty document
            DocKeywordsMinRegion d = new DocKeywordsMinRegion("");
            verify( d.FindMinRegion(new List<string> { "w1", "w4", "w5" }), null );

            // null document
            d = new DocKeywordsMinRegion(null);
            verify( d.FindMinRegion(new List<string> { "w1" }), null );

            // Non-existent or empty or null Keywords cases
            d = new DocKeywordsMinRegion("w1 w4 w1 w3 w4 w5 w3 w1 w4 w5 w3 w3 w1 w2 w4 w2 w3 w5 w4");
            verify(d.FindMinRegion(null), null);                                // null keywords list
            verify(d.FindMinRegion(new List<string> { }), null);                // empty keywords list
            verify(d.FindMinRegion(new List<string> { "w4", "w7" }), null);     // non-existent keyword "w7"
        }

        [TestMethod]
        public void TestDocKeywordsMinRegionNomralInputs()
        {
            DocKeywordsMinRegion d =
                new DocKeywordsMinRegion("w1 w4 w1 w3 w4 w5 w3 w1 w4 w5 w3 w3 w1 w2 w4 w2 w3 w5 w4");

            //  Above doc string should result in this Index:
            //  { "w1", { 0, 2, 7, 12 } },
            //  { "w2", { 13, 15 } },
            //  { "w3", { 3, 6, 10, 11, 16 } },
            //  { "w4", { 1, 4, 8, 14, 18 } },
            //  { "w5", { 5, 9, 17 } }
            
            //  Verify result of a Test case:
            //  - Keywords Search on { "w2", "w4", "w5" } should result in { minSize:3, pos(14, 17) }
            verify( d.FindMinRegion(new List<string>{ "w2", "w4", "w5" }), 
                new Tuple<int, Tuple<int, int>>(3, new Tuple<int, int>(14, 17)));

            //
            //  Some other test cases below...
            //

            verify(d.FindMinRegion(new List<string> { "w1", "w4", "w5" }),
                new Tuple<int, Tuple<int, int>>(2, new Tuple<int, int>(7, 9)));

            verify(d.FindMinRegion(new List<string> { "w1", "w2", "w5" }),
                new Tuple<int, Tuple<int, int>>(4, new Tuple<int, int>(9, 13)));

            verify(d.FindMinRegion(new List<string> { "w3", "w4", "w5" }),
                new Tuple<int, Tuple<int, int>>(2, new Tuple<int, int>(3, 5)));

            verify(d.FindMinRegion(new List<string> { "w1", "w5" }),
                new Tuple<int, Tuple<int, int>>(2, new Tuple<int, int>(5, 7)));

            verify(d.FindMinRegion(new List<string> { "w2" }),
                new Tuple<int, Tuple<int, int>>(0, new Tuple<int, int>(13, 13)));

            verify(d.FindMinRegion(new List<string> { "w1", "w2", "w3", "w4", "w5" }),
                new Tuple<int, Tuple<int, int>>(5, new Tuple<int, int>(8, 13)));
        }

        private void verify(Tuple<int, Tuple<int, int>> t1, Tuple<int, Tuple<int, int>> t2)
        {
            if (t1 != null && t2 != null)
            {
                Assert.AreEqual(t1.Item1, t2.Item1);
                Assert.AreEqual(t1.Item2.Item1, t2.Item2.Item1);
                Assert.AreEqual(t1.Item2.Item2, t2.Item2.Item2);
            }
            else
            {
                Assert.IsTrue(t1 == null && t2 == null);
            }
        }
    }
}
