using CommonClassLibary;
using System;
using System.IO;
using System.Reflection;
using CommonClassLibary;

namespace DesignPattern.Generic_Prototype
{
    /// <summary>
    /// Prototype泛型改良版
    /// 
    /// 以泛型解決Prototype的缺點
    /// 並把Clone定義分為深複製與淺複製以免誤用
    /// </summary>
    public class Generic_Prototype : IExecute
    {
        public void Main()
        {
            TestClass a = new TestClass() { Id = 1 };
            TestClass shallowCopyObj = PrototypeHelper.ShallowCopy(a);
            TestClass deepCopyObj = PrototypeHelper.DeepCopy(a);

            a.Dump();
            shallowCopyObj.Dump();
            deepCopyObj.Dump();
        }
    }

    [Serializable]
    internal class TestClass
    {
        public int Id { get; set; }
    }

    public static class PrototypeHelper
    {
        // 淺複製
        public static T ShallowCopy<T>(this T targetObject)
        {
            if ((object)targetObject is null)
                throw new ArgumentNullException();

            return (T)targetObject.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(targetObject, null);
        }

        // 深複製
        public static T DeepCopy<T>(this T targetObject)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(memory, targetObject);
                memory.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(memory);
            }
        }
    }
}