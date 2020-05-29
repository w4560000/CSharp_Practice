using System;

namespace AOP
{
    public class ActionAttribute : ActionBaseAttribute
    {
        public override void Before(string method, object[] parameters)
        {
            Console.WriteLine($"Action Before 1 : {method} , parameters: {parameters}");
        }

        public override object After(string method, object result)
        {
            Console.WriteLine($"Action After 1 : {method} , parameters: {result}");

            return result;
        }
    }
}