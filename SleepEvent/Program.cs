using System;
using System.Threading;

namespace SleepEvent
{
    public delegate void TimerCount(int n);

    class Timer
    {
        private int seconds;
        public TimerCount timerCount;   //проблема, поле приватное
        public delegate void TimerNotify(string t);
        public event TimerNotify Notify;
        public Timer(int s)
        {
            seconds = s;
        }

        public void CountBackward()
        {
            Console.WriteLine("Counting started...");
            while (seconds > 0)
            {
                timerCount(seconds--);
                Thread.Sleep(1000);
                if (seconds == 0)
                {
                    Notify.Invoke("It's time!");
                }
            }
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
            timer.timerCount = timerCountMain;
            TimerCount timerCount10Mess = (n) =>
            {
                if (n % 10 == 0)
                {
                    Console.WriteLine($"{n} seconds passed...");
                }
            };
            timer.timerCount += timerCount10Mess;
            timer.CountBackward();

        }
    }
}
