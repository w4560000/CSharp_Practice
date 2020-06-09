using System;

namespace DesignPattern.Factory
{
    /// <summary>
    /// 定義:
    /// 定義一個用於建立物件的介面，讓工廠子類別決定建立產品類別。
    /// 用於在不指定待建立物件的具體類的情況下建立物件 (但必須知道工廠子類別名稱，抽象工廠則無該缺點)
    /// 
    /// 角色: 
    /// 1.Product：Product角色，定義工廠方法所建立的物件的(介面or抽象類別) [Car]
    /// 2.ConcreteProduct：具體Product角色，實現Product(介面方法or抽象方法) [Audi]、[BMW]
    /// 3.Factory: (1)抽象的工廠角色，宣告工廠方法，該方法返回一個Product型別的物件
    ///            (2)Factory可以定義一個工廠方法的預設實現，返回一個預設的ConcreteProduct物件。可以呼叫工廠方法以建立一個Product物件
    ///            [ICarFactory]
    /// 4.ConcreteFactory：具體的工廠角色，建立具體Product的子工廠，重寫工廠方法以返回一個ConcreteProduct例項 [AudiCarFactory]、[BMWCarFactory]
    /// 
    /// 範例設計邏輯:
    /// 1.建立一個介面 ICarFactory 並定義CreateCar的方法
    /// 2.個別建立CarFactory 並繼承ICarFactory 並實作CreateCar的方法
    /// 3.建立一個抽象類別 來規範Car類別的方法 並定義MakeCar的方法
    /// 4.個別建立汽車品牌類別 並繼承抽象類別Car 並實作MakeCar的方法
    /// 
    /// 與Simple_Factory不同之處在於
    /// 當要多製造新車時，只需要多新增
    /// 1.新車的工廠類別 繼承自ICarFactory介面
    /// 2.新車類別 繼承自Car抽象類別
    /// 
    /// 即可使用，無須更動原本的工廠類別
    /// </summary>
    public class Factory : IExecute
    {
        public void main()
        {
            Car car;

            car = new AudiCarFactory().CreateCar();
            car.MakeCar();

            car = new BMWCarFactory().CreateCar();
            car.MakeCar();

            Console.ReadLine();
        }
    }

    /// <summary>
    /// (Factory)
    /// </summary>
    interface ICarFactory
    {
        Car CreateCar();
    }

    /// <summary>
    /// (Product)
    /// </summary>
    internal abstract class Car
    {
        public abstract void MakeCar();
    }

    /// <summary>
    /// (ConcreteFactory)
    /// </summary>
    internal class AudiCarFactory : ICarFactory
    {
        public Car CreateCar()
        {
            return new Audi();
        }
    }

    /// <summary>
    /// (ConcreteProduct)
    /// </summary>
    class Audi : Car
    {
        public override void MakeCar()
        {
            Console.WriteLine("製造了 Audi");
        }
    }

    /// <summary>
    /// (ConcreteFactory)
    /// </summary>
    internal class BMWCarFactory : ICarFactory
    {
        public Car CreateCar()
        {
            return new Audi();
        }
    }

    /// <summary>
    /// (ConcreteProduct)
    /// </summary>
    class BMW : Car
    {
        public override void MakeCar()
        {
            Console.WriteLine("製造了 BMW");
        }
    }
}