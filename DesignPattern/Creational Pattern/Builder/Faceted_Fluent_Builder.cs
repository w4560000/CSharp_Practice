using System;

namespace DesignPattern.Faceted_Fluent_Builder
{
    /// <summary>
    /// Fluent_Build加強版
    /// 
    /// 當物件需要更細的去區分時，可使用此pattern，更易方便閱讀
    /// </summary>
    public class Faceted_Fluent_Builder : IExecute
    {
        public void main()
        {
            Juice guavaJuice = new JuiceFacadeBuilder()
                            .FruitInfo.SetFruitName("芭樂").SetOrigin("台中").SetVariety("紅心的")
                            .JuiceSaleInfo.SetCapacity(1000).SetUnitPrice(200)
                            .Build();

            Console.WriteLine(guavaJuice.ToString());
        }
    }

    public class Juice
    {
        public string FruitName { get; set; }
        public string Origin { get; set; }
        public string Variety { get; set; }

        public int Capacity { get; set; }
        public int UnitPrice { get; set; }

        public override string ToString()
        {
            return $@"{FruitName}汁資訊: 原產地: {Origin}, 品種: {Variety}, 容量: {Capacity}ml, 單價: NT$ {UnitPrice}";
        }
    }

    public class JuiceFacadeBuilder
    {
        protected Juice _juice;

        public JuiceFacadeBuilder()
        {
            _juice = new Juice();
        }

        public Juice Build() => _juice;

        public FruitInfoBuilder FruitInfo => new FruitInfoBuilder(_juice);
        public JuiceSaleInfoBuilder JuiceSaleInfo => new JuiceSaleInfoBuilder(_juice);
    }

    public class FruitInfoBuilder : JuiceFacadeBuilder
    {
        public FruitInfoBuilder(Juice juice)
        {
            _juice = juice;
        }

        public FruitInfoBuilder SetFruitName(string fruitName)
        {
            _juice.FruitName = fruitName;
            return this;
        }

        public FruitInfoBuilder SetOrigin(string origin)
        {
            _juice.Origin = origin;
            return this;
        }

        public FruitInfoBuilder SetVariety(string variety)
        {
            _juice.Variety = variety;
            return this;
        }
    }

    public class JuiceSaleInfoBuilder : JuiceFacadeBuilder
    {
        public JuiceSaleInfoBuilder(Juice juice)
        {
            _juice = juice;
        }

        public JuiceSaleInfoBuilder SetCapacity(int capacity)
        {
            _juice.Capacity = capacity;
            return this;
        }

        public JuiceSaleInfoBuilder SetUnitPrice(int unitPrice)
        {
            _juice.UnitPrice = unitPrice;
            return this;
        }
    }
}