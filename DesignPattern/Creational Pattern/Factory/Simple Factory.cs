using System;

namespace DesignPattern.Simple_Factory
{
    /// <summary>
    /// 定義:
    /// 由一個工廠物件決定創建出哪一種產品類別
    /// 
    /// 角色:
    /// Factory:建立產品類別的判斷角色 [CarFactory]
    /// Product:定義工廠方法所建立的物件的(介面or抽象類別) [Car]
    /// ConcreteProduct:具體Product角色，實現Product(介面方法or抽象方法) [Audi]、[BMW]
    /// 
    /// 範例設計邏輯:
    /// 1.建立抽象類別Car 並定義MakeCar的抽象方法
    /// 2.個別建立汽車品牌類別 繼承抽象類別Car 並實作MakeCar的抽象方法
    /// 3.建立CarFactory類別 來判斷要建立哪一個汽車品牌類別
    /// 
    /// 
    /// 當沒有使用工廠類別 或 DI時，常常會寫很多new class()
    /// 會造成類別之間耦合太重，當要抽換掉class時，需要更改的地方會很多。
    /// 
    /// 而使用簡單工廠模式 可以讓類別之間多了一層工廠當中介
    /// 當要抽換類別或新增時 修改工廠類別即可
    /// 
    /// 
    /// 優點: 降低耦合、類別來源統一管理
    /// 缺點: 抽換或新增時 需要再修改工廠類別 而更動原本的程式就有改壞的風險
    /// </summary>
    public class Simple_Factory : IExecute
    {
        public void main()
        {
            Car aa;
            aa = CarFactory.CreateCar(CarEnum.audi);
            aa.MakeCar();

            aa = CarFactory.CreateCar(CarEnum.bmw);
            aa.MakeCar();

            Console.ReadLine();
        }
    }

    abstract class Car
    {
        public abstract void MakeCar();
    }

    class Audi : Car
    {
        public override void MakeCar()
        {
            Console.WriteLine("製造了 Audi");
        }
    }

    class BMW : Car
    {
        public override void MakeCar()
        {
            Console.WriteLine("製造了 BMW");
        }
    }

    class CarFactory
    {
        public static Car CreateCar(CarEnum car)
        {
            return car switch
            {
                CarEnum.audi => new Audi(),
                CarEnum.bmw => new BMW(),
                _ => throw new Exception("沒有這個類別"),
            }; ;
        }
    }

     enum CarEnum
    {
        audi,
        bmw
    }
}