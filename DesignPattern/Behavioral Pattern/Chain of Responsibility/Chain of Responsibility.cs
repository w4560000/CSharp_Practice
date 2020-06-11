using System;

namespace DesignPattern.Chain_of_Responsibility
{
    /// <summary>
    /// 定義:
    /// 將處理請求的類別，以繼承方式串聯成一條鍊，讓請求的處理一層一層往上找，直到能夠處理請求的類別為止。
    /// 
    /// 角色:
    /// Handler: 定義一個指定上級處理方法與抽象請求方法的抽象類別 [ManagerHandler]
    /// ConcreteHandler: 繼承自Handler的類別，並實作請求方法 [Manager] [Director] [GeneralManager]
    /// 
    /// 優點:
    /// 將各個職責切開，並可以動態調整，降低了之間的偶和程度
    /// 
    /// 
    /// 範例來源:
    /// https://xyz.cinc.biz/2013/07/chain-of-responsibility-pattern.html
    /// </summary>
    public class Chain_of_Responsibility : IExecute
    {
        public void main()
        {
            // 管理者初始化
            Manager manager = new Manager("阿福"); // 經理
            Director director = new Director("技安"); // 協理
            GeneralManager generalManager = new GeneralManager("宜靜"); // 總經理

            manager.SetUpManager(director); // 設定經理的上級為協理
            director.SetUpManager(generalManager); // 設定協理的上級為總經理

            // 假單初始化
            LeaveRequest leaveRequest = new LeaveRequest(); // 假單
            leaveRequest.Name = "大雄"; // 員工姓名

            leaveRequest.DayNum = 1; // 請假天數
            manager.RequestPersonalLeave(leaveRequest);// 送出1天的假單

            leaveRequest.DayNum = 3; // 請假天數
            manager.RequestPersonalLeave(leaveRequest);// 送出3天的假單

            leaveRequest.DayNum = 7; // 請假天數
            manager.RequestPersonalLeave(leaveRequest);// 送出7天的假單

            leaveRequest.DayNum = 10; // 請假天數
            manager.RequestPersonalLeave(leaveRequest);// 送出10天的假單
        }
    }

    /// <summary>
    /// 管理者處理事假申請的抽象類別 (Handler)
    /// </summary>
    internal abstract class ManagerHandler
    {
        protected string name;
        protected ManagerHandler upManager; // 上一級的管理者

        public ManagerHandler(string name)
        {
            this.name = name;
        }

        // 設定上一級的管理者
        public void SetUpManager(ManagerHandler upManager)
        {
            this.upManager = upManager;
        }

        // 事假處理
        abstract public void RequestPersonalLeave(LeaveRequest leaveRequest);
    }

    /// <summary>
    /// 經理 (ConcreteHandler)
    /// </summary>
    internal class Manager : ManagerHandler
    {
        public Manager(string name) : base(name)
        {
        }

        public override void RequestPersonalLeave(LeaveRequest leaveRequest)
        {
            if (leaveRequest.DayNum <= 2)
            {
                // 2天以內，經理可以批准
                Console.WriteLine("經理 {0} 已批准 {1}{2}天的事假", this.name, leaveRequest.Name, leaveRequest.DayNum);
            }
            else
            {
                // 超過2天，轉呈上級
                if (upManager != null)
                {
                    upManager.RequestPersonalLeave(leaveRequest);
                }
            }
        }
    }

    /// <summary>
    /// 協理 (ConcreteHandler)
    /// </summary>
    internal class Director : ManagerHandler
    {
        public Director(string name) : base(name)
        {
        }

        public override void RequestPersonalLeave(LeaveRequest leaveRequest)
        {
            if (leaveRequest.DayNum <= 5)
            {
                // 5天以內，經理可以批准
                Console.WriteLine("協理 {0} 已批准 {1}{2}天的事假", this.name, leaveRequest.Name, leaveRequest.DayNum);
            }
            else
            {
                // 超過5天，轉呈上級
                if (upManager != null)
                {
                    upManager.RequestPersonalLeave(leaveRequest);
                }
            }
        }
    }

    /// <summary>
    /// 總經理 (ConcreteHandler)
    /// </summary>
    internal class GeneralManager : ManagerHandler
    {
        public GeneralManager(string name) : base(name)
        {
        }

        public override void RequestPersonalLeave(LeaveRequest leaveRequest)
        {
            if (leaveRequest.DayNum <= 7)
            {
                // 7天以內，總經理批准
                Console.WriteLine("總經理 {0} 已批准 {1}{2}天的事假", this.name, leaveRequest.Name, leaveRequest.DayNum);
            }
            else
            {
                // 超過7天以上，再深入了解原因
                Console.WriteLine("總經理 {0} 要再了解 {1}{2}天的事假原因", this.name, leaveRequest.Name, leaveRequest.DayNum);
            }
        }
    }

    // 假單
    internal class LeaveRequest
    {
        public string Name { get; set; }

        public int DayNum { get; set; }
    }
}