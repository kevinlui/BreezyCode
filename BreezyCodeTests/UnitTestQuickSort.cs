using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BreezyCode.Classes;


namespace BreezyCode.Tests
{
    [TestClass]
    public class UnitTestQuickSort
    {
        private static readonly int _testCases = 4;
        private static readonly int _rangeSize = 40000000; // 70000000;
        private static List<int[]> _arrays;
        private static int[] _arraySysSorted;

        [ClassInitialize]
        public static void Initialize(TestContext ctx)
        {
            _arrays = new List<int[]>(_testCases);
            for (int i = 0; i < _testCases; i++)
                _arrays.Add(new int[_rangeSize]);

            // populate the arrays
            Random rnd = new Random();
            for (int i = 0; i < _rangeSize; i++)
            {
                int r = rnd.Next(_rangeSize);
                for (int tc = 0; tc < _testCases; tc++)
                    _arrays[tc][i] = r;
            }

            // copy array, then sort it using system's Array.Sort(), prepare it for comparision
            _arraySysSorted = new int[_rangeSize];

            _arrays[0].CopyTo(_arraySysSorted, 0);
            Array.Sort(_arraySysSorted);
        }

        [TestMethod]
        public void TestQuickSort_SequentialClassic()
        {
            TestQuickSort(_arrays[0], _arraySysSorted,
                (a) => QuickSort.QuickSortSequentialClassic(a, 0, a.Length - 1));
        }

        [TestMethod]
        public void TestQuickSort_ParallelAlways()
        {
            TestQuickSort(_arrays[1], _arraySysSorted,
                (a) => QuickSort.QuickSortParallelAlways(a, 0, a.Length - 1));
        }

        [TestMethod]
        public void TestQuickSort_ParallelTimeChunks()
        {
            TestQuickSort(_arrays[2], _arraySysSorted,
                (a) => QuickSort.QuickSortParallelTimeChunks(a, 0, a.Length - 1));
        }

        [TestMethod]
        public void TestQuickSort_ParallelProcessorCount()
        {
            TestQuickSort(_arrays[3], _arraySysSorted,
                (a) => QuickSort.QuickSortParallelProcessorCount(a, 0, a.Length - 1));
        }

        void TestQuickSort(int[] array, int[] arrayCompare, Action<int[]> action)
        {
            action(array);

            AssertEqualArrays(array, arrayCompare);            
        }

        void AssertEqualArrays<T>(T[] array1, T[] array2) where T: IComparable<T>
        {
            if (array1.Length != array2.Length)
                throw new Exception("Arrays of unequal length");

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].CompareTo(array2[i]) != 0)
                    throw new Exception(string.Format("Arrays differ in value at index {0}: {1} != {2}", i, array1[i], array2[i]));
            }
            
        }
    }

}
