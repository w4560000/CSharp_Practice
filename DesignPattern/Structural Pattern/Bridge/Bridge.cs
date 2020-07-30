using System;
using CommonClassLibary;

namespace DesignPattern.Bridge
{
    /// <summary>
    /// 定義:
    /// 將抽象部分與實現部分分離，使它們都可以獨立地變化。
    ///
    /// 角色:
    /// 1. Abstraction : 定義出抽象動作 [RemoteControl]
    /// 2. Refined Abstraction : implementation abstraction approach，當作Abstraction與Implementor的中介，使得設計更彈性 [ConcreteRemoter]
    /// 3. Implementor : 定義出底層操作的抽象介面 [IControlImplementor]
    /// 4. Concrete Implementor : 實作Implementor的方法 [AppRemoter] [TraditionRemoter]
    /// 
    /// 優點:
    /// Implementor的抽象介面可以更彈性的實現底層方法
    /// Refined Abstraction負責實作Abstraction時，可以更彈性的利用Implementor的方法在加以操作
    /// 
    /// 若一個Refined Abstraction不夠實現需求，還可以新增Refined Abstraction
    /// 所以Implementor搭配Refined Abstraction，可以實現二維的彈性組合
    /// </summary>
    public class Bridge : IExecute
    {
        public void Main()
        {
            ConcreteRemoter a = new ConcreteRemoter(new AppRemoter());
            ConcreteRemoter b = new ConcreteRemoter(new TraditionRemoter());

            // 分別控制冷氣和冰箱
            Console.WriteLine("手機App控制流程");
            a.ControlAirConditioner();
            a.ControlRefrigerator();

            Console.WriteLine("\n-----------------------\n");

            Console.WriteLine("傳統遙控器控制流程");
            b.ControlAirConditioner();
            b.ControlRefrigerator();

            Console.Read();
        }
    }

    /// <summary>
    /// 遠端遙控的抽象類別 (Abstraction)
    /// </summary>
    internal abstract class RemoteControl
    {
        protected IControlImplementor _controlImplementor;

        protected RemoteControl(IControlImplementor controlImplementor)
        {
            this._controlImplementor = controlImplementor;
        }

        public abstract void ControlAirConditioner();

        public abstract void ControlRefrigerator();
    }

    /// <summary>
    /// 遙控類別 (抽象物件) (Refined Abstraction)
    /// </summary>
    internal class ConcreteRemoter : RemoteControl
    {
        public ConcreteRemoter(IControlImplementor controlImplementor)
            : base(controlImplementor)
        {
        }

        public override void ControlAirConditioner()
        {
            _controlImplementor.PowerOn();
            _controlImplementor.ControlAirConditioner();
            Console.WriteLine("");
        }

        public override void ControlRefrigerator()
        {
            _controlImplementor.PowerOn();
            _controlImplementor.ControlRefrigerator();
            Console.WriteLine("");
        }
    }

    /// <summary>
    /// 遙控器實作介面 (Implementor)
    /// </summary>
    internal interface IControlImplementor
    {
        void PowerOn();

        void ControlAirConditioner();

        void ControlRefrigerator();
    }

    /// <summary>
    /// APP遙控 (Concrete Implementor)
    /// </summary>
    internal class AppRemoter : IControlImplementor
    {
        public void PowerOn()
        {
            Console.WriteLine("手機開啟 -> 並啟動APP");
        }

        public void ControlAirConditioner()
        {
            Console.WriteLine("以手機App遙控冷氣");
        }

        public void ControlRefrigerator()
        {
            Console.WriteLine("以手機App遙控冰箱");
        }
    }

    /// <summary>
    /// 傳統遙控器 (Concrete Implementor)
    /// </summary>
    internal class TraditionRemoter : IControlImplementor
    {
        public void PowerOn()
        {
            Console.WriteLine("裝上遙控器三號電池 -> 開機");
        }

        public void ControlAirConditioner()
        {
            Console.WriteLine("以傳統遙控器控制冷氣");
        }

        public void ControlRefrigerator()
        {
            Console.WriteLine("以傳統遙控器控制冰箱");
        }
    }
}