﻿using CommonClassLibary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DesignPattern.Prototype
{
    /// <summary>
    /// 定義:
    /// 使用原型實例指定創建對象的種類，然後通過拷貝這些原型來創建新的對象
    ///
    /// 角色:
    /// 1. Prototype:定義一個抽象方法 Clone [PrototypeBase]
    /// 2. ConcretePrototype: 實現Clone方法 [ConcretePrototype1]
    ///
    /// 缺點:
    /// 1.當有大量子類別需要Clone時，實現Clone的子類別也要各寫一個
    /// 2.Clone有分淺複製和深複製，.Net Framework提供的ICloneable 只有一個Clone方法
    ///   無法明確定義出該Clone是淺複製還是深複製
    /// </summary>
    public class Prototype : IExecute
    {
        public void Main()
        {
            ConcretePrototype1 obj = new ConcretePrototype1() { Id = 1, IdList = new List<int>() { 1, 2, 3 } };
            ConcretePrototype1 copyObj = (ConcretePrototype1)obj.Clone();

            obj.Id = 10;
            obj.IdList.Clear();
            Console.WriteLine($"obj={JsonConvert.SerializeObject(obj)}");
            Console.WriteLine($"copyObj={JsonConvert.SerializeObject(copyObj)}");
        }
    }

    /// <summary>
    /// (Prototype)
    /// </summary>
    public abstract class PrototypeBase : ICloneable
    {
        // Methods
        public abstract object Clone();
    }

    /// <summary>
    /// (ConcretePrototype)
    /// </summary>
    public class ConcretePrototype1 : PrototypeBase
    {
        public int Id { get; set; }

        public List<int> IdList { get; set; }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}