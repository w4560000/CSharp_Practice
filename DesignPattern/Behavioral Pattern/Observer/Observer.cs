using CommonClassLibary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPattern.Observer
{
    /// <summary>
    /// 定義:
    /// 定義一對多的依賴關係，當被訂閱者狀態發生變化，而有訂閱的觀察者會收到訊息依序觸發動作
    ///
    /// 角色:
    /// 1. Subject : 定義操作觀察者的方法
    ///              在此又切出一層泛型類別 直接繼承.Net提供的介面 [IObservable<T>]  用以實作增加訂閱的方法 [ObservableService<T>]
    ///              並實作取消訂閱類別 [UnSubscrible<T>]
    /// 2. Observer : 定義觀察者訂閱後提供給被訂閱者呼叫的方法 直接使用.Net提供的介面 [IObserver<T>]
    /// 3. ConcreteSubject : 實作訂閱動作，並繼承Subject [Channel]
    /// 4. ConcreteObserver : 實作Observer定義的方法，以供被訂閱者呼叫 [ObserverFirst] [ObserverSecond]
    ///
    /// 優點:
    /// 適合廣播推送訊息，介面定義清楚後，使用上很直覺
    /// 
    /// 缺點:
    /// 若程式建立實體是使用DI注入方式建立，需要小心循環依賴問題
    /// 比如: 若Channel 以DI注入方式 建立 ObserverFirst or ObserverSecond，則會產生循環依賴問題
    ///       因ObserverFirst or ObserverSecond 注入了 Channel， 而 Channel 又注入了ObserverFirst or ObserverSecond，使得注入不斷循環
    /// 
    /// 參考來源: https://docs.microsoft.com/zh-tw/dotnet/standard/events/how-to-implement-an-observer
    /// </summary>
    public class Observer : IExecute
    {
        public void Main()
        {
            Console.WriteLine("Start:\n");

            // 建立訂閱頻道
            Channel channel = new Channel();

            // 訂閱頻道被兩個觀察者訂閱
            IDisposable observerFirstSubscribe = channel.Subscribe(new ObserverFirst());
            IDisposable observerSecondSubscribe = channel.Subscribe(new ObserverSecond());

            // 觀察者訂閱後 -> 查看頻道的訂閱數 -> 應為2
            channel.CheckSubscribeCount();

            // 訂閱頻道發送訊息後 -> 觀察者將會收到訊息
            channel.Publish(new Dto() { dataString = "data" });

            Console.WriteLine("");

            // 訂閱頻道被兩個觀察者取消訂閱
            observerFirstSubscribe.Dispose();
            observerSecondSubscribe.Dispose();

            // 觀察者取消訂閱後 -> 查看頻道的訂閱數 -> 應為0
            channel.CheckSubscribeCount();
        }
    }

    public class Dto
    {
        public string dataString { get; set; }
    }

    /// <summary>
    /// 訂閱頻道 (ConcreteSubject)
    /// </summary>
    public class Channel : ObservableService<Dto>
    {
        // 發送訊息
        public void Publish(Dto dto)
        {
            // 訂閱的集合跑foreach 做各自執行的事
            foreach (IObserver<Dto> observer in this.Observers)
            {
                try
                {
                    observer.OnNext(dto);
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            }
        }

        public void CheckSubscribeCount()
        {
            Console.WriteLine($"Channel 被訂閱的數量: {this.Observers.Count()}\n");
        }
    }

    /// <summary>
    /// 觀察者一號 (ConcreteObserver)
    /// </summary>
    public class ObserverFirst : IObserver<Dto>
    {
        // 若有跑DI 則可在construct訂閱
        //	public ObserverFirst(IChannel channel)
        //	{
        //		// 訂閱channel Class
        //		channel.Subscribe(this);
        //	}

        // 提供新的資料給觀察器。
        public void OnNext(Dto dto)
        {
            Console.WriteLine($"FirstObserver received {dto.dataString}!");
        }

        // 向觀察器告知提供者已發生錯誤狀況。
        public void OnError(Exception error)
        {
            Console.WriteLine($"FirstObserver got error!");
        }

        // 向觀察器告知提供者已完成推入型通知的傳送。
        public void OnCompleted()
        {
            Console.WriteLine($"FirstObserver received finish!");
        }
    }

    /// <summary>
    /// 觀察者二號 (ConcreteObserver)
    /// </summary>
    public class ObserverSecond : IObserver<Dto>
    {
        // 提供新的資料給觀察器。
        public void OnNext(Dto dto)
        {
            Console.WriteLine($"SecondObserver received {dto.dataString}");
        }

        // 向觀察器告知提供者已發生錯誤狀況。
        public void OnError(Exception error)
        {
            Console.WriteLine($"SecondObserver got error!");
        }

        // 向觀察器告知提供者已完成推入型通知的傳送。
        public void OnCompleted()
        {
            Console.WriteLine($"SecondObserver received finish!");
        }
    }

    /// <summary>
    /// 取消訂閱泛型類別
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnSubscrible<T> : IDisposable
    {
        // The subscriber would be removed when user call dispose.
        private readonly IObserver<T> Observer;

        // The subscribers collection
        private readonly List<IObserver<T>> Observers;

        // construct
        public UnSubscrible(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this.Observers = observers;
            this.Observer = observer;
        }

        public void Dispose()
        {
            if (Observer != null && Observers.Contains(Observer))
            {
                Observers.Remove(Observer);
                Console.WriteLine($"{Observer.GetType().Name} UnSubscribed!");
            }
        }
    }

    /// <summary>
    /// 訂閱泛型類別 (Subject)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableService<T> : IObservable<T>
    {
        // The subscribers collection
        protected readonly List<IObserver<T>> Observers = new List<IObserver<T>>();

        // subscribe
        public IDisposable Subscribe(IObserver<T> observer)
        {
            // 加到現有已訂閱集合裡
            if (!Observers.Contains(observer))
            {
                this.Observers.Add(observer);
                Console.WriteLine($"{observer.GetType().Name} Subscribled!");
            }

            return new UnSubscrible<T>(this.Observers, observer);
        }
    }
}