using System;
using System.Threading;

namespace Networks_Practic1
{
    class Program
    {
        static void Main(string[] args)
        {
            Semaphore firstReceive = new Semaphore(0, 1);
            Semaphore secondReceive = new Semaphore(0, 1);

            Char[] symbols = new Char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Random rnd = new Random();
            string message = "";
            for (int i=0; i<5; i++)
            {
                message += symbols[rnd.Next(0, 26)];
            }

            FirstThread firstThread = new FirstThread(secondReceive, firstReceive,message);
            SecondThread secondThread = new SecondThread(firstReceive, secondReceive);

            firstThread.SetSecondThread(secondThread);
            secondThread.SetFirstThread(firstThread);

            firstThread.currentThread.Start();
            secondThread.currentThread.Start();
            firstThread.currentThread.Join();
            secondThread.currentThread.Join();
        }
    }
}
