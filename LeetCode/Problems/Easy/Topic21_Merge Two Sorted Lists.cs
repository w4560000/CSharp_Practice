using LeetCode.Interface;
using System;
using System.Collections.Generic;

namespace LeetCode.Problems.Easy
{
    public class Topic21_Merge_Two_Sorted_Lists : IEntry
    {
        public void Main()
        {
            ListNode a = new ListNode(1, new ListNode(2, new ListNode(4)));
            ListNode b = new ListNode(1, new ListNode(3, new ListNode(4)));

            Console.WriteLine(MergeTwoLists_Iteration(a, b));
        }

        // 自己土炮解法 96 ms	 26.1 MB
        public ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            List<int> temp = new List<int>();

            if (l1 != null)
            {
                temp.Add(l1.val);
                while (l1.next != null)
                {
                    l1 = l1.next;
                    temp.Add(l1.val);
                }
            }

            if (l2 != null)
            {
                temp.Add(l2.val);
                while (l2.next != null)
                {
                    l2 = l2.next;
                    temp.Add(l2.val);
                }
            }
            temp.Sort();

            static ListNode GetListNode(List<int> list)
            {
                if (list.Count == 0)
                    return null;
                else
                {
                    int value = list[0];
                    list.RemoveAt(0);
                    return new ListNode(value, GetListNode(list));
                }
            }

            return GetListNode(temp);
        }

        // 跌代解法 92 ms 5.7 MB
        public ListNode MergeTwoLists_Iteration(ListNode l1, ListNode l2)
        {
            ListNode head = new ListNode();
            ListNode prev = head;

            while (l1 != null && l2 != null)
            {
                if(l1.val > l2.val)
                {
                    prev.next = l2;
                    l2 = l2.next;
                } else
                {
                    prev.next = l1;
                    l1 = l1.next;
                }

                prev = prev.next;
            }

            prev.next = l1 ?? l2;

            return head.next;
        }

        // 遞迴解法 96 ms 25.7 MB
        public ListNode MergeTwoLists_Recursion(ListNode l1, ListNode l2)
        {
            if (l1 == null)
                return l2;
            else if (l2 == null)
                return l1;
            else if(l1.val < l2.val)
            {
                l1.next = MergeTwoLists_Recursion(l1.next, l2);
                return l1;
            }
            else
            {
                l2.next = MergeTwoLists_Recursion(l1, l2.next);
                return l2;
            }
        }
    }

    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }
}