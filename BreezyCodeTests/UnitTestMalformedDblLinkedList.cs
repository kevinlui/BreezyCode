using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BreezyCode.Classes;


namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestMalformedDblLinkedList
    {
        [TestMethod]
        public void TestMalfromedDblLinkedList1()
        {
            DeepCopyVerify(null);
            DeepCopyVerify(new MalformedDblLinkedList.Node<char> { Value = 'A' });

        }

        [TestMethod]
        public void TestMalfromedDblLinkedList2()
        {
            var d = new MalformedDblLinkedList.Node<char> { Value = 'D' };
            var c = new MalformedDblLinkedList.Node<char> { Value = 'C', Next = d };
            var b = new MalformedDblLinkedList.Node<char> { Value = 'B', Next = c };
            var a = new MalformedDblLinkedList.Node<char> { Value = 'A', Next = b };
            b.BackPtr = d;
            c.BackPtr = b;
            d.BackPtr = a;

            DeepCopyVerify(a);
        }

        [TestMethod]
        public void TestMalfromedDblLinkedList3()
        {
            var e = new MalformedDblLinkedList.Node<char> { Value = 'E' };
            var d = new MalformedDblLinkedList.Node<char> { Value = 'D', Next = e };
            var c = new MalformedDblLinkedList.Node<char> { Value = 'C', Next = d };
            var b = new MalformedDblLinkedList.Node<char> { Value = 'B', Next = c };
            var a = new MalformedDblLinkedList.Node<char> { Value = 'A', Next = b };
            b.BackPtr = a;
            c.BackPtr = e;
            d.BackPtr = c;
            e.Next = null; e.BackPtr = b;

            DeepCopyVerify(a);
        }


        private void DeepCopyVerify(MalformedDblLinkedList.Node<char> list)
        {
            var clonedList = MalformedDblLinkedList.DeepCopy<char>(list);

            // Test to verify the Forward pointer list in Clone is in same order as Original
            string strOrg = RenderListForward(list);
            string strClone = RenderListForward(clonedList);
            Debug.Print(string.Format("RenderForward: strOrg={0} strClone={1}", strOrg, strClone));
            Assert.AreEqual(strOrg, strClone, string.Format("Mismatch: {0} != {1}", strOrg, strClone));

            // Test to verify the Back/Random pointer list in Clone is in same order as Original
            strOrg = RenderListBackPtr(list);
            strClone = RenderListBackPtr(clonedList);
            Debug.Print(string.Format("RenderOnRadom: strOrg={0} strClone={1}", strOrg, strClone));
            Assert.AreEqual(strOrg, strClone, string.Format("Mismatch on Random: {0} != {1}", strOrg, strClone));

        }

        /// <summary>
        /// Render out the list using Next pointer
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string RenderListForward(MalformedDblLinkedList.Node<char> n)
        {
            StringBuilder strBuild = new StringBuilder();
            while (n != null)
            {
                strBuild.Append(n.Value);
                n = n.Next;
            }

            return strBuild.ToString();
        }

        /// <summary>
        /// Render list of data using the BackPtr
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string RenderListBackPtr(MalformedDblLinkedList.Node<char> n)
        {
            StringBuilder strBuild = new StringBuilder();
            while (n != null)
            {
                strBuild.Append(string.Format("{0}{1},", n.Value.ToString(), (n.BackPtr != null ? n.BackPtr.Value : ' ')));
                n = n.Next;
            }

            return strBuild.ToString();
        }
    }
}