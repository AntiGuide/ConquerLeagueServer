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
                            
                            var dataLength = message.ReadInt32();
                            var data = message.ReadBytes(dataLength);
                            //foreach (var item in data) {
                            //    Console.Write(item.ToString() + ", ");
                            //}

                            //Console.Write("\r\n");
                            sessionManager.ForwardMessageToSession(message.SenderConnection, dataLength, data);
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
                                case NetConnectionStatus.RespondedConnect:
                                    Console.WriteLine(message.SenderConnection.Status.ToString());
                                    break;
                                default:
                                    Console.WriteLine("Unhandled status change with type: " + message.SenderConnection.Status.ToString());
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            message.SenderConnection.Approve();
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
                    server.Recycle(message);
                }
            }

        }
    }
}
