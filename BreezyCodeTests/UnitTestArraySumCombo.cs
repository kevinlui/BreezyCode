using System;
using BreezyCode.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestArraySumCombo
    {
        [TestMethod]
        public void TestArraySumCombo()
        {
            Assert.IsTrue(ArraySumCombo.HasCombo(new int[] { 1, 3, 5, 7, 9 }, 13));
            Assert.IsFalse(ArraySumCombo.HasCombo(new int[] { 1, 3, 5, 7, 9 }, 100));
        }
    }
}
