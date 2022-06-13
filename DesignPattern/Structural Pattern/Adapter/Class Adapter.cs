using CommonClassLibary;
using System;
using System.Collections.Generic;

namespace DesignPattern.Class_Adapter
{
    /// <summary>
    /// 定義:
    /// 透過一個轉接類別，使得兩個不相容的類別正常運作
    ///
    /// 角色:
    /// 1. Target : 定義轉接動作 [ITarget]
    /// 2. Client : 定義使用不相容系統的使用方 [ThirdPartyBillingSystem]
    /// 3. Adaptee : 定義需要被轉接的不相容系統 [OrderSystem]
    /// 4. Adapter : 定義把Adptee(不相容系統)轉接成符合Client需求，繼承Interface Target，才能清楚明白轉接的動作 [OrderCurrencyAdapter]
    ///
    /// 範例說明:
    /// 假設目前有一個訂單系統，全球都可以使用，但因為使用者輸入的貨幣前端沒有處理好
    /// 送到後端時，貨幣單位沒有統整，無法計算金額
    /// 所以透過一個中介轉接類別，統一轉換各國貨幣單位為台幣，讓訂單系統正常計算金額。
    ///
    /// 缺點:
    /// 若Adapter必須要轉接多個類別時，因為CSharp語言無法多重繼承，而造成擴充不易，而Object Adapter可以解決。
    /// </summary>
    public class Class_Adapter : IExecute
    {
        public void Main()
        {
            Console.WriteLine("Class Adapter\n");

            OrderCurrencyAdapter targetAdapter = new OrderCurrencyAdapter();
            targetAdapter.AddOrder("Leo", 150, "USD");
            targetAdapter.AddOrder("Wayne", 400, "HKD");
            targetAdapter.AddOrder("Mary", 3000, "JPY");

            ThirdPartyBillingSystem billingSystem = new ThirdPartyBillingSystem(targetAdapter);
            billingSystem.ProcessOrderList();
        }
    }

    public class Order
    {
        public string Purchaser { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    }

    /// <summary>
    /// 定義轉接動作 (ITarget)
    /// </summary>
    public interface ITarget
    {
        List<Order> GetBillList();
    }

    /// <summary>
    /// 第三方支付系統(限定用台幣付款) (Client)
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
    }

    /// <summary>
    /// orderSystem 輸入的貨幣單位不一致 (Adaptee)
    /// </summary>
    public class OrderSystem
    {
        public List<Order> BillList { get; set; } = new List<Order>();

        public void AddOrder(string purchaser, double amount, string currency)
        {
            BillList.Add(new Order() { Purchaser = purchaser, Amount = amount, Currency = currency });
            Console.WriteLine($"訂購人:{purchaser}的訂單，金額為:{currency} {amount}");
        }
    }

    /// <summary>
    /// 因為貨幣單位不一致，使得金額無法計算，由此類別統一轉換為新台幣 (Adapter)
    /// </summary>
    public class OrderCurrencyAdapter : OrderSystem, ITarget
    {
        public List<Order> GetBillList()
        {
            BillList.ForEach(x =>
            {
                x.Amount = x switch
                {
                    { Currency: "USD" } => x.Amount * 29.515,
                    { Currency: "HKD" } => x.Amount * 3.703,
                    { Currency: "JPY" } => x.Amount * 0.2654,
                    _ => throw new NotImplementedException(),
                };

                x.Currency = "TWD";
            });

            return BillList;
        }
    }
}