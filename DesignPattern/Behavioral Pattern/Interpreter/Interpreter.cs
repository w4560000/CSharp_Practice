using System;
using System.Collections.Generic;
using CommonClassLibary;

namespace DesignPattern.Interpreter
{
    /// <summary>
    /// 定義:
    /// 實作出解譯器用以解析各種客製化規則語法或是語言
    ///
    /// 角色:
    /// AbstractExpression: 定義抽象的解析方法的抽象類別或是介面 [Expression]
    /// TerminalExpression: 終端表達式用以處理各個句子中的符號
    ///                     實作AbstractExpression的解析方法 [TerminalExpression]
    /// NonterminalExpression: 非終端表達式用以解析各個句子中所要轉譯方法
    ///                        實作AbstractExpression的解析方法 [RomanNumberExpression] [ChineseNumberExpression]
    /// Context: 定義轉譯文字的物件 [Context]
    ///
    /// 缺點:
    /// 應用場合不多，一班普通的商業邏輯用不到
    /// </summary>
    public class Interpreter : IExecute
    {
        public void Main()
        {
            // 待解譯文字
            Context context = new Context
            {
                Text = "aMCMXCIV b三千二百一十五"
            };

            Console.WriteLine(new TerminalExpression().Interpret(context.Text));
            Console.Read();
        }
    }

    /// <summary>
    /// 解譯器抽像類別 (AbstractExpression)
    /// </summary>
    public abstract class Expression
    {
        public abstract string Interpret(string text);
    }

    /// <summary>
    /// 處理符號解譯器類別 (TerminalExpression)
    /// </summary>
    public class TerminalExpression : Expression
    {
        public override string Interpret(string text)
        {
            // 解譯器
            string[] textArray = text.Split(' ');
            string input = string.Empty;
            string output = "0";

            for (int i = 0; i < textArray.Length; i++)
            {
                input = textArray[i][1..];
                switch (textArray[i].Substring(0, 1))
                {
                    case "a":
                        output = new RomanNumberExpression().Interpret(input);
                        textArray[i] = $"羅馬數字:{input}";
                        break;

                    case "b":
                        output = new ChineseNumberExpression().Interpret(input);
                        textArray[i] = $"中文數字:{input}";
                        break;
                }

                textArray[i] += output == "0" ? " 解譯錯誤" : $" ={output}";
            }

            return string.Join('\n', textArray);
        }
    }

    /// <summary>
    /// 羅馬數字轉譯器 無error防呆 (NonterminalExpression)
    /// </summary>
    public class RomanNumberExpression : Expression
    {
        private Dictionary<char, int> dictionary = new Dictionary<char, int>()
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        public override string Interpret(string romanNumber)
        {
            int number = 0;

            for (int i = 0; i < romanNumber.Length; i++)
            {
                if (i + 1 < romanNumber.Length && dictionary[romanNumber[i]] < dictionary[romanNumber[i + 1]])
                {
                    number -= dictionary[romanNumber[i]];
                }
                else
                {
                    number += dictionary[romanNumber[i]];
                }
            }

            return number.ToString();
        }
    }

    /// <summary>
    /// 中文數字轉譯器 (NonterminalExpression)
    ///
    /// sourceCode來源: http://readily-notes.blogspot.com/2014/03/c.html
    /// </summary>
    internal class ChineseNumberExpression : Expression
    {
        private Dictionary<string, long> digit = new Dictionary<string, long>()
        {
            { "一", 1 },
            { "二", 2 },
            { "三", 3 },
            { "四", 4 },
            { "五", 5 },
            { "六", 6 },
            { "七", 7 },
            { "八", 8 },
            { "九", 9 }
        };

        private Dictionary<string, long> word = new Dictionary<string, long>()
        {
            { "百", 100 },
            { "千", 1000 },
            { "萬", 10000 },
            { "億", 100000000 },
            { "兆", 1000000000000 }
        };

        private Dictionary<string, long> ten = new Dictionary<string, long>()
        {
            { "十", 10 }
        };

        public override string Interpret(string chineseNumber)
        {
            long iResult = 0;

            chineseNumber = chineseNumber.Replace("零", "");
            int index = 0;
            long t_l = 0, _t_l = 0;
            string t_s;

            while (chineseNumber.Length > index)
            {
                t_s = chineseNumber.Substring(index++, 1);

                // 數字
                if (digit.ContainsKey(t_s))
                {
                    _t_l += digit[t_s];
                }
                // 十
                else if (ten.ContainsKey(t_s))
                {
                    _t_l = _t_l == 0 ? 10 : _t_l * 10;
                }
                // 百、千、億、兆
                else if (word.ContainsKey(t_s))
                {
                    // 碰到千位則使 _t_l 與 t_l 相加乘上目前讀到的數字，
                    // 並將輸出結果累加。
                    if (word[t_s] > word["千"])
                    {
                        iResult += (t_l + _t_l) * word[t_s];
                        t_l = 0;
                        _t_l = 0;

                        continue;
                    }
                    _t_l = _t_l * word[t_s];
                    t_l += _t_l;

                    _t_l = 0;
                }
            }
            // 將殘餘值累加至輸出結果
            iResult += t_l;
            iResult += _t_l;

            return iResult.ToString();
        }
    }

    // 存放待解譯資料 (Context)
    public class Context
    {
        public string Text { get; set; }
    }
}