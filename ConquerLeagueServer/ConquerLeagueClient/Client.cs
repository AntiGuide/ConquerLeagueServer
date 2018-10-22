using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeagueClient {
    class Client {
        private const ushort PORT = 47410;

        static void Main(string[] args) {
            // Set up client
            var client = new SimpleTcpClient();
            try {
                client.Connect("127.0.0.1", PORT);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.Read();
                return;
            }
            client.Delimiter = 0x13;
            client.AutoTrimStrings = false;

            var replyMsg = client.WriteLineAndGetReply("Hello world!", TimeSpan.FromSeconds(3));
            Console.WriteLine("Client sent:     " + "Hello world!");
            Console.WriteLine("Client recieved: " + replyMsg.MessageString);
            Console.WriteLine((int)replyMsg.MessageString[replyMsg.MessageString.Length - 1]);
            client.Disconnect();
            Console.Read();
        }
    }
}
