using System;

namespace ItHappend.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new ConsoleClient();
            client.Start();
        }
    }
}