using CommonClassLibary;
using System;
using System.Collections.Generic;

namespace Algorithm.Bubble_Sort
{
    public class Bubble_Sort : IExecute
    {
        public void Main()
        {
            List<int> list = new List<int>() { 2, 3, 1, 6, 2, 9, 4, 1, 7 };
            Console.WriteLine("排序前數列");
            list.ForEach(x => Console.Write(x));

            Console.WriteLine();
            BubbleSort(list);
            Console.WriteLine();

            Console.WriteLine("排序後數列");
            list.ForEach(x => Console.Write(x));
        }

        /// <summary>
        /// 泡沫排序: 比較 n 個數字
        /// 
        /// 不斷向右比較相鄰兩個數字，若後面較小則兩者交換
        /// 第一回合會把最大數字排列到最後面，比較次數: n - 1 
        /// 第二回合把次大數字排到倒數第二位，比較次數: n - 2
        /// .. 以此類推
        /// 
        /// 總比較回合數: n - 1
        /// </summary>
        /// <param name="list"></param>
        public static void BubbleSort(List<int> list)
        {
            int len = list.Count;
            for (int i = len - 1; i > 0; i--)//執行的回數
                for (int j = 0; j < i; j++)//執行的次數
                {
                    if (list[j] > list[j + 1])
                    {
                        //二數交換
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
        }
    }
}