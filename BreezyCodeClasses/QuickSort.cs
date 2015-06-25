using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BreezyCode.Classes
{
    /// <summary>
    /// Investigate using TaskParallelLibrary to speed up QuickSort
    /// </summary>
    public class QuickSort
    {
        // we want to chunck parallelims for every 1000ms worth of work
        private static readonly long SmartParallelThresoldMS = 4000;

        public static void QuickSortSequentialClassic(int[] array, int lo, int hi)
        {
            if (lo < hi)
            {
                int midIndex = Partition(array, lo, hi);

                if (lo < midIndex)
                    QuickSortSequentialClassic(array, lo, midIndex);
                if (midIndex+1 < hi)
                    QuickSortSequentialClassic(array, midIndex + 1, hi);
            }
        }

        public static void QuickSortParallelAlways(int[] array, int lo, int hi)
        {
            if (lo < hi)
            {
                int midIndex = Partition(array, lo, hi);

                Parallel.Invoke(
                    () => {
                        if (lo < midIndex) 
                            QuickSortParallelAlways(array, lo, midIndex);
                    },
                    () => {
                        if (midIndex + 1 < hi) 
                            QuickSortParallelAlways(array, midIndex + 1, hi);
                    });
            }
        }


        public static void QuickSortParallelTimeChunks(int[] array, int lo, int hi, long msStart = 0)
        {
            if (lo < hi)
            {
                int midIndex = Partition(array, lo, hi);

                long msNow = DateTime.Now.Ticks;

                // Check if contiune in Sequential or Parallel, if time elapsed long enough
                if ((msNow - msStart) / 10000 > SmartParallelThresoldMS)
                {
                    // Either first case, or we had been in Sequential mode long enough: invoke a Parallel !
                    Parallel.Invoke(
                        () =>
                        {
                            if (lo < midIndex)
                                QuickSortParallelTimeChunks(array, lo, midIndex, msNow);
                        },
                        () =>
                        {
                            if (midIndex + 1 < hi)
                                QuickSortParallelTimeChunks(array, midIndex + 1, hi, msNow);
                        });
                }
                else
                {
                    // Continue in Sequential, and pass on the ongoing msStart
                    if (lo < midIndex)
                        QuickSortParallelTimeChunks(array, lo, midIndex, msStart);
                    if (midIndex + 1 < hi)
                        QuickSortParallelTimeChunks(array, midIndex + 1, hi, msStart);
                }
            }
        }

        public static void QuickSortParallelProcessorCount(int[] array, int lo, int hi, int parallelCount = -99)
        {
            if (lo < hi)
            {
                int midIndex = Partition(array, lo, hi);

                // initial case, parallelism # depends on Environment.ProcessorCount
                if (parallelCount == -99)
                    parallelCount = Environment.ProcessorCount;

                // Check if contiune to Parallel or fallback to Sequential, if used up Processors
                if (parallelCount >= 1)
                {
                    // Not sure if this should -2 given the Parallel.Invoke below spawns 2 tasks
                    parallelCount -= 1;

                    Parallel.Invoke( 
                    () =>
                    {
                        if (lo < midIndex)
                            QuickSortParallelProcessorCount(array, lo, midIndex, parallelCount);
                    },
                    () =>
                    {
                        if (midIndex + 1 < hi)
                            QuickSortParallelProcessorCount(array, midIndex + 1, hi, parallelCount);
                    });
                }
                else
                {
                    // Continue in Sequential
                    if (lo < midIndex)
                        QuickSortParallelProcessorCount(array, lo, midIndex, parallelCount);
                    if (midIndex + 1 < hi)
                        QuickSortParallelProcessorCount(array, midIndex + 1, hi, parallelCount);
                }
            }
        }

        static int Partition(int[] array, int lo, int hi)
        {
            int pivotIndex = (lo + (hi - lo) / 2);
            int pivotValue = array[pivotIndex];
            // put the chosen pivot at array[hi]
            SwapElements(array, pivotIndex, hi);
            int storeIndex = lo;
            // Compare remaining array elements against pivotValue = array[hi]
            for (int i = lo; i < hi; i++)
            {
                if (array[i] < pivotValue)
                {
                    if (i != storeIndex)
                        SwapElements(array, i, storeIndex);
                    ++storeIndex;
                }
            }
            SwapElements(array, storeIndex, hi);  // Move pivot to its final place

            return storeIndex;
        }

        static void SwapElements(int[] array, int index1, int index2)
        {
            if (index1 != index2)
            {
                int temp = array[index2];
                array[index2] = array[index1];
                array[index1] = temp;
            }
        }
    }
}
