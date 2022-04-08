using CommonClassLibary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            TestClass a = new TestClass() { Id = 1, IdList = new List<int>() { 1, 2, 3 } };
            TestClass shallowCopyObj = PrototypeHelper.ShallowCopy(a);
            TestClass deepCopyObj = PrototypeHelper.DeepCopy(a);

            a.Id = 10;
            a.IdList.Clear();
            Console.WriteLine($"a={JsonConvert.SerializeObject(a)}");
            Console.WriteLine($"shallowCopyObj={JsonConvert.SerializeObject(shallowCopyObj)}");
            Console.WriteLine($"deepCopyObj={JsonConvert.SerializeObject(deepCopyObj)}");
        }
    }

    [Serializable]
    internal class TestClass
    {
        public int Id { get; set; }
        public List<int> IdList { get; set; }
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