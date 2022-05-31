using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace wsserver
{
    class Client : WebSocketBehavior
    {
        static private Dictionary<string, User> openConnections = new Dictionary<string, User>();

        protected override void OnMessage(MessageEventArgs e)
        {
            string data = e.Data;
            WSEvents myEvent = JsonSerializer.Deserialize<WSEvents>(data);

            switch (myEvent.Type)
            {
                case (int)EventType.REQUEST_USERS:
                    string resp = "Connected Users:\n";

                    foreach (var pair in openConnections)
                    {
                         resp += $"{pair.Value.ID} - {pair.Value.Name} : {pair.Value.JoinedAt}\n";
                    }
                    Send(resp);
                    break;
                case (int)EventType.PM_USER:
                    var parts = myEvent.Payload.Split(":");
                    if (parts.Length != 2)
                    {
                        Send("Bad message format message.");
                    }
                    if (!openConnections.ContainsKey(parts[0]))
                    {
                        Send($"User: {parts[0]} does not exist.");
                    }
                    Sessions.SendTo(parts[1], parts[0]);
                    break;
                default:
                    Send(e.Data);
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (!openConnections.ContainsKey(ID))
            {
                throw new Exception($"Error: id {ID} should exist.");
            }
            var user = openConnections[ID];
            Console.WriteLine($"id: {ID} ({user.Name}) left. (was here for {DateTime.Now - user.JoinedAt})");
            base.OnClose(e);
        }

        protected override void OnOpen()
        {
            var name = Context.QueryString["name"];
            Console.WriteLine($"id: {ID} ({name}) joined.");
            if (openConnections.ContainsKey(ID))
            {
                // request reconnect.
                throw new Exception($"Error: user with ID {ID} already exists.");
            }

            openConnections.Add(ID, new User()
            {
                Name = name,
                ID = ID,
                JoinedAt = DateTime.Now
            });

            base.OnOpen();
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }
    }
}
