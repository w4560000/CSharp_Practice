using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace LeetCode.Problems.Easy
{
    /// <summary>
    /// Kadane’s Algorithm
    ///
    /// 53. Maximum Subarray
    /// Given an integer array nums, find the subarray with the largest sum, and return its sum.
    /// </summary>
    public class Topic53_Maximum_Subarray : IEntry
    {
        public void Main()
        {
            Console.WriteLine(S1(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }));
            //Console.WriteLine(S1(new int[] { 1, 2 }));
            //Console.WriteLine(S1(new int[] { -2, 1 }));
        }

        public int S1(int[] nums)
        {
            int res = nums[0];
            int total = 0;

            foreach (int n in nums)
            {
                if (total < 0)
                {
                    total = 0;
                }

                total += n;
                res = Math.Max(res, total);
            }

            return res;
        }
    }
}