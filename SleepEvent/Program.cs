using System;
using System.Threading;

namespace SleepEvent
{
    public delegate void TimerCount(int n);
    class Timer
    {
        private int seconds;
        private TimerCount timerCount;
        public delegate void TimerNotify(string t);
        public event TimerNotify Notify;
        public Timer(int s)
        {
            seconds = s;
        }
        public TimerCount TimerCount
        {
            get { return timerCount; }
            set { timerCount = value; }
        }
        public void CountBackward()
        {
            Console.WriteLine("Counting started...");
            while (seconds > 0)
            {
                timerCount(seconds--);
                Thread.Sleep(1000);
            }
            Notify.Invoke("It's time!");
        }
    }
    class Program
    {
        private static void ShowMessage(string txt)
        {
            Console.WriteLine(txt);
        }
        private static Timer InputSeconds()
        {
            Console.WriteLine("Enter seconds for counting:");
            int seconds = Convert.ToInt32(Console.ReadLine());
            return new Timer(seconds);
        }
        static void Main()
        {
            Timer timer = InputSeconds();
            timer.Notify += ShowMessage;
            TimerCount timerCountMain = (n) =>
            {
                Console.WriteLine(n);
            };
            timer.TimerCount = timerCountMain;
            timer.CountBackward();
        }
    }
}
