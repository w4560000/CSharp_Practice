using LeetCode.Interface;
using System;
using System.Collections.Generic;

namespace LeetCode.Problems.Easy
{
    public class Topic20_Valid_Parentheses : IEntry
    {
        public void Main()
        {
            Console.WriteLine(IsValid_Replace("([)"));
        }

        // stack解法 time complexity = O(n)
        public bool IsValid(string s)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in s)
            {
                if (stack.Count == 0)
                    stack.Push(c);
                else if (stack.Peek() == '{' && c == '}' || stack.Peek() == '[' && c == ']' || stack.Peek() == '(' && c == ')')
                    stack.Pop();
                else
                    stack.Push(c);
            }

            return stack.Count == 0;
        }

        // 雖然簡潔 但效率較差
        // Contains O(n) 、 Replace O(n) 、 執行while O(n)  time complexity = 3 * O(n)
        public bool IsValid_Replace(string s)
        {
           while(s.Contains("()") || s.Contains("[]") || s.Contains("{}"))
                s = s.Replace("()", "").Replace("[]", "").Replace("{}", "");

            return s.Length == 0;
        }
    }
}