using System;
using WebSocketSharp.Server;

namespace wsserver
{
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://localhost");
            wssv.AddWebSocketService<Client>("/client");
            wssv.Start();
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
