using CommonClassLibary;
using System;

namespace DesignPattern.Memento
{
    /// <summary>
    /// 定義:
    /// 在不破壞&修改原有封裝類別的前題下，暫存類別的內部狀態，並將狀態保存在該類別之外(以避免記憶體重複參考)，當需要復原狀態時可以將暫存資料倒回該類別之中。
    ///
    /// 角色:
    /// 1. Originator : 作為被復原資料的類別角色，該類別必須自己提供儲存資料與載入資料的方法已供復原資料 [Player]
    /// 2. Memento : 作為暫存資料的類別 [MementoClass]
    /// 3. Caretaker : 負責保存被暫存起來的Memento類別，該類別不可自行操作資料，也無需得知資料型態..等等細節
    ///                只需要保存資料即可，在該範例中只有提供一個暫存，也可改為多個暫存，以Dictionary or List 或是其他物件容器來儲存 [Caretaker]
    ///
    /// 優點:
    /// 提供了狀態恢復的機制，可以很方便地回到指定的時間線還原
    ///
    /// 缺點:
    /// 資源消耗過大，因暫存一次就會建立一個新的實體類別出來暫存，還需要規範暫存的移除機制，例如暫存次數或是暫存資料有效時間
    /// 若超過暫存次數則不予以儲存 & 若暫存資料超過有效期限 則呼叫GC.SuppressFinalize 銷毀自己 以免造成記憶體負擔
    ///
    /// 範例來源: https://xyz.cinc.biz/2013/07/memento-pattern.html
    /// </summary>
    public class Memento : IExecute
    {
        public void Main()
        {
            Player aa = new Player();
            ShowHealthInfo(aa, "初始狀態");

            // 新增資料管理者
            Caretaker aaCaretaker = new Caretaker
            {
                PlayerMemento = aa.saveToMemento() // 儲存目前狀態
            };

            aa.run(-80, 125, "打Boss地點"); // 戰鬥，HP、EXP 改變
            ShowHealthInfo(aa, "戰鬥後");

            aa.loadFromMemento(aaCaretaker.PlayerMemento); // 回復到戰鬥前的狀態
            ShowHealthInfo(aa, "回復到戰鬥前狀態");
        }

        private void ShowHealthInfo(Player player, string timeLine)
        {
            Console.WriteLine($"{Environment.NewLine}{timeLine}");
            Console.WriteLine($"HP : {player.hp}");
            Console.WriteLine($"EXP : {player.exp}");
            Console.WriteLine($"Place : {player.place}");
        }
    }

    /// <summary>
    /// 玩家 (Originator)
    /// </summary>
    internal class Player
    {
        // 儲存資料
        public MementoClass saveToMemento()
        {
            // 將要儲存的資料傳給 Memento 物件
            return (new MementoClass(hp, exp, place));
        }

        // 回復資料
        public void loadFromMemento(MementoClass m)
        {
            hp = m.hp;
            exp = m.exp;
            place = m.place;
        }

        // 體力、經驗值 增減
        public void run(int hp, int exp, string place)
        {
            this.hp += hp;
            this.exp += exp;
            this.place = place;
        }

        public int hp { get; set; } = 100;

        public int exp { get; set; } = 0;

        public string place { get; set; } = "城鎮";
    }

    /// <summary>
    /// 暫存資料類別 (Memento)
    /// </summary>
    internal class MementoClass
    {
        public MementoClass(int hp, int exp, string place)
        {
            this.hp = hp;
            this.exp = exp;
            this.place = place;
        }

        public int hp { get; set; }

        public int exp { get; set; }

        public string place { get; set; }
    }

    /// <summary>
    /// 資料管理者 (Caretaker)
    /// </summary>
    internal class Caretaker
    {
        public MementoClass PlayerMemento { get; set; }
    }
}