using System;
using System.Collections.Generic;
using CommonClassLibary;

namespace DesignPattern.Transparency_Composite
{
    /// <summary>
    /// 透明的組合模式
    ///
    /// 定義:
    /// 將對象組織到樹結構中，可以用來描述整體與部分的關係。合成模式可以使客戶端將單純元素與複合元素同等看待。
    ///
    /// 角色:
    /// 1. Component : 定義抽象動作
    /// 2. Composite : 繼承至Component，負責處理階層
    /// 3. Leaf : 繼承至Component
    ///
    /// 缺點:
    /// 透明的組合模式由於Component定義了處理階層Method(Add、Remove)，使得Leaf需要額外threw new Exception來避免意外新增。
    /// 若Leaf意外新增了，compile時不會發現，只能等到runtime時噴錯才知道。
    /// </summary>
    public class Transparency_Composite : IExecute
    {
        public void Main()
        {
            ComplexGraphics complexGraphics = new ComplexGraphics("Graphics");

            ComplexGraphics complexGraphics1 = new ComplexGraphics("二條線和一個圓圈組成的複雜圖形");
            complexGraphics1.Add(new Line());
            complexGraphics1.Add(new Line());
            complexGraphics1.Add(new Circle());

            ComplexGraphics complexGraphics2 = new ComplexGraphics("一條線和三個圓圈組成的複雜圖形");
            complexGraphics2.Add(new Line());
            complexGraphics2.Add(new Circle());
            complexGraphics2.Add(new Circle());
            complexGraphics2.Add(new Circle());

            ComplexGraphics complexGraphics3 = new ComplexGraphics("第三階層圖形");
            complexGraphics3.Add(new Line());

            complexGraphics1.Add(complexGraphics3);
            complexGraphics.Add(complexGraphics1);
            complexGraphics.Add(complexGraphics2);

            complexGraphics.Draw(0);
        }

        /// <summary>
        /// Component
        /// </summary>
        private abstract class Graphics
        {
            protected string name;

            public Graphics(string name)
            {
                this.name = name;
            }

            // 繪出圖像
            public abstract void Draw(int depth);

            public abstract void Add(Graphics m);

            public abstract void Remove(Graphics m);
        }

        /// <summary>
        /// Composite
        /// </summary>
        private class ComplexGraphics : Graphics
        {
            private List<Graphics> menu = new List<Graphics>();

            public ComplexGraphics(string name)
                : base(name)
            {
            }

            // 新增
            public override void Add(Graphics m)
            {
                menu.Add(m);
            }

            // 移除
            public override void Remove(Graphics m)
            {
                menu.Remove(m);
            }

            // *代表階層
            public override void Draw(int depth)
            {
                Console.WriteLine($"{new string('-', depth)}*{name}");

                foreach (Graphics m in menu)
                    m.Draw(depth + 5);
            }
        }

        /// <summary>
        /// Leaf
        /// </summary>
        private class Line : Graphics
        {
            public Line(string name = "線") : base(name)
            {
            }

            public override void Draw(int depth)
            {
                Console.WriteLine($"{new string('-', depth)}畫出{name}");
            }

            public override void Add(Graphics m)
            {
                throw new ArgumentNullException("Leaf can't add child node");
            }

            // 移除
            public override void Remove(Graphics m)
            {
                throw new ArgumentNullException("Leaf can't remove child node");
            }
        }

        /// <summary>
        /// Leaf
        /// </summary>
        private class Circle : Graphics
        {
            public Circle(string name = "圓圈") : base(name)
            {
            }

            public override void Draw(int depth)
            {
                Console.WriteLine($"{new string('-', depth)}畫出{name}");
            }

            public override void Add(Graphics m)
            {
                throw new ArgumentNullException("Leaf can't add child node");
            }

            // 移除
            public override void Remove(Graphics m)
            {
                throw new ArgumentNullException("Leaf can't remove child node");
            }
        }
    }
}