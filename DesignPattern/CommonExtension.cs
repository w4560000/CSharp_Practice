using System;
using System.Reflection;

namespace DesignPattern
{
    public static class CommonExtension
    {
        public static void EnterMethod(this SwitchExecute switchExecute)
        {

            Type type = Assembly.GetExecutingAssembly()
                                .GetType($"{typeof(CommonExtension).Namespace}.{switchExecute}.{switchExecute}");

            type.GetMethod(type.GetInterface(nameof(IExecute)).GetMethods()[0].Name).Invoke(Activator.CreateInstance(type), new object[] { });

            Console.WriteLine("End");
            Console.ReadLine();
        }

        public static void Dump<T>(this T model)
        {
            foreach(PropertyInfo info in model.GetType().GetProperties())
                Console.WriteLine($"{info.Name} : {info.GetValue(model)}");

            Console.WriteLine("\nDump End\n");
        }
    }
}