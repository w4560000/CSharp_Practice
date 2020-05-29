using LeetCode.Interface;
using System;

namespace LeetCode.Problems.Easy
{
    public class Topic27_Remove_Element : IEntry
    {
        public void Main()
        {
            Console.WriteLine(RemoveElement(new int[] { 0, 1, 2, 2, 3, 0, 4, 2 }, 2));
        }

        public int RemoveElement(int[] nums, int val)
        {
            int i = 0;
            for (int j = 0; j < nums.Length; j++)
            {
                if (nums[j] != val)
                {
                    nums[i] = nums[j];
                    i++;
                }
            }
            return i;
        }
    }
}