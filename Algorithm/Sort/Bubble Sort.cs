using CommonClassLibary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Bubble_Sort
{
    public class Bubble_Sort : IExecute
    {
        public void Main()
        {
            Console.WriteLine("泡沫排序\n");
            List<int> list = new List<int>() { 2, 3, 1, 6, 2, 9, 4, 1, 7 };
            //List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Console.WriteLine($"排序前數列:{string.Join(",", list)}");

            BubbleSortV2(list);

            Console.WriteLine($"\n排序後數列:{string.Join(",", list)}");
        }

        /// <summary>
        /// 泡沫排序: 從頭開始逐一比較相鄰兩筆資料，將較大值往後移動位置，直到最後，資料就會從最小值依序排到最大值
        ///
        /// 不斷向右比較相鄰兩個數字，若後面較小則兩者交換
        /// 第一回合會把最大數字排列到最後面，比較次數: n - 1
        /// 第二回合把次大數字排到倒數第二位，比較次數: n - 2
        /// .. 以此類推
        ///
        /// 若該回合已排列好 則結束
        /// 
        /// 時間複雜度 Ο(n2)
        /// </summary>
        public static void BubbleSortV1(List<int> list)
        {
            int index = 1;
            int runtime = 1;

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - 1 - i; j++)//執行的次數
                {
                    if (list[j] > list[j + 1])
                    {
                        #region 交換前 Log
                        Console.Write($"\n第{index:D2}次交換前, 數列:");

                        foreach (var item in list.Select((value, z) => new { value, z }))
                        {
                            if (item.z == j)
                                CConsole.Write($"{item.value:red} ");
                            else if (item.z == j + 1)
                                CConsole.Write($"{item.value:green} ");
                            else
                                Console.Write($"{item.value} ");
                        }
                        Console.WriteLine();
                        #endregion

                        //二數交換
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        #region 交換後 Log
                        Console.Write($"第{index:D2}次交換後, 數列:");

                        foreach (var item in list.Select((value, z) => new { value, z }))
                        {
                            if (item.z == j + 1)
                                CConsole.Write($"{item.value:red} ");
                            else if (item.z == j)
                                CConsole.Write($"{item.value:green} ");
                            else
                                Console.Write($"{item.value} ");
                        }
                        Console.WriteLine();
                        index++;
                        #endregion
                    }
                    runtime++;
                }
            }

            Console.WriteLine($"\n實際執行次數:{runtime}\n");
        }

        /// <summary>
        /// 泡沫排序法優化
        /// 加入一個flag做判斷，當該回合資料都沒有交換，代表已排序完提前結束
        /// 
        /// 最佳時間複雜度 Ο(n)
        /// 時間複雜度 Ο(n2)
        /// </summary>
        public static void BubbleSortV2(List<int> list)
        {
            int index = 1;
            int runtime = 1;

            var flag = true;
            for (int i = 0; i < list.Count - 1 && flag; i++)
            {
                flag = false;
                for (int j = 0; j < list.Count - 1 - i; j++)//執行的次數
                {
                    if (list[j] > list[j + 1])
                    {
                        #region 交換前 Log
                        Console.Write($"\n第{index:D2}次交換前, 數列:");

                        foreach (var item in list.Select((value, z) => new { value, z }))
                        {
                            if (item.z == j)
                                CConsole.Write($"{item.value:red} ");
                            else if (item.z == j + 1)
                                CConsole.Write($"{item.value:green} ");
                            else
                                Console.Write($"{item.value} ");
                        }
                        Console.WriteLine();
                        #endregion

                        //二數交換
                        int temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        flag = true;
                        #region 交換後 Log
                        Console.Write($"第{index:D2}次交換後, 數列:");

                        foreach (var item in list.Select((value, z) => new { value, z }))
                        {
                            if (item.z == j + 1)
                                CConsole.Write($"{item.value:red} ");
                            else if (item.z == j)
                                CConsole.Write($"{item.value:green} ");
                            else
                                Console.Write($"{item.value} ");
                        }
                        Console.WriteLine();
                        index++;
                        #endregion
                    }
                    runtime++;
                }
            }

            Console.WriteLine($"\n實際執行次數:{runtime}\n");
        }
    }
}