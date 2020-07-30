using System;
using System.Collections.Generic;
using CommonClassLibary;

namespace DesignPattern.Builder
{
    /// <summary>
    /// 定義:
    /// 在Director傳入不同的ConcreteBuilder，來建立出步驟相同的不同產品
    /// 達到以相同的構造模式，建立出不同產品
    ///
    /// 角色:
    /// 1. Proudct: 定義最後要建立的產品 [Juice]
    /// 2. Builder: 定義一個介面用來規範建立產品各個步驟方法 [IJuiceBuilder]
    /// 3. ConcreteBuilder: 實作建立產品各個步驟方法 [AppleJuiceBuilder、BananaJuiceBuilder]
    /// 4. Director: 定義產品生產的步驟 [JuiceDirector]
    /// 
    /// 優點:
    /// 雖然功能與sampleFactory相似，但多了Director可以定義各個生產的步驟
    /// 在有明確步驟流程的需求下適用
    /// 比如成立一筆訂單後，會再去減庫存，甚至是更多繁瑣步驟
    /// 都可以透過builder pattern完成，明確定義了方法步驟順序
    /// 各個不同產品只需要處理實作方法而已，不用管生產步驟
    /// 
    /// 用途:
    /// 當需要統一操作步驟
    /// 
    /// 缺點:
    /// 在擁有大量參數時，未必每個都需要使用到
    public class Builder : IExecute
    {
        public void Main()
        {
            JuiceDirector appleJuice = new JuiceDirector(new AppleJuiceBuilder()).MakeJuice();
            JuiceDirector bananaJuice = new JuiceDirector(new BananaJuiceBuilder()).MakeJuice();

            appleJuice.GetJuice().ShowProcessStep();

            Console.WriteLine("\n-------------------------\n");

            bananaJuice.GetJuice().ShowProcessStep();
        }
    }

    /// <summary>
    /// (Proudct)
    /// </summary>
    public class Juice
    {
        public List<string> ManufactureProcess { get; set; } = new List<string>();

        public void ShowProcessStep()
        {
            ManufactureProcess.ForEach(x => Console.WriteLine(x));
        }
    }

    /// <summary>
    /// (Builder)
    /// </summary>

    public interface IJuiceBuilder
    {
        void PrepareFruit();

        void Blend();

        void PourIntoCup();

        Juice GetJuiceInstance();
    }

    /// <summary>
    /// (ConcreteBuilder)
    /// </summary>
    public class AppleJuiceBuilder : IJuiceBuilder
    {
        private Juice _juice = new Juice();

        public void PrepareFruit()
        {
            _juice.ManufactureProcess.Add("買蘋果 -> 洗蘋果 -> 削皮 -> 切蘋果");
        }

        public void Blend()
        {
            _juice.ManufactureProcess.Add("將蘋果放入果汁機中打成汁");
        }

        public void PourIntoCup()
        {
            _juice.ManufactureProcess.Add("將蘋果汁倒入杯中");
        }

        public Juice GetJuiceInstance()
        {
            return _juice;
        }
    }

    /// <summary>
    /// (ConcreteBuilder)
    /// </summary>
    public class BananaJuiceBuilder : IJuiceBuilder
    {
        private Juice _juice = new Juice();

        public void PrepareFruit()
        {
            _juice.ManufactureProcess.Add("買香蕉 -> 切香蕉");
        }

        public void Blend()
        {
            _juice.ManufactureProcess.Add("將香蕉、果糖放入果汁機中打成汁");
        }

        public void PourIntoCup()
        {
            _juice.ManufactureProcess.Add("將香蕉汁倒入杯中");
        }

        public Juice GetJuiceInstance()
        {
            return this._juice;
        }
    }

    /// <summary>
    /// (Director)
    /// </summary>
    public class JuiceDirector
    {
        private IJuiceBuilder _juiceBuilder;

        public JuiceDirector(IJuiceBuilder juiceBuilder)
        {
            _juiceBuilder = juiceBuilder;
        }

        public JuiceDirector MakeJuice()
        {
            _juiceBuilder.PrepareFruit();
            _juiceBuilder.Blend();
            _juiceBuilder.PourIntoCup();

            return this;
        }

        public Juice GetJuice()
        {
            return _juiceBuilder.GetJuiceInstance();
        }
    }
}