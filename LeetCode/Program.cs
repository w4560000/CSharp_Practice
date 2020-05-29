using LeetCode.Interface;
using System;
using System.Linq;
using System.Reflection;

namespace LeetCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("輸入 Leetcode 題號:");
            string number = Console.ReadLine();

            var entryMethod = Assembly.GetExecutingAssembly()
                    .GetExportedTypes()
                    .Where(x => typeof(IEntry).IsAssignableFrom(x) && !x.IsInterface && x.Name.StartsWith("Topic" + number))
                    .Select(y => (IEntry)Activator.CreateInstance(y))
                    .FirstOrDefault();

            if (entryMethod != null)
                entryMethod.Main();
            else
                Console.WriteLine("無此題號");

            Console.ReadKey();
        }
    }
}