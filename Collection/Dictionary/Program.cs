using System;
using System.Collections.Generic;

namespace Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> d1 = new Dictionary<string, string>();
            d1.Add("key1", "value1");

            Console.WriteLine(d1["key1"]);
            Console.ReadLine();
        }
    }
}
