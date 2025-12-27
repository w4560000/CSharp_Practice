using LeetCode.Interface;
using System;

namespace LeetCode.Problems.Easy
{
    /// <summary>
    /// Kadane’s Algorithm
    ///
    /// 121. Best Time to Buy and Sell Stock
    /// You are given an array prices where prices[i] is the price of a given stock on the ith day.
    /// You want to maximize your profit by choosing a single day to buy one stock and choosing a different day in the future to sell that stock.
    /// Return the maximum profit you can achieve from this transaction.If you cannot achieve any profit, return 0.
    /// </summary>
    public class Topic121_Best_Time_To_Buy_And_Sell_Stock : IEntry
    {
        public void Main()
        {
            Console.WriteLine(S1(new int[] { 7, 1, 5, 3, 6, 4 }));
            //Console.WriteLine(S1(new int[] { 7, 6, 4, 3, 1 }));
        }

        public int S1(int[] nums)
        {
            int buyPrice = nums[0];
            int profit = 0;

            for (int i = 1; i < nums.Length; i++)
            {
                if (buyPrice > nums[i])
                    buyPrice = nums[i];

                profit = Math.Max(profit, nums[i] - buyPrice);
            }

            return profit;
        }
    }
}