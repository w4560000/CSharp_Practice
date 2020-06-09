using System;

namespace DesignPattern.Facade
{
    /// <summary>
    /// 定義:
    /// 為複雜的類別提供一個對外的類別，使得外界透過包裝好的Facade類別，更容易操作到複雜類別。
    ///
    /// 角色:
    /// Facade: 定義一個讓外界透過這個Facade類別能夠輕鬆操作複雜的子系統 [RestaurantFacade]
    /// SubSystem: 複雜的子系統 [MenuSystem] [ChefSystem] [PromoteSystem]
    /// 
    /// 優點:
    /// 透過Facade當作中介，讓外界不需要十分了解各個子系統邏輯，也可以達到其目的
    /// 
    /// 缺點:
    /// Facade由於是專為SubSystem的功能客製的，當子系統邏輯改變時，Facade也需要思考是否要調整
    /// 
    /// 使用情境:
    /// 例如當專案的各個類別過於複雜，可由資深人員建立Facade類別讓新進人員也可以快速操作到子類別功能
    /// </summary>
    public class Facade : IExecute
    {
        public void main()
        {
            RestaurantFacade facade = new RestaurantFacade();
            facade.GetMondayInfo();
        }
    }

    /// <summary>
    /// Facade
    /// </summary>
    internal class RestaurantFacade
    {
        public MenuSystem _menuSystem;
        public ChefSystem _chefSystem;
        public PromoteSystem _promoteSystem;

        public RestaurantFacade()
        {
            _menuSystem = new MenuSystem();
            _chefSystem = new ChefSystem();
            _promoteSystem = new PromoteSystem();
        }

        public void GetMondayInfo()
        {
            _menuSystem.GetMondayMenu();
            _chefSystem.GetMondayChefeSchedule();
            _promoteSystem.GetPromoteDishes();
        }
    }

    internal class MenuSystem
    {
        public void GetMondayMenu()
        {
            Console.WriteLine("星期一菜單: 金沙小卷、沙茶牛肉");
        }
    }

    internal class ChefSystem
    {
        public void GetMondayChefeSchedule()
        {
            Console.WriteLine("大廚: 阿基師");
        }
    }

    internal class PromoteSystem
    {
        public void GetPromoteDishes()
        {
            Console.WriteLine("金沙小卷 特價一盤10塊");
        }
    }
}