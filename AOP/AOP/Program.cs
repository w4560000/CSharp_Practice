using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest instance = DynamicProxy.CreateProxyOfRealize<ITest, Test>();

            instance.Test1("777");

            Console.ReadKey();
        }
    }

    public interface ITest
    {
        void Test1(string A);
    }

    public class Test : ITest
    {
        [Action]
        public void Test1(string A)
        {
            Console.WriteLine($"{A}Test");
        }
    }
}
