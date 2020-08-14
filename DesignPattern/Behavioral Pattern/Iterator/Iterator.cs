using CommonClassLibary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPattern.Iterator
{
    /// <summary>
    /// 定義:
    /// 提供一個跌代器，當外界需要訪問其內部資料時，不需要得知其property名稱或是member名稱
    /// 
    /// 角色:
    /// Iterator: 定義遍歷元素所需方法的介面 .Net提供的介面[IEnumerator、IEnumerator<T>]
    /// ConcreteIterator: 實作Iterator定義的介面 [ConcreteIterator]
    /// Aggregate: 定義取得Iterator的方法 .Net提供的介面[IEnumerable、IEnumerable<T>]
    /// ConcreteAggregate: 實作Aggregate定義的界面 [ConcreteAggregate]
    /// 
    /// 說明:
    /// .Net的foreach背後就是透過Iterator Pattern去遍歷各元素
    /// 而各容器物件 如 List 、 Dictionary ...等等可以用foreach來遍歷就是其內部有繼承自IEnumberable並實作其方法
    /// </summary>
    public class Iterator : IExecute
    {
        public void Main()
        {
            var concreteAggregate = new ConcreteAggregate<string>();
            concreteAggregate[0] = "0";
            concreteAggregate[1] = "1";
            concreteAggregate[2] = "2";
            concreteAggregate[3] = "3";
            concreteAggregate[4] = "4";
            concreteAggregate[5] = "5";
            concreteAggregate[6] = "6";


            foreach (var item in concreteAggregate)
                Console.WriteLine(item);


            Console.WriteLine("\n測試Yield用法:");
            foreach (var item in concreteAggregate.YieldReturnTest())
                Console.WriteLine(item);
        }

        public class ConcreteAggregate<T> : IEnumerable<T>
        {
            private List<T> _items = new List<T>();
            public IEnumerator<T> GetEnumerator()
            {
                return new ConcreteIterator<T>(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return _items.Count; }
            }
            public T this[int index]
            {
                get { return _items[index]; }
                set { _items.Insert(index, value); }
            }

            public IEnumerable<T> YieldReturnTest()
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    // yield break會中斷yield return的回傳
                    if (i == 4)
                        yield break;

                    // yield return 會由編譯器幫你自動實作IEnumerator，就不需在自己刻一次
                    yield return _items[i];
                }
            }
        }

        public class ConcreteIterator<T> : IEnumerator<T>
        {
            private ConcreteAggregate<T> Data;
            private int _index;

            public T Current
            {
                get
                {
                    return Data[_index];
                }
            }

            object IEnumerator.Current => Current;

            public ConcreteIterator(ConcreteAggregate<T> data)
            {
                Data = data;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index += 1;

                return _index >= 0 && _index != Data.Count;
            }

            public void Reset()
            {
                _index = 0;
            }

            public void Dispose()
            {
                Data = null;
            }
        }
    }
}