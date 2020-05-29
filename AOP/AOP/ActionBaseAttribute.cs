using System;

namespace AOP
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ActionBaseAttribute : Attribute
    {
        public abstract void Before(string @method, object[] parameters);

        public abstract object After(string @method, object result);
    }
}