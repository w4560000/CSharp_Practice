using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace LeetCode.Problems
{
    /// <summary>
    /// 3. Longest Substring Without Repeating Characters
    /// 
    /// Given a string s, find the length of the longest substring without duplicate characters.
    /// 
    /// 給定一串文字，取得連續不重複字元的字數
    /// abcabcbb => 回傳 3
    /// </summary>
    public class Topic03_Longest_Substring_Without_Repeating_Characters : IEntry
    {
        public void Main()
        {
            var s = "abcabcbb";
            var s1 = "abba";
            Console.WriteLine(S2(s1));
        }

        /// <summary>
        /// 時間: O(n)、空間: O(n)
        /// 
        /// 目前是抓出不連續字元長度 s[left ... right]，所以長度 = right - left + 1
        /// 迴圈內判斷目的就是處理 left 數字
        /// 
        /// 建立一個 HashSet 暫存表，存入不連續文字字元
        /// 1. 跑迴圈判斷， 用 While 判斷 當暫存表內包含該迴圈字元，則刪除暫存表內第一筆字元，直到暫存表內無該迴圈字元、紀錄 left (左移視窗Index)
        /// 2. 暫存表內存入 當前迴圈字元
        /// 3. 判斷最大不連續字元數字 (判斷目前視窗有多長，連續字元長度)
        /// </summary>
        public int S1(string s)
        {
            int left = 0;
            int maxLength = 0;
            HashSet<char> charSet = new HashSet<char>();

            for (int right = 0; right < s.Length; right++)
            {
                while (charSet.Contains(s[right]))
                {
                    charSet.Remove(s[left]);
                    left++;
                }

                charSet.Add(s[right]);
                maxLength = Math.Max(maxLength, right - left + 1);
            }

            return maxLength;
        }

        /// <summary>
        /// 最佳解
        /// 時間: O(n)、空間: O(n)
        /// 
        /// 建立暫存表 Dictionary<字元、字元Index>
        /// 因Dictionary Key 無法重複，所以會存入該字元最後出現的Index
        /// 
        /// 1. 跑迴圈 (判斷暫存表內若有重複字元、該重複字元的Index、判斷重複字元在當前視窗內)
        /// 2. 暫存表內存入當前字元 Index
        /// 3. 取得視窗內長度
        /// 
        /// </summary>
        public int S2(string s)
        {
            int left = 0;
            int maxLength = 0;
            Dictionary<char, int> lastIndex = new Dictionary<char, int>();

            for (int right = 0; right < s.Length; right++)
            {
                char c = s[right];

                if (lastIndex.ContainsKey(c) && lastIndex[c] >= left)
                {
                    left = lastIndex[c] + 1; // 直接跳
                }

                lastIndex[c] = right; // 更新最後出現位置
                maxLength = Math.Max(maxLength, right - left + 1);
            }

            return maxLength;
        }
    }
}