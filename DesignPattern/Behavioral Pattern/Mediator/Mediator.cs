using CommonClassLibary;
using System;

namespace DesignPattern.Mediator
{
    /// <summary>
    /// 定義:
    /// 封裝一個中介類別，將邏輯定義在其中，其餘類別則透過中介來互相溝通，而不需個別互相溝通&引用
    ///
    /// 角色:
    /// 1. Mediator : 定義其餘類別需要溝通的方法 [AbstractMediator]
    /// 2. ConcreteMediator : 實作Mediator定義的方法，將邏輯統一封裝在這個類別中 [MediatorPater]
    /// 3. Colleague: 定義其餘類別的方法，並且注入ConcreteMediator讓實作的ConcreteColleague方便引用 [AbstractCardPatner]
    /// 4. ConcreteColleague: 實作Colleague定義的方法，當需要與其餘ConcreteColleague溝通時，透過ConcreteMediator來進行操作 [ParterA] [ParterB]
    ///
    /// 優點:
    /// 若邏輯封裝在中介類別，則各其餘類別的耦合會降低，主要透過中介來互相溝通即可，要修改邏輯也只需要修改中介類別邏輯
    ///
    /// 範例來源: https://blog.csdn.net/qq_39003429/article/details/82705509
    /// </summary>
    public class Mediator : IExecute
    {
        public void Main()
        {
            AbstractCardPatner A = new ParterA();
            AbstractCardPatner B = new ParterB();

            A.MoneyCount = 20;
            B.MoneyCount = 30;

            // 宣告中介類別，並將其餘類別注入
            AbstractMediator mediator = new MediatorPater(A, B);

            // A贏 5元, B賠 5元
            A.ChangeCount(5, mediator);
            Console.WriteLine("A 現在的錢是：{0}", A.MoneyCount);// A贏 5元 -> A = 20 + 5 = 25
            Console.WriteLine("B 現在的錢是：{0}", B.MoneyCount); // B賠 5元 -> B = 30 - 5 = 25

            // A賠 10元, B贏 10元
            B.ChangeCount(10, mediator);
            Console.WriteLine("A 現在的錢是：{0}", A.MoneyCount);// A賠 10元 -> A = 25 - 10 = 15
            Console.WriteLine("B 現在的錢是：{0}", B.MoneyCount);// B贏 10元 -> B = 25 + 10 = 35
        }
    }

    /// <summary>
    /// 抽象牌友類 (Colleague)
    /// </summary>
    public abstract class AbstractCardPatner
    {
        public int MoneyCount { get; set; }

        public AbstractCardPatner()
        {
            MoneyCount = 0;
        }

        public abstract void ChangeCount(int Count, AbstractMediator mediator);
    }

    /// <summary>
    /// 具體牌友類 (ConcreteColleague)
    /// </summary>
    public class ParterA : AbstractCardPatner
    {
        // 依賴與抽象中介者類別
        public override void ChangeCount(int Count, AbstractMediator mediator)
        {
            mediator.AWin(Count);
        }
    }

    public class ParterB : AbstractCardPatner
    {
        // 依賴與抽象中介者類別
        public override void ChangeCount(int Count, AbstractMediator mediator)
        {
            mediator.BWin(Count);
        }
    }

    /// <summary>
    /// 抽象中介者類別 (Mediator)
    /// </summary>
    public abstract class AbstractMediator
    {
        protected AbstractCardPatner A;
        protected AbstractCardPatner B;

        public AbstractMediator(AbstractCardPatner a, AbstractCardPatner b)
        {
            A = a;
            B = b;
        }

        public abstract void AWin(int count);

        public abstract void BWin(int count);
    }

    /// <summary>
    /// 具體中介者類別 (ConcreteMediator)
    /// </summary>
    public class MediatorPater : AbstractMediator
    {
        public MediatorPater(AbstractCardPatner a, AbstractCardPatner b) : base(a, b)
        {
        }

        public override void AWin(int count)
        {
            A.MoneyCount += count;
            B.MoneyCount -= count;
        }

        public override void BWin(int count)
        {
            A.MoneyCount -= count;
            B.MoneyCount += count;
        }
    }
}