using LeetCode.Interface;
using System;

namespace LeetCode.Problems.Easy
{
    public class Topic35_Search_Insert_Position : IEntry
    {
        public void Main()
        {
            Console.WriteLine(SearchInsert_BinarySearchV1(new int[] { 1, 3, 5, 6 }, 2));
        }

        // 土炮解法
        public int SearchInsert(int[] nums, int target)
        {
            int i = 0;
            while (i < nums.Length)
            {
                if (nums[i] == target)
                    return i;
                else if (i == 0 && (nums.Length == 1 || nums[i + 1] > target))
                    return nums[i] < target ? i + 1 : i;
                else if (i != 0 && i != nums.Length - 1 && nums[i] < target && target < nums[i + 1])
                    return i + 1;
                else
                    i += 1;
            }

            return i;
        }

        // 二分搜尋 限定有排序的結構
        public int SearchInsert_BinarySearch(int[] nums, int target)
        {
            var low = 0;
            var high = nums.Length - 1;
            int mid;
            while (low <= high)
            {
                mid = (low + high) / 2;
                if (target < nums[mid])
                {
                    high = mid - 1;
                }
                else if (target > nums[mid])
                {
                    low = mid + 1;
                }
                else
                {
                    return mid;
                }
            }
            return low;
        }

        // 二分搜尋  recursion
        public int SearchInsert_BinarySearchV1(int[] nums, int target)
        {
            static int BinarySearch(int[] arr, int low, int high, int key)
            {
                int mid = (low + high) / 2;
                if (low > high)
                    return high + 1 ;
                else
                {
                    if (arr[mid] == key)
                        return mid;
                    else if (arr[mid] > key)
                        return BinarySearch(arr, low, mid - 1, key);
                    else if (arr[mid] < key)
                        return BinarySearch(arr, mid + 1, high, key);
                    else
                        return mid;
                }
            }

            return BinarySearch(nums, 0, nums.Length - 1, target);
        }
    }
}