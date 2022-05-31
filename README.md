# WebSocketServerTemplate
Just sharing code that I wrote on one computer that will be worked on later on another computer


## Example Client
```csharp
using System;
using WebSocketSharp;

namespace wsclient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ws = new WebSocket("ws://localhost/client?name=test"))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine(e.Data);

                ws.Connect();
                bool notExit = true;
                while (notExit)
                {
                    string line = Console.ReadLine();
                    notExit = line != "exit";
                    // "{ \"Type\": 0, \"Payload\": \"\"}"
                    ws.Send(line);
                }
                Console.ReadKey(true);
            }
        }
    }
}
```
