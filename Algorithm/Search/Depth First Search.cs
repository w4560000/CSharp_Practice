using CommonClassLibary;
using System;
using System.Collections.Generic;

namespace Algorithm.Depth_First_Search
{
    public class Depth_First_Search : IExecute
    {
        public void Main()
        {
            // DFS
            // 從根結點開始，按照層級一層一層遍歷所有節點
            Console.WriteLine("深度優先搜尋\n");

            var root = new Node("A")
            {
                children = new List<Node>()
                {
                   new Node("E")
                   {
                       children = new List<Node>()
                       {
                           new Node("G"),
                           new Node("W"),
                           new Node("X")
                       }
                   },
                   new Node("Q")
                   {
                       children = new List<Node>()
                       {
                           new Node("I"),
                           new Node("W"),
                           new Node("R")
                       }
                   }
                }
            };

            Stack<Node> stack = new Stack<Node>();
            string result = string.Empty;
            stack.Push(root);

            while (stack.Count != 0)
            {
                Node currentNode = stack.Pop();
                result += $"{currentNode._value} ";

                // 將當前 Node 的 childrenNode 寫入 Queue
                // Stack 是 FILO 先寫入後處理
                // 將子節點以相反的順序推入堆疊 (因要先寫入較遠的節點，之後在處理)
                for (int i = currentNode.children.Count - 1; i >= 0; i--)
                    stack.Push(currentNode.children[i]);
            }

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }

    public class Node
    {
        public string _value;
        public List<Node> children = new List<Node>();

        public Node(string value)
        {
            _value = value;
        }
    }
}