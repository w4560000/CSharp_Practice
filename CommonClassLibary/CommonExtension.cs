using System;
using System.Reflection;

namespace CommonClassLibary
{
    public static class CommonExtension
    {
        public static void EnterMethod(this string switchExecute, string nameSpace)
        {

            Type type = Assembly.Load(nameSpace)
                                .GetType($"{nameSpace}.{switchExecute}.{switchExecute}");

            type.GetMethod(type.GetInterface(nameof(IExecute)).GetMethods()[0].Name).Invoke(Activator.CreateInstance(type), new object[] { });

            Console.WriteLine("\n\nEnd");
            Console.ReadLine();
        }

        public static void Dump<T>(this T model)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
                Console.WriteLine($"{info.Name} : {info.GetValue(model)}");

            Console.WriteLine("\nDump End\n");
        }

    }
}
