using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PDALab3
{
    class Program
    {
        public static int maxProd = 20;
        public static List<string> list = new List<string>();
        public static Object condC = new object();
        public static Object condP = new object();
        

        public static void Consume()
        {
            string cons = "";
            lock(list)
            {
                if(list.Count == 0)
                {
                    Console.WriteLine("Lista goala");
                    lock (condC)
                    {
                        Monitor.Wait(condC);
                    }
                }
                cons = list[0];
                list.RemoveAt(0);
            }
            if(cons != "")
            {
                Console.WriteLine("Produs consumat");
                lock (condP)
                {
                    Monitor.Pulse(condP);
                }
            }
            else
            {
                Console.WriteLine("Ceva nu a mers");
            }
        }

        public static void Produce()
        {
            string prod = "produs";
            lock(list)
            {
                if(list.Count == maxProd)
                {
                    Console.WriteLine("Lista plina");
                    lock (condP)
                    {
                        Monitor.Wait(condP);
                    }
                }
                list.Add(prod);
                Console.WriteLine("Produs creat");
            }
            lock(condC)
            {
                Monitor.Pulse(condC);    
            }
        }

        static void Main(string[] args)
        {
            Thread[] thP = new Thread[10];
            Thread[] thC = new Thread[10];

            for(int i = 0; i < 10; i++)
            {
                thP[i] = new Thread(Produce);
                thC[i] = new Thread(Consume);
            }
            for(int i = 0; i < 10; i++)
            {
                thP[i].Start();
                thC[i].Start();
            }

            for (int i = 0; i < 10; i++)
            {
                thP[i].Join();
                thC[i].Join();
            }

            Console.ReadLine();
        }
    }
}
