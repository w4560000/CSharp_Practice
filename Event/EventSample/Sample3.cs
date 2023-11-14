using System;

namespace EventSample.Sapmle3
{
    /// <summary>
    /// 原始的 delegate 用法
    /// </summary>
    internal class Sample3
    {
        public void Run()
        {
            Boss boss = new Boss();
            BossArgs bossEventArgs = new BossArgs()
            {
                Message = "Boss called"
            };

            // 註冊 Fans 執行命令 到 Boss Event 中
            new Fans().Register(boss);

            // 註冊 Employees 執行命令 到 Boss Event 中
            new Employees().Register(boss);

            // Boss 執行呼叫動作
            boss.OnEventCall(bossEventArgs);
        }
    }

    public class Boss
    {
        public delegate void BossHandler(BossArgs args);
        public BossHandler OnBossHandler;

        public void OnEventCall(BossArgs e)
        {
            Console.WriteLine("boss call everyone.");
            //通知執行所有已註冊在Boss類別的EventHandler的方法
            OnBossHandler(e);
        }
    }

    /// <summary>
    /// 當要客製化 EventArgs，繼承 EventArgs 再自行實作
    /// </summary>
    public class BossArgs
    {
        public string Message { get; set; }
    }

    public class Fans
    {
        public void Register(Boss boss)
        {
            //將 FansSayHello 註冊到 Boss 類別的 EventHandler
            boss.OnBossHandler += this.FansSayHello;
        }

        private void FansSayHello(BossArgs e)
        {
            string msg = e.Message;
            Console.WriteLine($"boss request fans: {msg}, fans response: I am a big fan.");
        }
    }

    public class Employees
    {
        public void Register(Boss boss)
        {
            //將 EmployeesSayHello 註冊到 Boss 類別的 EventHandler
            boss.OnBossHandler += this.EmployeesSayHello;
        }

        private void EmployeesSayHello(BossArgs e)
        {
            string msg = e.Message;
            Console.WriteLine($"boss request employees: {msg}, employees response: I am a Employee.");
        }
    }
}