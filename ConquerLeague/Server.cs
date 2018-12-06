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
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);
            server.Start();

            SessionManager sessionManager = new SessionManager();

            while (true) {
                NetIncomingMessage message;
                while ((message = server.ReadMessage()) != null) {
                    switch (message.MessageType) {
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage response = server.CreateMessage(); // Create a response and write some example data to it
                            response.Write("ConquerLeagueServer");
                            server.SendDiscoveryResponse(response, message.SenderEndPoint); // Send the response to the sender of the request
                            break;
                        case NetIncomingMessageType.Data:
                            var dataLength = message.ReadInt32();
                            var data = message.ReadBytes(dataLength);
                            sessionManager.ForwardMessageToSession(message.SenderConnection, dataLength, data);
                            break;
                        case NetIncomingMessageType.StatusChanged:
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
                            Console.WriteLine(message.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine("Warning: " + message.ReadString());
                            break;
                        default:
                            Console.WriteLine("unhandled message with type: " + message.MessageType);
                            break;
                    }

                    server.Recycle(message);
                }
            }

        }
    }
}
