using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BreezyCode.Classes;


namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestMethodOverloading
    {
        [TestMethod]
        public void TestMethodOverloading()
        {
            Base d = new Derived();
            MethodOverloading.OverloadedMethod(d);

            Base d2 = new Derived();
            d2.VirtualMethod();
        }
    }
}
