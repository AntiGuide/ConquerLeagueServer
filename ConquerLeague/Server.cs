using ConquerLeague;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lidgren.Network {
    class Server {
        public static NetServer server;

        static void Main(string[] args) {
            var config = new NetPeerConfiguration("ConquerLeague") { Port = 47410 };
            server = new NetServer(config);
            server.Start();

            SessionManager sessionManager = new SessionManager();

            while (true) {
                NetIncomingMessage message;
                while ((message = server.ReadMessage()) != null) {
                    //Console.WriteLine("Message came in");
                    switch (message.MessageType) {
                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            var data = message.ReadString();
                            sessionManager.ForwardMessageToSession(message.SenderConnection, data);
                            //var msg = server.CreateMessage(data);
                            //server.SendMessage(msg, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                            // Console.WriteLine(data);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            Console.WriteLine(message.SenderConnection.Status);
                            switch (message.SenderConnection.Status) {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("Client " + message.SenderConnection.RemoteEndPoint.ToString() + " connected!");
                                    sessionManager.AddPlayerToMatchmaking(message.SenderConnection);
                                    break;
                            }
                            break;

                        case NetIncomingMessageType.DebugMessage:
                            // handle debug messages
                            // (only received when compiled in DEBUG mode)
                            Console.WriteLine(message.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine("Warning: " + message.ReadString());
                            break;
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                    }
                }
            }

        }
    }
}
