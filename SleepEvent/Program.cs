using System;
using System.Threading;

namespace SleepEvent
{
    class MessageEventArgs : EventArgs
    {
        private readonly string message;
        public MessageEventArgs(string m)
        {
            message = m;
        }
        public string MessageNotify { get => message; }
    }
    public delegate void TimerCount(int n);
    class Timer
    {
        private int seconds;
        public event TimerCount TimerCount;
        public event EventHandler<MessageEventArgs> Message;
        public Timer(int s)
        {
            seconds = s;
        }
        private void OnMessageSent(MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = Volatile.Read(ref Message);
            handler?.Invoke(this, e);
        }
        public void CountBackward()
        {
            while (seconds > 0)
            {
                TimerCount.Invoke(seconds--);
                Thread.Sleep(1000);
            }
            MessageEventArgs messageEvent = new("It's time!");
            OnMessageSent(messageEvent);
        }
    }
    class Program
    {
        private static Timer InputSeconds()
        {
            Console.WriteLine("Enter seconds for counting:");
            int seconds = Convert.ToInt32(Console.ReadLine());
            return new Timer(seconds);
        }
        static void Main()
        {
            Timer timer = InputSeconds();
            timer.Message += Timer_Message;
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
        private static void Timer_Message(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.MessageNotify);
        }
    }
}
