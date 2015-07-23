using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreezyCode.Classes
{
    /// <summary>
    /// Problem Statement: 
    /// - Given an array of integers and another integer, determine if there is a set of 
    /// numbers in the array that will add up to that other integer.
    /// </summary>
    public class ArraySumCombo
    {
        public static bool HasCombo(int[] a, int k)
        {
            for (int j = 0; j < a.Length; j++)
            {
                int sumSoFar = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    sumSoFar += a[i];

                    if (HasCombo(a, k - sumSoFar, i + 1, sumSoFar))
                        return true;
                }

                // swap a[0] with a[j+1]
                if (j < a.Length)
                {
                    // BUG: this time of swapping arithmatic can result in overflow or underflow problem
                    a[j + 1] = a[0] + a[j + 1];
                    a[0] = a[j + 1]-a[0];
                    a[j + 1] -= a[0];
                }
            }

            return false;
        }

        public static bool HasCombo(int[] a, int k, int startIndex, int sumSoFar)
        {
            // check for terminating case
            if (startIndex == a.Length - 1)
                return (a[startIndex] == k);

            if (a[startIndex] == k - sumSoFar)
                return true;

            for (int i = startIndex; i < a.Length; i++)
            {
                sumSoFar += a[i];

                if (HasCombo(a, k - sumSoFar, i + 1, sumSoFar))
                    return true;
            }

            return false;
        }

    }
}
