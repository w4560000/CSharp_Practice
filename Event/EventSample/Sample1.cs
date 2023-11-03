using System;

namespace EventSample
{
    /// <summary>
    /// SourceCode: https://ryanchen34057.github.io/2019/10/12/eventIntro/
    /// </summary>
    internal class Sample1
    {
        public void Run()
        {
            var worker = new Worker();
            worker.WorkPerformed += new EventHandler<WorkPerformedEventArgs>(Worker_WorkPerformed);
            worker.WorkCompleted += new EventHandler(Worker_WorkCompleted);
            worker.DoWork(8, WorkType.GenerateReports);
        }

        public static void Worker_WorkPerformed(object sender, WorkPerformedEventArgs e)
        {
            Console.WriteLine(e.Hours + " " + e.WorkType);
        }

        public static void Worker_WorkCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Work Completed!");
        }
    }

    public enum WorkType
    {
        GoToMeetings,
        Golf,
        GenerateReports
    }

    public class WorkPerformedEventArgs : System.EventArgs
    {
        public int Hours { get; set; }
        public WorkType WorkType { get; set; }

        public WorkPerformedEventArgs(int hours, WorkType workType)
        {
            this.Hours = hours;
            this.WorkType = workType;
        }
    }

    public class Worker
    {
        public event EventHandler<WorkPerformedEventArgs> WorkPerformed; // Event的定義

        public event EventHandler WorkCompleted;

        public virtual void DoWork(int hours, WorkType workType)
        {
            for (int i = 0; i < hours; i++)
            {
                // 每小時通知一次
                OnWorkPerformed(i + 1, workType);
            }
            // 結束時再通知一次
            OnWorkCompleted();
        }

        protected virtual void OnWorkPerformed(int hours, WorkType workType)
        {
            WorkPerformed?.Invoke(this, new WorkPerformedEventArgs(hours, workType));
        }

        protected virtual void OnWorkCompleted()
        {
            WorkCompleted?.Invoke(this, EventArgs.Empty); // 如果不打算帶資料可以使用EventArgs.Empty
        }
    }
}