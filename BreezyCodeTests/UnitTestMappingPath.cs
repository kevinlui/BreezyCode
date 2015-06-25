using System;
using System.Collections.Generic;
using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BreezyCode.Classes;


namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestMappingPath
    {
        [TestMethod]
        public void TestMappingPath()
        {
            MappingPath mRightBias = new MappingPath(10, 10, MappingPath.DirectionBias.Right);

            Assert.AreEqual(mRightBias.GetPath(new Point(0, 0), new Point(9, 9), 
                new List<Point>{ new Point(5,5), new Point(6,6) }), 
                "RRRRRRRRRUUUUUUUUU");

            MappingPath mUpBias = new MappingPath(10, 10, MappingPath.DirectionBias.Up);

            Assert.AreEqual(mUpBias.GetPath(new Point(0, 0), new Point(9, 9),
                new List<Point> { new Point(5, 5), new Point(6, 6) }),
                "UUUUUUUUURRRRRRRRR");
        }
    }
}
