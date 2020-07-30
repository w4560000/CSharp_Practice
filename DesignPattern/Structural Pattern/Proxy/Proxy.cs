using System;
using CommonClassLibary;

namespace DesignPattern.Proxy
{
    /// <summary>
    /// 定義:
    /// 提供RealSubject一個可以代理的類別，接口完成相同，由操作Proxy類別，進而操作到RealSubject
    ///
    /// 角色:
    /// Subject: 定義出代理對象的抽象方法 [IMath]
    /// RealSubject: 被代理的對象，是真實存在的類別並繼承Subject的方法 [Math]
    /// Proxy: 代理RealSubject的類別，繼承Subject的方法 [MathProxy]
    ///
    /// 優點:
    /// 遠端代理的需求: 當RealSubject可能在另一台主機或是其他因素可導致無法直接存取時，可透過Proxy代替發送Http Request取得資料
    /// 並透過相同的方法簽章，讓使用者順利存取
    /// 還有多種情況，如: 虛擬代理 ...
    /// 
    /// 缺點:
    /// 都建立了一個代理模式會連帶產生許多類別，若需要產生大量代理模式，則建立的類別會十分繁瑣，可以使用Dynamic Proxy動態的來建立(todo)
    /// 
    /// 範例來源:
    /// https://blog.csdn.net/zhongguoren666/article/details/6770980
    /// </summary>
    public class Proxy : IExecute
    {
        public void Main()
        {
            // Create math proxy.
            var proxy = new MathProxy();

            // Do the math.
            Console.WriteLine("4 + 2 = " + proxy.Add(4, 2));
            Console.WriteLine("4 - 2 = " + proxy.Sub(4, 2));
            Console.WriteLine("4 * 2 = " + proxy.Mul(4, 2));
            Console.WriteLine("4 / 2 = " + proxy.Div(4, 2));
        }
    }

    /// <summary>
    /// (Subject)
    /// </summary>
    public interface IMath
    {
        double Add(double x, double y);

        double Div(double x, double y);

        double Mul(double x, double y);

        double Sub(double x, double y);
    }

    /// <summary>
    /// (RealSubject)
    /// </summary>
    internal class Math : IMath
    {
        public double Add(double x, double y)
        {
            return x + y;
        }

        public double Div(double x, double y)
        {
            return x / y;
        }

        public double Mul(double x, double y)
        {
            return x * y;
        }

        public double Sub(double x, double y)
        {
            return x - y;
        }
    }

    /// <summary>
    /// (Proxy)
    /// </summary>
    internal class MathProxy : IMath
    {
        private readonly Math _math = new Math();

        public double Add(double x, double y)
        {
            return _math.Add(x, y);
        }

        public double Div(double x, double y)
        {
            return _math.Div(x, y);
        }

        public double Mul(double x, double y)
        {
            return _math.Mul(x, y);
        }

        public double Sub(double x, double y)
        {
            return this._math.Sub(x, y);
        }
    }
}