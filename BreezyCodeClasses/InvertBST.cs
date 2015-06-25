using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BreezyCode.Classes.BST;

// C# (C-Sharp)

namespace BreezyCode.Classes
{
    /// <summary>
    /// 
    ///  Invert a BST:
    /// 
    ///       4
    ///     /   \
    ///    2     7
    ///   / \   / \
    ///  1   3 6   9
    /// 
    ///      To:
    /// 
    ///       4
    ///     /   \
    ///    7     2
    ///   / \   / \
    ///  9   6 3   1
    /// 
    /// </summary>
    public class InvertBST
    {
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;

            public TreeNode(int x)
            {
                val = x;
            }
        }

        public static TreeNode InvertTree(TreeNode root)
        {
            if (root != null)
            {
                var cacheLeft = root.left;
                var cacheRight = root.right;

                root.left = cacheRight;
                root.right = cacheLeft;

                if (cacheLeft != null)
                    InvertTree(cacheLeft);

                if (cacheRight != null)
                    InvertTree(cacheRight);                
            }

            return root;
        }
    }

}
