namespace TopicPractice.BlockQueue
{
    public class BlockQueue<T>
    {
        public readonly int SizeLimit = 0;

        private Queue<T> _inner_queue = null;

        private ManualResetEvent _enqueue_wait = null;

        private ManualResetEvent _dequeue_wait = null;

        /// <summary>
        /// SizeLimit = Queue 上限
        /// 
        /// ManualResetEvent 特性 有信號後，一路敞開
        /// </summary>
        /// <param name="sizeLimit"></param>
        public BlockQueue(int sizeLimit)
        {
            this.SizeLimit = sizeLimit;
            this._inner_queue = new Queue<T>(this.SizeLimit);
            this._enqueue_wait = new ManualResetEvent(false); // 初始 門上鎖
            this._dequeue_wait = new ManualResetEvent(false); // 初始 門上鎖
        }

        /// <summary>
        /// 塞入物件
        /// 
        /// 鎖住 _inner_queue
        /// 判斷 _inner_queue 筆數 < 總筆數 則可以新增
        /// => _inner_queue 新增筆數
        /// => _enqueue_wait.Reset() => 門上鎖
        /// => _dequeue_wait.Set() => 門打開 (讓 DeQueue 可以拉出物件)
        /// 
        /// 若超過 總筆數
        /// => _enqueue_wait.WaitOne() => 阻擋 => 不能新增 (呼叫方會等待)
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="InvalidCastException"></exception>
        public void EnQueue(T item)
        {
            if (this._IsShutdown == true) throw new InvalidCastException("Queue was shutdown. Enqueue was not allowed. ");

            while (true)
            {
                lock (this._inner_queue)
                {
                    if (this._inner_queue.Count < this.SizeLimit)
                    {
                        this._inner_queue.Enqueue(item);
                        this._enqueue_wait.Reset();
                        this._dequeue_wait.Set();
                        break;
                    }
                }
                this._enqueue_wait.WaitOne();
            }
        }

        /// <summary>
        /// 拉出物件
        /// 
        /// 鎖住 _inner_queue
        /// => 判斷 _inner_queue 筆數 > 0 則可以拉出
        /// => _inner_queue 拉出物件
        /// => _dequeue_wait.Reset() => 門上鎖
        /// => _enqueue_wait.Set() => 門打開 (讓 EnQueue 可以新增物件)
        /// 
        /// 若 _inner_queue 筆數 <= 0
        /// => _dequeue_wait.WaitOne() => 阻擋 => 不能拉出 (呼叫方會等待)
        /// 
        /// </summary>
        /// <returns></returns>
        public T DeQueue()
        {
            while (true)
            {
                if (this._IsShutdown == true)
                {
                    lock (this._inner_queue) return this._inner_queue.Dequeue();
                }

                lock (this._inner_queue)
                {
                    if (this._inner_queue.Count > 0)
                    {
                        T item = this._inner_queue.Dequeue();
                        this._dequeue_wait.Reset();
                        this._enqueue_wait.Set();
                        return item;
                    }
                }
                this._dequeue_wait.WaitOne();
            }
        }

        private bool _IsShutdown = false;

        public void Shutdown()
        {
            this._IsShutdown = true;
            this._dequeue_wait.Set();
        }
    }
}