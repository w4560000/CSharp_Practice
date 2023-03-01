using CommonClassLibary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Selection_Sort
{
    public class Selection_Sort : IExecute
    {
        public void Main()
        {
            Console.WriteLine("選擇排序\n");
            List<int> list = new List<int>() { 2, 3, 1, 6, 2, 9, 4, 1, 7 };
            Console.WriteLine($"排序前數列:      {string.Join(",", list)}");

            SelectionSort(list);

            Console.WriteLine($"排序後數列:      {string.Join(",", list)}");
        }

        /// <summary>
        /// 選擇排序: 將資料分成已排序、未排序兩部分，由已排序最後一筆與未排序資料相比，若已排序最後一位 > 未排序的某筆資料 則兩者交換
        /// 
        /// 時間複雜度 Ο(n2)
        /// </summary>
        public static void SelectionSort(List<int> list)
        {
            int index = 1;
            int runtime = 1;

            int minIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                minIndex = i;
                for (int j = i + 1; j < list.Count; j++)//執行的次數
                {
                    if (list[minIndex] > list[j])
                    {
                        minIndex = j;
                    }
                    runtime++;
                }

                if(minIndex != i)
                {
                    #region 交換前 Log
                    Console.Write($"\n第{index:D2}次交換前, 數列:");

                    foreach (var item in list.Select((value, z) => new { value, z }))
                    {
                        if (item.z == i)
                            CConsole.Write($"{item.value:red} ");
                        else if (item.z == minIndex)
                            CConsole.Write($"{item.value:green} ");
                        else
                            Console.Write($"{item.value} ");
                    }
                    Console.WriteLine();
                    #endregion

                    //二數交換
                    int temp = list[minIndex];
                    list[minIndex] = list[i];
                    list[i] = temp;

                    #region 交換後 Log
                    Console.Write($"第{index:D2}次交換後, 數列:");

                    foreach (var item in list.Select((value, z) => new { value, z }))
                    {
                        if (item.z == minIndex)
                            CConsole.Write($"{item.value:red} ");
                        else if (item.z == i)
                            CConsole.Write($"{item.value:green} ");
                        else
                            Console.Write($"{item.value} ");
                    }
                    Console.WriteLine();
                    index++;
                    #endregion

                }
            }

            Console.WriteLine($"\n實際執行次數:{runtime}\n");
        }
    }
}