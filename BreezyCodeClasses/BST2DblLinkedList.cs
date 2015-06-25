using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreezyCode.Classes.BST
{
    /// <summary>
    /// 
    /// - multithreaded CheckAccess, is that only for STA or for MTA as well ?
    /// - WPF Duplex Channel 2 way communication
    /// - How to detect and troubleshoot memory leak given memory dump file
    /// - Coding: Convert BST into Sorted Doubly Linked List
    /// 
    /// </summary>
    public class Node<T>
    {
        public Node<T> Left;
        public Node<T> Right;
        public T Value;

        /// <summary>
        /// Turn BST into Doubley-Linked list
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Node<T> ConvertToDblLinkedList(Node<T> p, bool runParallel = true)
        {
            if (p == null) return null;

            // Break down into 3 sub-problems:
            // 1) Convert left sub-tree into DblLinkedList
            // 2) Convert left right-tree into DblLinkedList
            // 3) Turn current node p from BST with Left and Right child pointers, into a DblLinkedList which 
            //      means its Left and Right BST pointers should become LinkedList pointers Back and Next 
            //      They should both be null'ed out at this point, because we are going to attach/join the 3 parts below.

            Node<T> leftSubList = null, rightSubList = null;
            if (runParallel)
            {
                Parallel.Invoke(
                    () => { leftSubList = ConvertToDblLinkedList(p.Left); },
                    () => { rightSubList = ConvertToDblLinkedList(p.Right); });
            }
            else
            {
                leftSubList = ConvertToDblLinkedList(p.Left);
                rightSubList = ConvertToDblLinkedList(p.Right);
            }
            p.Left = p.Right = null;

            // With above 3 sub-problems solved, we join the 3 together
            Node<T> newHead = Attach(leftSubList, p);
            Attach(p, rightSubList);

            return newHead;
        }

        private static Node<T> Attach(Node<T> list1, Node<T> list2)
        {
            if (list1 == null) return list2;
            if (list2 == null) return list1;

            Node<T> tailList1 = getTail(list1);

            // join the tailNode of list1 to headNode of list2
            tailList1.Right = list2;
            list2.Left = tailList1;

            return list1;
        }

        private static Node<T> getTail(Node<T> w)
        {
            Node<T> prev = null;
            while (w != null)
            {
                prev = w;
                w = w.Right;
            }

            return prev;
        }
    }
}
