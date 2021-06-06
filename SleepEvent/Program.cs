using System;
using System.Threading;

namespace SleepEvent
{
    public delegate void TimerCount(int n);
    class Timer
    {
        private int seconds;
        public event TimerCount TimerCount;
        public delegate void TimerNotify(string t);

        public event TimerNotify Notify;
        public Timer(int s)
        {
            seconds = s;
        }
        
        public void CountBackward()
        {
            while (seconds > 0)
            {
                TimerCount(seconds--);
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
            TimerCount Each10Sec = (n) =>
            {
                if (n % 10 == 0)
                {
                    Console.WriteLine(n + " passed...");
                }
            };
            timer.TimerCount += timerCountMain;
            timer.TimerCount += Each10Sec;
            Console.WriteLine("Counting started...");
            timer.CountBackward();
        }
    }
}
