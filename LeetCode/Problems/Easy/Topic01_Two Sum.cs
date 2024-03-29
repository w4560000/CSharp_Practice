﻿using LeetCode.Interface;
using System;
using System.Collections.Generic;

namespace LeetCode.Problems
{
    /// <summary>
    /// https://leetcode.com/problems/two-sum/
    /// </summary>
    public class Topic01_Two_Sum : IEntry
    {
        public void Main()
        {
            var nums = new int[] { 230, 863, 916, 585, 981, 404, 316, 785, 88, 12, 70, 435, 384, 778, 887, 755, 740, 337, 86, 92, 325, 422, 815, 650, 920, 125, 277, 336, 221, 847, 168, 23, 677, 61, 400, 136, 874, 363, 394, 199, 863, 997, 794, 587, 124, 321, 212, 957, 764, 173, 314, 422, 927, 783, 930, 282, 306, 506, 44, 926, 691, 568, 68, 730, 933, 737, 531, 180, 414, 751, 28, 546, 60, 371, 493, 370, 527, 387, 43, 541, 13, 457, 328, 227, 652, 365, 430, 803, 59, 858, 538, 427, 583, 368, 375, 173, 809, 896, 370, 789 };
            var result1 = TwoSumV1(nums, 542);
            var result2 = TwoSumV2(nums, 542);
            Console.WriteLine(string.Join(',', result1));
            Console.WriteLine(string.Join(',', result2));
        }

        public int[] TwoSumV1(int[] nums, int target)
        {
            Dictionary<int, int> temp = new Dictionary<int, int>();

            for (int i = 0; i < nums.Length; i++)
            {
                int number = target - nums[i];

                if (temp.ContainsKey(number))
                    return new int[] { temp[number], i };

                temp[nums[i]] = i;
            }

            throw new ArgumentNullException();
        }

        public int[] TwoSumV2(int[] nums, int target)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 1; j < nums.Length; j++)
                {
                    if (i != j && nums[i] + nums[j] == target)
                    {
                        return new int[] { i, j };
                    }
                }
            }

            throw new Exception();
        }
    }
}