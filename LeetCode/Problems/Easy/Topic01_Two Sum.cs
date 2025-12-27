using LeetCode.Interface;
using System;
using System.Collections.Generic;

namespace LeetCode.Problems
{
    /// <summary>
    /// 1. Two Sum
    /// 
    /// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
    /// You may assume that each input would have exactly one solution, and you may not use the same element twice.
    /// You can return the answer in any order.
    /// 
    /// 傳入一串數字陣列，給定一個加總數字
    /// 該加總數字為兩個數字總和，回傳這兩個數字的陣列位子
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

        /// <summary>
        /// 最佳解
        /// 時間: O(n)、空間: O(n)
        /// 
        /// 建立暫存表temp <數字, 陣列Index>
        /// 1. 跑迴圈，用總和數字 - 當前迴圈數字A 得 差值 B
        /// 2. 比對暫存表內是否有差值B (先之前迴圈會把迴圈數字存入暫存表，此時當前迴圈 若總和數字-當前迴圈數字 有符合之前迴圈的數字，則代表這兩個陣列數字總和 = 題目總和)
        /// 若有 代表吻合數字 直接回傳 差值B的 陣列Index、當前陣列Index
        /// 3. 將當前迴圈數字A 存入暫存表紀錄
        /// </summary>
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

        /// <summary>
        /// 暴力解
        /// 時間: O(n平方)、空間: O(1)
        /// 
        /// 單純跑雙層迴圈，一筆一筆相加做比對，比對到總和數字就回傳
        /// </summary>
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