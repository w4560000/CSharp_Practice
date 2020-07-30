using System;
using System.Collections.Generic;
using CommonClassLibary;

namespace DesignPattern.Safety_Composite
{
    /// <summary>
    /// 安全組合模式
    /// 
    /// 與透明組合模式差別在
    /// Component不定義處理階層方法(Add、Remove)，而是讓Composite自己處理
    /// 這是優點也是缺點
    /// 
    /// 優點是:
    /// Leaf不用再被強迫實作處理階層方法，也就沒有噴Exception問題。
    /// 
    /// 缺點是:
    /// Composite需要自行處理階層方法，而若有多個不同Composite時，有可能造成處理階層方法簽章不一致問題
    /// 此時再加上一個Interface來進行規範即可
    /// </summary>
    public class Safety_Composite : IExecute
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

            // 顯示資料
            public abstract void Draw(int depth);
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
            public void Add(Graphics m)
            {
                menu.Add(m);
            }

            // 移除
            public void Remove(Graphics m)
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
        }
    }
}