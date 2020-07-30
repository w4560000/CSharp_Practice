using System;
using CommonClassLibary;

namespace DesignPattern.Abstract_Factory
{
    /// <summary>
    /// 定義:
    /// 提供一個建立一系列相關或相互依賴物件的介面，而無須指定它們實際類別
    /// 
    /// 角色:
    /// AbstractFactory：用於宣告生成抽象產品的方法 [ICarFactory]
    /// ConcreteFactory：實作AbstractFactory的方法，並建立Product，這些產品構成了一個產品族，每一個產品都位於某個產品等級結構中 [CarFactory]
    /// AbstractProduct：為每種產品宣告介面，在抽象產品中定義了產品的抽象業務方法 (若有可重複利用的方法，可改為建立抽象類別) [ICarManufacture]
    /// Product：定義具體工廠生產的具體產品物件，實現抽象產品介面中定義的業務方法 [AudiManufacture]、[BMWManufacture]
    /// 
    /// 範例設計邏輯:
    /// 1.建立AbstractFactory角色 [ICarFactory]               -> 定義實際要建立產品類別的方法
    /// 2.建立ConcreteFactory角色 [CarFactory]                -> 實作AbstractFactory所定義的方法
    /// 3.建立AbstractProduct角色 [ICarManufacture]           -> 定義產品類別的方法
    /// 4.建立Product角色 [AudiManufacture]、[BMWManufacture] -> 實作建立AbstractProduct所定義的方法
    /// 
    /// 與工廠模式差別在於
    /// 抽象工廠在建立實體時，不需要知道要由哪個ConcreteFactory來建立，只需要知道AbstractProduct即可
    /// 
    /// 優點: 隔離了ConcreteFactory、抽換Product很方便
    /// 缺點: 若要修改AbstractProduct定義的方法，則要連同Product角色實作方法一起修改，影響幅度較大
    /// </summary>
    public class Abstract_Factory : IExecute
    {
        public void Main()
        {
            ICarManufacture audi = new CarFactory().CreateAudiCar();
            audi.ManufactureEngine();
            audi.ManufactureSideDoor();

            ICarManufacture bmw = new CarFactory().CreateBMWCar();
            bmw.ManufactureEngine();
            bmw.ManufactureSideDoor();

            Console.ReadLine();
        }
    }

    /// <summary>
    /// (AbstractFactory)
    /// </summary>
    public interface ICarFactory
    {
        AudiManufacture CreateAudiCar();

        BMWManufacture CreateBMWCar();
    }

    /// <summary>
    /// (ConcreteFactory)
    /// </summary>
    public class CarFactory : ICarFactory
    {
        public AudiManufacture CreateAudiCar()
        {
            return new AudiManufacture();
        }

        public BMWManufacture CreateBMWCar()
        {
            return new BMWManufacture();
        }
    }

    /// <summary>
    /// (AbstractProduct)
    /// </summary>
    public interface ICarManufacture
    {
        public void ManufactureEngine();

        public void ManufactureSideDoor();
    }

    /// <summary>
    /// (Product)
    /// </summary>
    public class AudiManufacture : ICarManufacture
    {
        public void ManufactureEngine()
        {
            Console.WriteLine("製造了 Audi Engine");
        }

        public void ManufactureSideDoor()
        {
            Console.WriteLine("製造了 Audi SideDoor");
        }
    }

    /// <summary>
    /// (Product)
    /// </summary>
    public class BMWManufacture : ICarManufacture
    {
        public void ManufactureEngine()
        {
            Console.WriteLine("製造了 BMW Engine");
        }

        public void ManufactureSideDoor()
        {
            Console.WriteLine("製造了 BMW SideDoor");
        }
    }
}