using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Networks_Practic1
{
    class SecondThread
    {
        private Semaphore semaphoreToSend, semaphoreToReceive;
        private string sentMessage, receivedMessage, message, quittance;
        private FirstThread firstThread;

        public Thread currentThread;

        public SecondThread(Semaphore send, Semaphore receive)
        {
            semaphoreToSend = send;
            semaphoreToReceive = receive;
            currentThread = new Thread(s => Run());
        }

        public void StopReceiving()
        {
            Console.WriteLine("Второй поток: завершение работы");
            Thread.CurrentThread.Abort();
        }

        public void Solve()
        {
            Console.WriteLine("Второй поток: начало работы");

            while (true)
            {
                Console.WriteLine("Второй поток: ожидание передачи данных");
                this.semaphoreToReceive.WaitOne();
                message = this.receivedMessage;
                Console.WriteLine($"Второй поток: приняты данные - {message}");
                message = Underside.MessageTrim(message);

                if(Underside.GetElemFromTitle(message, 32) == Underside.ControlSumCount(message.Substring(32)))
                {
                    Console.WriteLine("Второй поток: контрольные суммы совпали");
                    message = message.Substring(32);

                    Console.WriteLine($"Второй поток: длина полезной информации = {Underside.GetElemFromTitle(message, 32)}");
                    message = message.Substring(32);

                    message = Underside.BitStringToString(Underside.DeleteBit(message));
                    Console.WriteLine($"Второй поток: расшифровано сообщение - {message}");
                    quittance = "yes";
                }
                else quittance = "no";

                Console.WriteLine("Второй поток: подготовка квитанции");
                this.sentMessage = Underside.MessageConcat(Underside.PushBit(Underside.StringToBitString(quittance)));
                firstThread.ReceiveData(this.sentMessage);
                this.semaphoreToSend.Release();
                Console.WriteLine($"Второй поток: отправлена квитанция {this.sentMessage}");
            }
        }

        public void Receive(string msg)
        {
            this.receivedMessage = msg;
        }

        public void Run() { this.Solve(); }
        public void SetFirstThread(FirstThread first)
        {
            this.firstThread = first;
        }
        
    }
}
