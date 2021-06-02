using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Networks_Practic1
{
    class FirstThread
    {
        private Semaphore semaphoreToSend, semaphoreToReceive;
        private string sentMessage, receivedMessage, message, quittance;
        private SecondThread secondThread;

        public Thread currentThread;

        public FirstThread(Semaphore send, Semaphore receive, string msg)
        {
            semaphoreToSend = send;
            semaphoreToReceive = receive;
            message = msg;
            quittance = "no";
            currentThread = new Thread(s => Run());
        }

        public void Solve()
        {
            Console.WriteLine("Первый поток: начало работы");

            while(this.quittance == "no")
            {
                Console.WriteLine("Первый поток: подготовка данные для передачи");
                Console.WriteLine($"Первый поток: {this.message}");

                string msg = Underside.PushBit(Underside.StringToBitString(this.message));
                int len = msg.Length;

                msg = Underside.AddElemToTitle(msg, len, 32);
                Console.WriteLine($"Первый поток: длина полезной информации = {len}");

                int control = Underside.ControlSumCount(msg);
                msg = Underside.AddElemToTitle(msg, control, 32);
                msg = Underside.MessageConcat(msg);
                this.sentMessage = msg;

                this.secondThread.Receive(this.sentMessage);
                this.semaphoreToSend.Release();

                Console.WriteLine($"Первый поток: переданы данные - {this.sentMessage}");
                Console.WriteLine($"Первый поток: ожидание квитанции");

                this.semaphoreToReceive.WaitOne();

                this.quittance = Underside.BitStringToString(Underside.DeleteBit(Underside.MessageTrim(this.quittance)));
                Console.WriteLine($"Первый поток: получена квитанция - {this.quittance}");

                if (this.quittance == "no")
                {
                    Console.WriteLine($"Первый поток: ошибка при передаче квитанции - требуется повторная отправка");
                }
                Console.WriteLine($"Первый поток: завершение работы");
                this.secondThread.StopReceiving();
            }
        }

        public void ReceiveData(string msg)
        {
            this.receivedMessage = msg;
        }

        public void Run() { this.Solve(); }

        public void SetSecondThread(SecondThread secondThread)
        {
            this.secondThread = secondThread;
        }
    }
}
