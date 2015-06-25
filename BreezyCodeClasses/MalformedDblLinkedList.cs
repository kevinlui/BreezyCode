using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BreezyCode.Classes
{
    /// <summary>
    /// Consider a Doubly-Linked List, but with BackPtr on nodes that point at random node, 
    /// not necessarily at the previos node.
    /// </summary>
    public class MalformedDblLinkedList
    {
        public class Node<T>
        {
            public Node<T> Next = null;
            public Node<T> BackPtr = null;
            public T Value;
        }

        /// <summary>
        /// DeepCopy of the mal-formed Doubly-Linked List.
        /// Implementation uses a Dictionary to track corresponding Nodes.
        /// </summary>
        /// <param name="nodeSrc"></param>
        /// <returns></returns>
        public static Node<T> DeepCopy<T>(Node<T> nodeSrc)
        {
            if (null == nodeSrc)
                return null;

            // A map from src list to cloned list
            var dictSrcDest = new Dictionary<Node<T>, Node<T>>();

            Node<T> nodeOrig = nodeSrc, nodeHeadDest = null, nodeDestPrev = null;

            // First Pass to create the Singly-linked list
            while (null != nodeSrc)
            {
                Node<T> nodeNew = new Node<T> {Value = nodeSrc.Value};
                // keep track of the mapping
                dictSrcDest.Add(nodeSrc, nodeNew);

                if (nodeHeadDest == null)
                    nodeHeadDest = nodeNew;
                if (nodeDestPrev != null)
                    nodeDestPrev.Next = nodeNew;

                nodeDestPrev = nodeNew;
                nodeSrc = nodeSrc.Next;
            }

            // Second pass scan to hook up the Back/Random pointer, using Dictionary created above
            var nodeDestWalk = nodeHeadDest;
            nodeSrc = nodeOrig;
            while (null != nodeSrc)
            {
                // find the corresponding Node in cloned list
                if (nodeSrc.BackPtr != null)
                    nodeDestWalk.BackPtr = dictSrcDest[nodeSrc.BackPtr];

                nodeSrc = nodeSrc.Next;
                nodeDestWalk = nodeDestWalk.Next;
            }

            return nodeHeadDest;
        }
    }
}