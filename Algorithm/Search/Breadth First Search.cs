using CommonClassLibary;
using System;
using System.Collections.Generic;

namespace Algorithm.Breadth_First_Search
{
    public class Breadth_First_Search : IExecute
    {
        public void Main()
        {
            // BFS
            // 從根結點開始，會先探索到最深的節點，在返回上一層節點繼續探索
            Console.WriteLine("廣度優先搜尋\n");

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

            Queue<Node> queue = new Queue<Node>();
            string result = string.Empty;
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                Node currentNode = queue.Dequeue();
                result += $"{currentNode._value} ";

                // 將當前 Node 的 childrenNode 寫入 Queue
                // Queue 是 FIFO 先寫入先處理
                foreach (Node child in currentNode.children)
                    queue.Enqueue(child);
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