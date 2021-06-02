using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Networks_Practic1
{
    class Underside
    {
        public static int ControlSumCount(string msg)
        {
            int control = 0;

            for (int i=0; i<msg.Length; i++)
            {
                if(msg[i] == '1')
                {
                    control++;
                }
            }

            return control;
        }

        public static string MessageConcat(string msg)
        {
            string messageWithFlags = "01111110" + msg + "01111110";
            return messageWithFlags;
        }

        public static string MessageTrim(string msg)
        {
            if(msg.Substring(0, 8) == "01111110" && msg.Substring(msg.Length - 8, 8) == "01111110")
            {
                msg = msg.Substring(8, msg.Length-8);
            }
            return msg;
        }

        public static string StringToBitString(string msg)
        {
            return string.Join("", Encoding.ASCII.GetBytes(msg).Select(b => Convert.ToString(b,2).PadLeft(8,'0')));
        }

        public static string BitStringToString(string msg)
        {
            StringBuilder result = new StringBuilder();
            int k = 0;
            string symbol = "";

            for (int i=0; i<msg.Length; i++)
            {
                symbol += msg[i];
                k++;
                if(k%8 == 0)
                {
                    result.Append(BitToStringSymbol(symbol));
                    symbol = "";
                    k = 0;
                }
            }

            return result.ToString();

        }

        private static string BitToStringSymbol(string msg)
        {
            int k = Convert.ToInt32(msg, 2);
            byte[] b = new byte[] { Convert.ToByte(k) };
            return b.ToString();
        }

        public static string AddElemToTitle(string msg, int element, int size)
        {
            string elem = element.ToString();
            elem = StringToBitString(elem);
            if(elem.Length < size)
            {
                for(int i=0; i<32-elem.Length; i++)
                {
                    elem = "0" + elem;
                }
            }

            msg = elem;
            return msg;
        }

        public static int GetElemFromTitle(string msg, int size)
        {
            string title = "";

            for (int i=0; i<msg.Length; i++)
            {
                title += msg[i];
            }
            
            //title = BitStringToString(title);
            Console.WriteLine(title.Trim());
            int k = Int32.Parse(title);
            return k;
        }

        public static string DeleteBit(string msg)
        {
            int k = 0, msgLen = msg.Length;

            for(int i=0; i< msgLen; i++)
            {
                if(msg[i] == '1') k++;
                else  k = 0;

                if (k == 5)
                {
                    msg = msg.Substring(0, i + 1) + msg.Substring(i + 2, msgLen);
                    msgLen--;
                    k = 0;
                }
            }
            return msg;
        }

        public static string PushBit(string msg)
        {
            int k = 0, msgLen = msg.Length;

            for (int i = 0; i < msgLen; i++)
            {
                if (msg[i] == '1') k++;
                else k = 0;

                if (k == 5)
                {
                    msg = msg.Substring(0, i + 1) + "0" + msg.Substring(i + 2, msgLen);
                    msgLen++;
                    k = 0;
                }
            }
            return msg;
        }
    }
}
