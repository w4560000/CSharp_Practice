using LeetCode.Interface;
using System;

namespace LeetCode.Problems.Easy
{
    public class Topic28_Implement_strStr : IEntry
    {
        public void Main()
        {
            Console.WriteLine(StrStr_finalSolution("hello", "ll"));

           // Console.WriteLine(StrStr("mississippi", "mississippi"));
        }

        public int StrStr(string haystack, string needle)
        {
            if (needle == "") return 0;
            for (int i = 0; i < haystack.Length; i++)
            {
                int j = 0;
                while ((i + j) < haystack.Length && haystack[i + j] == needle[j])
                {
                    if (j == needle.Length - 1) return i;
                    j++;
                }
                if (i + j == haystack.Length) return -1;
            }
            return -1;
        }

        public int StrStr_antherSolution(string haystack, string needle)
        {

            if (needle.Length == 0) return 0;
            if (haystack.Length < needle.Length) return -1;

            var currentHaystackIndex = 0;
            var currentNeedleIndex = 0;
            while (currentHaystackIndex < haystack.Length
                 && currentNeedleIndex < needle.Length)
            {
                if (haystack[currentHaystackIndex] == needle[currentNeedleIndex])
                {
                    currentHaystackIndex++;
                    currentNeedleIndex++;
                }
                else
                {
                    currentHaystackIndex =
                        currentHaystackIndex - currentNeedleIndex + 1;
                    currentNeedleIndex = 0;
                }
            }
            return (currentNeedleIndex == needle.Length)
                   ? currentHaystackIndex - needle.Length
                   : -1;
        }

        public int StrStr_finalSolution(string haystack, string needle)
        {

            if (needle.Length == 0) return 0;
            if (haystack.Length < needle.Length) return -1;

            var currentHaystackIndex = 0;
            var currentNeedleIndex = 0;
            var next = GetNext(needle);
            while (currentHaystackIndex < haystack.Length
                 && currentNeedleIndex < needle.Length)
            {
                if (currentNeedleIndex == -1 ||
                   haystack[currentHaystackIndex] == needle[currentNeedleIndex])
                {
                    currentHaystackIndex++;
                    currentNeedleIndex++;
                }
                else
                {
                    currentNeedleIndex = next[currentNeedleIndex];
                }
            }
            return (currentNeedleIndex == needle.Length)
                   ? currentHaystackIndex - needle.Length
                   : -1;
        }

        private int[] GetNext(string needle)
        {
            var index = 0;
            var indexValue = -1;
            var next = new int[needle.Length];
            next[index] = indexValue;
            while (index < needle.Length - 1)
            {
                if (indexValue == -1
                   || needle[index] == needle[indexValue])
                {
                    index++;
                    indexValue++;
                    if (needle[index] != needle[indexValue])
                    {
                        next[index] = indexValue;
                    }
                    else
                    {
                        next[index] = next[indexValue];
                    }
                }
                else
                {
                    indexValue = next[indexValue];
                }
            }
            return next;
        }
    }
}