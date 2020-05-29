using System;

namespace Common
{
    public class ProgramExtension
    {
        public static void EnterMethod<T>(Type type,T testMethod)
        {
            type.GetMethod(testMethod.ToString()).Invoke(null, new object[] { });
            Console.ReadLine();
        }
    }
}
