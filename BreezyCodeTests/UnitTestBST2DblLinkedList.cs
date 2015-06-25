using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BreezyCode.Classes.BST;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestBST2DblLinkedList
    {
        [TestMethod]
        public void TestBST2DblLinkedList_Basic()
        {
            Node<int> bst1 = new Node<int>
            {
                Value = 2, 
                Left = new Node<int> { Value = 1 }, 
                Right = new Node<int> { Value = 3}
            };

            AssertConvertedList(
                Node<int>.ConvertToDblLinkedList(bst1), 
                new List<int>(new int[] { 1, 2, 3 }) );

            Node<int> bst2 = new Node<int>
            {
                Value = 4,
                Left = new Node<int>
                {
                    Value = 2,
                    Left = new Node<int> { Value = 1 },
                    Right = new Node<int> { Value = 3 }
                },
                Right = new Node<int>
                {
                    Value = 6,
                    Left = new Node<int> { Value = 5 },
                    Right = new Node<int> { Value = 7 }
                }
            };

            AssertConvertedList(
                Node<int>.ConvertToDblLinkedList(bst2), 
                new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 }));
        }

        [TestMethod]
        public void TestBST2DblLinkedList_LongSequential()
        {
            TestBST2DblLinkedListLong(13, false);
        }

        [TestMethod]
        public void TestBST2DblLinkedList_LongParallel()
        {
            TestBST2DblLinkedListLong(13, true);
        }

        private void TestBST2DblLinkedListLong(int level, bool parallel)
        {
            long max = (long)(Math.Pow(2, level));

            Node<long> bst = new Node<long> { Value = max / 2 };

            if (parallel)
            {
                Parallel.Invoke(
                    () => { GenSubTree(bst, level - 1, true, true); },
                    () => { GenSubTree(bst, level - 1, false, true); });
            }
            else
            {
                GenSubTree(bst, level - 1, true, false);
                GenSubTree(bst, level - 1, false, false);
            }

            long itemsCount = max - 1;
            List<long> lstInts = new List<long>();
            for (long i = 1; i <= itemsCount; i++)
                lstInts.Add(i);

            AssertConvertedList(
                Node<long>.ConvertToDblLinkedList(bst, parallel),
                lstInts);            
        }

        private void GenSubTree(Node<long> node, int level, bool leftNode, bool parallel)
        {
            if (level <= 0) return;

            long levelDelta = (long)Math.Pow(2, level - 1);
            long newValue = (leftNode ? node.Value - levelDelta : node.Value + levelDelta);
            Node<long> newNode = new Node<long> { Value = newValue };

            if (leftNode)
                node.Left = newNode;                
            else
                node.Right = newNode;

            if (parallel)
            {
                Parallel.Invoke(
                    () => { GenSubTree(newNode, level - 1, true, true); },
                    () => { GenSubTree(newNode, level - 1, false, true); });
            }
            else
            {
                GenSubTree(newNode, level - 1, true, false);
                GenSubTree(newNode, level - 1, false, false);
            }
        }

        private void AssertConvertedList<T>(Node<T> listNode, List<T> items)
        {
            // Walk forward and assert
            Node<T> trailNode = listNode;
            for (int i = 0; i < items.Count; i++)
            {
                Assert.AreEqual(listNode.Value, items[i], string.Format("Assert Failed on: {0}", listNode.Value));
                trailNode = listNode;
                listNode = listNode.Right;
            }
            Assert.AreEqual(listNode, null);

            // Walk backward and assert
            listNode = trailNode;
            for (int i = items.Count-1; i >= 0; i--)
            {
                Assert.AreEqual(listNode.Value, items[i], string.Format("Assert Failed on: {0}", listNode.Value));
                listNode = listNode.Left;
            }
            Assert.AreEqual(listNode, null);
            
        }
    }
}
