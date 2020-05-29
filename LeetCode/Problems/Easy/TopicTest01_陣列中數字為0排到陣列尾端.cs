using LeetCode.Interface;
using System;
using System.Linq;

namespace LeetCode.Problems
{
    /// <summary>
    /// 將陣列中數字為0往後排 而不為0的 依照陣列順序往前排
    /// </summary>
    public class TopicTest01_Test_陣列中數字為0排到陣列尾端 : IEntry
    {
        public void Main()
        {
            int[] arr = new int[] { 9, 0, 0, 3, 0, 0, 0, 1, 0, 4 };
            SolutionV1(arr);
            Console.WriteLine(arr);
        }

        /// <summary>
        /// 先撈出第一個為0的index，再撈出該index後面不為0的值
        /// 再來跑迴圈 將不為0的數值 往第一個不為0的index依序排入
        /// 排完非0數值後 其餘全塞入0
        /// 
        /// 交換順序如下:
        /// 原始陣列
        /// 9, 0, 0, 3, 0, 0, 0, 1, 0, 4
        /// 
        /// 9, 3, 0, 3, 0, 0, 0, 1, 0, 4    forindex = 1
        /// 9, 3, 1, 3, 0, 0, 0, 1, 0, 4    forindex = 2
        /// 9, 3, 1, 4, 0, 0, 0, 1, 0, 4    forindex = 3
        /// 9, 3, 1, 4, 0, 0, 0, 1, 0, 4    forindex = 4
        /// 9, 3, 1, 4, 0, 0, 0, 1, 0, 4    forindex = 5
        /// 9, 3, 1, 4, 0, 0, 0, 1, 0, 4    forindex = 6
        /// 9, 3, 1, 4, 0, 0, 0, 0, 0, 4    forindex = 7
        /// 9, 3, 1, 4, 0, 0, 0, 0, 0, 4    forindex = 8
        /// 9, 3, 1, 4, 0, 0, 0, 0, 0, 0    forindex = 9
        /// </summary>
        /// <param name="arr"></param>
        public void SolutionV1(int[] arr)
        {
            int firstZero = Array.FindIndex(arr, x => x == 0);
            int[] changeList = Enumerable.Range(firstZero, arr.Length - firstZero)
                                             .Where(i => arr[i] != 0)
                                             .Select(s => arr[s])
                                             .ToArray();

            int temp = 0;
            foreach (int index in Enumerable.Range(firstZero, arr.Length - firstZero))
            {
                if (index < firstZero + changeList.Length)
                {
                    arr[index] = changeList[temp];
                    temp += 1;
                }
                else
                    arr[index] = 0;
            }
        }

        /// <summary>
        /// 跑迴圈，先找出陣列數值為0的index 當作待會要交換的index = closed_index
        /// 當找到 closed_index之後 非0的index後 進行交換 而closed_index則加一
        /// 
        /// 交換順序如下:
        /// 原始陣列
        /// 9, 0, 0, 3, 0, 0, 0, 1, 0, 4
        /// 
        /// 9, 0, 0, 3, 0, 0, 0, 1, 0, 4    forindex = 0
        /// 9, 0, 0, 3, 0, 0, 0, 1, 0, 4    forindex = 1 closed_index = 1
        /// 9, 0, 0, 3, 0, 0, 0, 1, 0, 4    forindex = 2 closed_index = 1
        /// 9, 3, 0, 2, 0, 0, 0, 1, 0, 4    forindex = 3 closed_index = 2
        /// 9, 3, 2, 0, 0, 0, 0, 1, 0, 4    forindex = 4 closed_index = 3
        /// 9, 3, 2, 0, 0, 0, 0, 1, 0, 4    forindex = 5 closed_index = 3
        /// 9, 3, 2, 0, 0, 0, 0, 1, 0, 4    forindex = 6 closed_index = 3
        /// 9, 3, 2, 1, 0, 0, 0, 0, 0, 4    forindex = 7 closed_index = 4
        /// 9, 3, 2, 1, 0, 0, 0, 0, 0, 4    forindex = 8 closed_index = 4
        /// 9, 3, 2, 1, 4, 0, 0, 0, 0, 0    forindex = 9 closed_index = 5
        /// </summary>
        /// <param name="arr"></param>
        public void Solution(int[] arr)
        {
            int closed_index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 0 && closed_index == 0)
                {
                    closed_index = i;
                }
                if (arr[i] != 0 && closed_index != 0)
                {
                    // swap
                    int temp = arr[i];
                    arr[i] = arr[closed_index];
                    arr[closed_index] = temp;
                    if (i - closed_index > 1)
                    {
                        // 代表還有很多 0, 往下近一個就好
                        closed_index++;
                    }
                    else
                    {
                        // 代表只有他一個 0
                        closed_index = i;
                    }
                }
            }
        }
    }
}