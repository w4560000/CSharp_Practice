using System;
using System.Collections.Generic;

namespace DesignPattern.Flyweight
{
    /// <summary>
    /// 定義:
    /// 透過Cache住物件實體，讓使用者反覆利用相同的Instance，以避免建立大量Instance而耗盡記憶體
    ///
    /// 角色:
    /// Flyweight: 定義出物件的外部狀態與內部狀態的抽象類別 [ChessFlyweight]
    /// ConcreteFlyweight: 實作Flyweight的抽象方法 [ConcreteFlyweight]
    /// UnsharedConcreteFlyweight: 同樣實作Flyweight的抽象方法，但不會有FlyweightFactory來幫忙做Instance的cache [UnsharedConcreteChessFlyweight]
    /// FlyweightFactory: 以Sample Factory模式幫忙創建出ConcreteFlyweight Instance的Cache [ChessFlyweightFactory]
    ///
    /// 定義Flyweight裡區分內部資料與外部資料
    /// Intrinsic State: 內部資料 - Flyweight類別內不隨外界環境而改變的Member
    /// Extrinsic State: 外部資料 - Flyweight類別內會隨外界環境而改變的Member，由外界自行設定
    ///
    /// 優點:
    /// 對於需要大量產生相同資料又不會更改的物件，用flyweight模式可以節省許多記憶體
    ///
    /// 缺點:
    /// 由於資料有分為不會更改與需要更改兩種，進而區分出內部資料與外部資料，使得使用上略為複雜
    /// 如果當有不會更改的資料也需要更改時，可以使用UnsharedConcreteFlyweight 直接new 出新的Instance
    /// 但若使用者沒有了解到其含義，則能會繼續使用FlyweightFactory的Instance進行修改，會造成共用的資料會統一修改，後果不堪設想
    /// 
    /// 範例來源:
    /// https://xyz.cinc.biz/2013/07/flyweight-pattern.html
    /// </summary>
    public class Flyweight : IExecute
    {
        public void main()
        {
            ChessFlyweight a1 = ChessFlyweightFactory.GetChessFlyweight("黑棋");
            a1.Display(1, 1);
            ChessFlyweight a2 = ChessFlyweightFactory.GetChessFlyweight("黑棋");
            a2.Display(1, 2);
            ChessFlyweight a3 = ChessFlyweightFactory.GetChessFlyweight("黑棋");
            a3.Display(1, 3);
            ChessFlyweight b1 = ChessFlyweightFactory.GetChessFlyweight("白棋");
            b1.Display(2, 1);
            ChessFlyweight b2 = ChessFlyweightFactory.GetChessFlyweight("白棋");
            b2.Display(2, 2);
            ChessFlyweight b3 = ChessFlyweightFactory.GetChessFlyweight("白棋");
            b3.Display(2, 3);

            Console.WriteLine("ChessFlyweight物件數量：{0}", ChessFlyweightFactory.GetChessFlyweightCount());
        }
    }

    /// <summary>
    /// 棋子享元工廠，回傳棋子物件 (FlyweightFactory)
    /// </summary>
    public static class ChessFlyweightFactory
    {
        private static readonly object lockObject = new object();

        private static Dictionary<string, ChessFlyweight> _chessFlyweight = new Dictionary<string, ChessFlyweight>();

        // 考量到多執行續情況，需要lock住存取邏輯，否則有可能同時新增相同的key至Dictionary而發生錯誤
        public static ChessFlyweight GetChessFlyweight(string key)
        {
            lock (lockObject)
            {
                if (_chessFlyweight.TryGetValue(key, out ChessFlyweight chessFlyweight))
                    return chessFlyweight;
                else
                    _chessFlyweight.Add(key, new ConcreteChessFlyweight(key));
            }

            return _chessFlyweight[key];
        }

        // 取得目前棋子物件數量
        public static int GetChessFlyweightCount()
        {
            return _chessFlyweight.Count;
        }
    }

    /// <summary>
    /// 棋子享元抽像物件 (Flyweight)
    /// </summary>
    public abstract class ChessFlyweight
    {
        protected string name; // 共享資料

        public ChessFlyweight(string name)
        {
            this.name = name;
        }

        public abstract void Display(int x, int y);
    }

    /// <summary>
    /// 棋子享元(共享物件) (ConcreteFlyweight)
    /// </summary>
    public class ConcreteChessFlyweight : ChessFlyweight
    {
        public ConcreteChessFlyweight(string name)
            : base(name)
        {
        }

        // X、Y座標，非共享資料
        public override void Display(int x, int y)
        {
            Console.WriteLine("{0}({1},{2})", this.name, x, y);
        }
    }

    /// <summary>
    /// 棋子享元(非共享物件) (UnsharedConcreteFlyweight)
    /// </summary>
    public class UnsharedConcreteChessFlyweight : ChessFlyweight
    {
        public UnsharedConcreteChessFlyweight(string name)
            : base(name)
        {
        }

        public override void Display(int x, int y)
        {
            Console.WriteLine("不共用的物件:{0}({1},{2})", this.name, x, y);
        }
    }
}