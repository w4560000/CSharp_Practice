using System;

namespace DesignPattern.Decorator
{
    /// <summary>
    /// 定義:
    /// 可以動態的擴展類別的功能，而不需要修改原類別
    /// 
    /// 角色:
    /// Component: 定義一個抽象類別源頭 [Hamburger]
    /// ConcreteComponent: 繼承至Component [BeafHamburger] [PorkHamburger] [BaconHamburger]
    /// Decorator: 定義一個裝飾用的抽象類別，並繼承至Component，可以自定義需要擴充的方法給ConcreteDecorator實現，進而操作Component的member [MaterialDecorator]
    /// ConcreteDecorator: 繼承至Decorator，實作其方法 [Egg] [Lettuce]
    /// 
    /// 優點:
    /// 擴展性佳，可以透過ConcreteComponent & ConcreteDecorator 來達成多種不同行為的組合
    /// 
    /// 缺點:
    /// 需要建立的類別很多，若沒有仔細劃分，會顯得十分雜亂
    /// 
    /// 當接口介面穩定時，在使用比較好。
    /// </summary>
    public class Decorator : IExecute
    {
        public void main()
        {
            Hamburger baconHamburger = new BaconHamburger();
            baconHamburger = new Egg(baconHamburger);
            baconHamburger = new Lettuce(baconHamburger);

            Hamburger beafHamburger = new BeafHamburger();
            beafHamburger = new Egg(beafHamburger);

            Console.WriteLine($"{baconHamburger.GetProductName()} -- 價格: {baconHamburger.Cost()}");
            Console.WriteLine($"{beafHamburger.GetProductName()} -- 價格: {beafHamburger.Cost()}");
        }
    }

    /// <summary>
    /// 抽象類別漢堡 (Component)
    /// </summary>
    public abstract class Hamburger
    {
        private string _productName;

        public Hamburger(string productName)
        {
            _productName = productName;
        }

        public abstract int Cost();

        public virtual string GetProductName()
        {
            return _productName;
        }

    }

    /// <summary>
    /// 牛肉漢堡 (ConcreteComponent)
    /// </summary>
    public class BeafHamburger : Hamburger
    {
        public BeafHamburger(string productName = "牛肉漢堡") : base(productName)
        {
        }

        public override int Cost()
        {
            return 100;
        }
    }

    /// <summary>
    /// 豬肉漢堡 (ConcreteComponent)
    /// </summary>
    public class PorkHamburger : Hamburger
    {
        public PorkHamburger(string productName = "豬肉漢堡") : base(productName)
        {
        }

        public override int Cost()
        {
            return 50;
        }
    }

    /// <summary>
    /// 培根漢堡 (ConcreteComponent)
    /// </summary>
    public class BaconHamburger : Hamburger
    {
        public BaconHamburger(string productName = "培根漢堡") : base(productName)
        {
        }

        public override int Cost()
        {
            return 30;
        }
    }

    /// <summary>
    /// 食材的裝飾類別 (Decorator)
    /// </summary>
    public abstract class MaterialDecorator : Hamburger
    {
        private Hamburger _hamburger;

        public MaterialDecorator(Hamburger hamburger) : base("")
        {
            _hamburger = hamburger;
        }

        public override string GetProductName()
        {
            return CheckExistMaterial() ? 
                $"{_hamburger.GetProductName()} , {SetMaterial()}" : 
                $"{_hamburger.GetProductName()}  食材: {SetMaterial()}";
        }

        private bool CheckExistMaterial()
        {
            return _hamburger.GetProductName().Contains("食材");
        }

        public abstract string SetMaterial();
    }

    /// <summary>
    /// 食材:蛋 (ConcreteDecorator)
    /// </summary>
    public class Egg : MaterialDecorator
    {
        private Hamburger _hamburger;

        public Egg(Hamburger hamburger) : base(hamburger)
        {
            _hamburger = hamburger;
        }

        public override int Cost()
        {
            return _hamburger.Cost() + 10;
        }

        public override string SetMaterial()
        {
            return "蛋";
        }
    }

    /// <summary>
    /// 食材:萵苣 (ConcreteDecorator)
    /// </summary>
    public class Lettuce : MaterialDecorator
    {
        private Hamburger _hamburger;

        public Lettuce(Hamburger hamburger) : base(hamburger)
        {
            _hamburger = hamburger;
        }

        public override int Cost()
        {
            return _hamburger.Cost() + 5;
        }

        public override string SetMaterial()
        {
            return "萵苣";
        }
    }
}