using System;

namespace PikabaV3.NET
{
    public class Program
    {
        static void Main()
        {
            var manager = new NetClientManager();
            manager.RunNetClientV1();
            manager.RunNetClientV2();
            Console.ReadKey();
        }
    }
}


