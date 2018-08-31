using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace timer
{
    class Program
    {
        static void PrintTime(object state)
        {
            //Console.Clear();
            Console.WriteLine("Текущее время:  " +
                DateTime.Now.ToLongTimeString());
        }

        static void Main()
        {
            // Делегат для типа Timer
            TimerCallback timeCB = new TimerCallback(PrintTime);

            Timer time = new Timer(timeCB, null, 0, 1000);
            Console.WriteLine("Нажми чтоб выйти");
            Console.ReadLine();
        }
    }
}
