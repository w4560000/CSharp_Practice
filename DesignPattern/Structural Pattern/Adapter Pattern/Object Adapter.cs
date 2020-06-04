using System;
using System.Collections.Generic;

namespace DesignPattern.Object_Adapter
{
    /// <summary>
    /// 與Class Adpater的差別在於:
    /// Object Adapter Pattern的Adapter在轉接Adaptee時，不透過繼承的方式
    /// 而是透過建構子傳入Adaptee的物件來做轉換。
    ///
    /// 範例說明:
    /// 延續Class Adapter的範例，假設今天有新需求需要多轉接儲值單系統
    /// 而因Class Adapter無法多重繼承造成擴充不易的缺點，故由Object Adapter來實現。
    ///
    /// 優點:
    /// 由於Adaptee是透過建構子傳入，所以解決了Class Adpater要擴充功能時無法多重繼承的缺點。
    /// </summary>
    public class Object_Adapter : IExecute
    {
        public void main()
        {
            Console.WriteLine("Object Adapter\n");

            OrderSystem orderSystem = new OrderSystem();
            orderSystem.AddOrder("Leo", 150, "USD");
            orderSystem.AddOrder("Wayne", 400, "HKD");
            orderSystem.AddOrder("Mary", 3000, "JPY");

            DepositSystem depositSystem = new DepositSystem();
            depositSystem.AddDeposit("Jimmy", 100, "USD");
            depositSystem.AddDeposit("Tom", 500, "HKD");
            depositSystem.AddDeposit("Jason", 1500, "JPY");

            OrderCurrencyAdapter targetAdapter = new OrderCurrencyAdapter(orderSystem, depositSystem);

            ThirdPartyBillingSystem billingSystem = new ThirdPartyBillingSystem(targetAdapter);
            billingSystem.ProcessOrderList();
            billingSystem.ProcessDepositList();
        }
    }

    public class Order
    {
        public string Purchaser { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }

    public class Deposit
    {
        public string Depositor { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }

    /// <summary>
    /// The 'ITarget' interface
    /// </summary>
    public interface ITarget
    {
        List<Order> GetBillList();

        List<Deposit> GetDepositList();
    }

    /// <summary>
    /// The 'Client' class
    /// 必須用台幣付款
    /// </summary>
    public class ThirdPartyBillingSystem
    {
        private ITarget orderSystem;

        public ThirdPartyBillingSystem(ITarget orderSystem)
        {
            this.orderSystem = orderSystem;
        }

        public void ProcessOrderList()
        {
            List<Order> billList = orderSystem.GetBillList();
            //To DO: Implement you business logic

            Console.WriteLine("\n----- Bill List Process -----");
            foreach (var bill in billList)
                Console.Write($"處理訂購人:{bill.Purchaser}的訂單，金額為:{bill.Currency} {bill.Amount}\n");
        }

        public void ProcessDepositList()
        {
            List<Deposit> depositList = orderSystem.GetDepositList();
            //To DO: Implement you business logic

            Console.WriteLine("\n----- Deposit List Process -----");
            foreach (var bill in depositList)
                Console.Write($"處理儲值者:{bill.Depositor}的儲值單，金額為:{bill.Currency} {bill.Amount}\n");
        }
    }

    /// <summary>
    /// The 'Adaptee1' class
    /// OrderSystem 輸入的貨幣單位不一致
    /// </summary>
    public class OrderSystem
    {
        private List<Order> billList { get; set; } = new List<Order>();

        public void AddOrder(string purchaser, double amount, string currency)
        {
            billList.Add(new Order() { Purchaser = purchaser, Amount = amount, Currency = currency });
            Console.WriteLine($"訂購人:{purchaser}的訂單，金額為:{currency} {amount}");
        }

        public List<Order> GetOriginalBillList()
        {
            return billList;
        }
    }

    /// <summary>
    /// The 'Adaptee2' class
    /// DepositSystem 輸入的貨幣單位不一致
    /// </summary>
    public class DepositSystem
    {
        private List<Deposit> depositList { get; set; } = new List<Deposit>();

        public void AddDeposit(string depositor, double amount, string currency)
        {
            depositList.Add(new Deposit() { Depositor = depositor, Amount = amount, Currency = currency });
            Console.WriteLine($"儲值者:{depositor}的訂單，金額為:{currency} {amount}");
        }

        public List<Deposit> GetOriginalDepositList()
        {
            return depositList;
        }
    }

    /// <summary>
    /// The 'Adapter' class
    /// 因為貨幣單位不一致，使得金額無法計算，由此類別統一轉換為新台幣
    /// </summary>
    public class OrderCurrencyAdapter : ITarget
    {
        private OrderSystem _orderSystem;
        private DepositSystem _depositSystem;

        public OrderCurrencyAdapter(OrderSystem orderSystem, DepositSystem depositSystem)
        {
            _orderSystem = orderSystem;
            _depositSystem = depositSystem;
        }

        public List<Order> GetBillList()
        {
            List<Order> orderList = new List<Order>();
            _orderSystem.GetOriginalBillList().ForEach(x =>
            {
                x.Amount = x switch
                {
                    { Currency: "USD" } => x.Amount * 29.515,
                    { Currency: "HKD" } => x.Amount * 3.703,
                    { Currency: "JPY" } => x.Amount * 0.2654,
                    _ => throw new NotImplementedException(),
                };

                x.Currency = "TWD";

                orderList.Add(x);
            });

            return orderList;
        }

        public List<Deposit> GetDepositList()
        {
            List<Deposit> depositList = new List<Deposit>();
            _depositSystem.GetOriginalDepositList().ForEach(x =>
            {
                x.Amount = x switch
                {
                    { Currency: "USD" } => x.Amount * 29.515,
                    { Currency: "HKD" } => x.Amount * 3.703,
                    { Currency: "JPY" } => x.Amount * 0.2654,
                    _ => throw new NotImplementedException(),
                };

                x.Currency = "TWD";

                depositList.Add(x);
            });

            return depositList;
        }
    }
}