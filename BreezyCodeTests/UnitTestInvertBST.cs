using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BreezyCode.Classes.BST;
using BreezyCode.Classes;

namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestInvertBST
    {
        [TestMethod]
        public void TestInvertBST()
        {
            //       4
            //     /   \
            //    2     7
            //   / \   / \
            //  1   3 6   9

            //       4
            //     /   \
            //    7     2
            //   / \   / \
            //  9   6 3   1

            InvertBST.TreeNode bst = new InvertBST.TreeNode(4)
            {
                left = new InvertBST.TreeNode(2)
                {
                    left = new InvertBST.TreeNode(1),
                    right = new InvertBST.TreeNode(3)
                },
                right = new InvertBST.TreeNode(7)
                {
                    left = new InvertBST.TreeNode(6),
                    right = new InvertBST.TreeNode(9)
                }
            };

            InvertBST.InvertTree(bst);

        }
    }
}
