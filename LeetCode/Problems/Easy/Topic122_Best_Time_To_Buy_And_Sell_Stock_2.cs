using LeetCode.Interface;
using System;
using System.Diagnostics;
using System.Text;

namespace LeetCode.Problems.Easy
{
    /// <summary>
    /// Kadane’s Algorithm
    ///
    /// 122. Best Time to Buy and Sell Stock II
    /// You are given an integer array prices where prices[i] is the price of a given stock on the ith day.
    /// On each day, you may decide to buy and/or sell the stock. You can only hold at most one share of the stock at any time.
    /// However, you can sell and buy the stock multiple times on the same day, ensuring you never hold more than one share of the stock.
    /// Find and return the maximum profit you can achieve.
    /// </summary>
    public class Topic122_Best_Time_To_Buy_And_Sell_Stock_2 : IEntry
    {
        public void Main()
        {
            //Console.WriteLine(S1(new int[] { 7, 1, 5, 3, 6, 4 }));
            Console.WriteLine(S1(new int[] { 1, 10, 100 }));
        }

        public int S1(int[] prices)
        {
            int profit = 0;

            for (int i = 1; i < prices.Length; i++)
            {
                if (prices[i] > prices[i - 1])
                    profit += prices[i] - prices[i - 1];
            }

            return profit;
        }
    }
}