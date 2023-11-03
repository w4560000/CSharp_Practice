using System;

namespace EventSample
{
    /// <summary>
    /// SourceCode: https://jeffprogrammer.wordpress.com/2015/07/29/%E8%A7%80%E5%BF%B5-c-eventhandler/
    ///
    /// Event 是一種封裝過的委託
    /// 有三點:
    /// 1. 事件發行者 [Boss]
    /// 2. 事件訂閱者 [Fans, Employees]
    /// 3. 定義發行者和訂閱者關係，一個發行者可以有多個訂閱者 Event 可以用 += 的方式疊加事件
    /// </summary>
    internal class Sample2
    {
        public void Run()
        {
            Boss boss = new Boss();
            BossEventArgs bossEventArgs = new BossEventArgs()
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
        public event EventHandler BossEventHandler;

        public void OnEventCall(BossEventArgs e)
        {
            Console.WriteLine("boss call everyone.");
            //通知執行所有已註冊在Boss類別的EventHandler的方法
            BossEventHandler(this, e);
        }
    }

    /// <summary>
    /// 當要客製化 EventArgs，繼承 EventArgs 再自行實作
    /// </summary>
    public class BossEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class Fans
    {
        public void Register(Boss boss)
        {
            //將 FansSayHello 註冊到 Boss 類別的 EventHandler
            boss.BossEventHandler += new EventHandler(this.FansSayHello);
        }

        private void FansSayHello(object sender, EventArgs e)
        {
            string msg = (e as BossEventArgs).Message;
            Console.WriteLine($"boss request fans: {msg}, fans response: I am a big fan.");
        }
    }

    public class Employees
    {
        public void Register(Boss boss)
        {
            //將 EmployeesSayHello 註冊到 Boss 類別的 EventHandler
            boss.BossEventHandler += new EventHandler(this.EmployeesSayHello);
        }

        private void EmployeesSayHello(object sender, EventArgs e)
        {
            string msg = (e as BossEventArgs).Message;
            Console.WriteLine($"boss request employees: {msg}, employees response: I am a Employee.");
        }
    }
}