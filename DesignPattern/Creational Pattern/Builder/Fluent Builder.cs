using System;
using System.Collections.Generic;

namespace DesignPattern.Fluent_Builder
{
    /// <summary>
    /// 由外部自行實作方法，不需再特別建立新的類別來實作
    /// 步驟流程與傳入參數由外部決定
    /// 
    /// 在擁有大量參數的類別中，未必每一項參數都必須要寫入
    /// 此時由外部控制可以更清楚明白傳入了哪些參數，更容易閱讀
    /// 
    /// 用途:
    /// 不想再多新建ConcreteBuilder時可以使用，參數由user自己操作
    /// </summary>
    public class Fluent_Builder : IExecute
    {
        public void main()
        {
            Juice orangeJuice = new JuiceBuilder().PrepareFruit("買橘子 -> 剝皮 -> 清洗 -> 切塊")
                                                  .Blend("將橘子放入果汁機中")
                                                  .PourIntoCup("將橘子汁裝杯")
                                                  .Finish();

            orangeJuice.ShowProcessStep();
        }
    }

    public class Juice
    {
        public List<string> ManufactureProcess { get; set; } = new List<string>();

        public void ShowProcessStep()
        {
            ManufactureProcess.ForEach(x => Console.WriteLine(x));
        }
    }

    public class JuiceBuilder
    {
        private string _prepareFruitStep = string.Empty;
        private string _blendStep = string.Empty;
        private string _pourIntoCupStep = string.Empty;

        private Juice _juice = new Juice();

        public JuiceBuilder PrepareFruit(string prepareFruitStep)
        {
            _prepareFruitStep = prepareFruitStep;
            return this;
        }

        public JuiceBuilder Blend(string blendStep)
        {
            _blendStep = blendStep;
            return this;
        }

        public JuiceBuilder PourIntoCup(string pourIntoCupStep)
        {
            _pourIntoCupStep = pourIntoCupStep;
            return this;
        }

        public Juice Finish()
        {
            _juice.ManufactureProcess = new List<string> { _prepareFruitStep, _blendStep, _pourIntoCupStep };
            return _juice;
        }
    }
}