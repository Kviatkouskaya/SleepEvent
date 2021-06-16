using System;
using System.Threading;

namespace SleepEvent
{
    class MessageEventArgs : EventArgs
    {
        public string MessageNotify { get; private set; }
        public MessageEventArgs(string m)
        {
            MessageNotify = m;
        }
    }
    class TimerCountEventArgs : EventArgs
    {
        public int SecondsCount { get; private set; }
        public TimerCountEventArgs(int s)
        {
            SecondsCount = s;
        }
    }
    public delegate void TimerCount(int n);
    class Timer
    {
        private int seconds;
        public event EventHandler<TimerCountEventArgs> Counter;
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
        private void OnCounterSent(TimerCountEventArgs e)
        {
            EventHandler<TimerCountEventArgs> handler = Volatile.Read(ref Counter);
            handler?.Invoke(this, e);
        }
        public void CountBackward()
        {
            while (seconds > 0)
            {
                TimerCountEventArgs timerCountEvent = new(seconds--);
                OnCounterSent(timerCountEvent);
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
            timer.Counter += Timer_Counter;
            Console.WriteLine("Counting started...");
            timer.CountBackward();
        }
        private static void Timer_Counter(object sender, TimerCountEventArgs e)
        {
            Console.WriteLine(e.SecondsCount);
        }
        private static void Timer_Message(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.MessageNotify);
        }
    }
}
