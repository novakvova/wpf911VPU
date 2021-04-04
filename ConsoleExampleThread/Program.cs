using CatRenta.Application.Interfaces;
using CatRenta.Infrastructure.Services;
using System;
using System.Threading;

namespace ConsoleExampleThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread catThread = new Thread(InsertCat);
            catThread.Start();
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Run main---");
                Thread.Sleep(200);
            }
            return;
            //int mainId = Thread.CurrentThread.ManagedThreadId;

            //Thread thread2 = new Thread(Thread2);
            //Thread thread3 = new Thread(Thread3);
            //Console.WriteLine("Hello World! " +mainId);
            //thread2.Start();
            //thread3.Start();
            //Thread.Sleep(2000);
            //Console.WriteLine("Hello World 1!");
        }
        static void InsertCat()
        {
            int count;
            ICatService catService = new CatService();
            Console.WriteLine("На початку роботи котів: " + catService.Count());
            Console.WriteLine("Скільки котів додати в БД:");
            count = int.Parse(Console.ReadLine());
            catService.InsertCats(count);
            Console.WriteLine("У кінці роботи котів: " + catService.Count());
        }

        //static void Thread2()
        //{
        //    Thread.Sleep(50);
        //    var colorDefalut = Console.ForegroundColor;
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    //Console.WriteLine("Thread "+threadId);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.WriteLine($"{i}.Привіт земляни!"+ threadId);
        //        Console.ForegroundColor = colorDefalut;
        //        Thread.Sleep(100);
        //    }
        //}

        //static void Thread3()
        //{
        //    var colorDefalut = Console.ForegroundColor;
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    //Console.WriteLine("Thread " + threadId);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine($"{i}.Привіт Колорадо! "+threadId);
        //        Thread.Sleep(200);
        //        Console.ForegroundColor = colorDefalut;
        //    }
        //}
    }
}
