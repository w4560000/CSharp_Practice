using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCode.Problems.Easy
{
    public class Topic26_Remove_Duplicates_from_Sorted_Array : IEntry
    {
        public void Main()
        {
            Console.WriteLine(RemoveDuplicates(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }));
        }

        public int RemoveDuplicates(int[] nums)
        {
            if (nums.Length == 0) return 0;
            int i = 0;
            for (int j = 1; j < nums.Length; j++)
            {
                if (nums[j] != nums[i])
                {
                    i++;
                    nums[i] = nums[j];
                }
            }
            return i + 1;
        }
    }
}
