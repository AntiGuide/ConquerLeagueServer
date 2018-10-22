using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;

namespace ConquerLeagueServer {
    class Server {
        private static bool stopServer = false;
        private const ushort PORT = 47410;
        private static string tmpMessage;

        static void Main(string[] args) {
            // Set up server
            var server = new SimpleTcpServer().Start(PORT);
            server.Delimiter = 0x13;
            server.AutoTrimStrings = false;

            server.DelimiterDataReceived += (sender, msg) => {
                msg.ReplyLine("You said: " + msg.MessageString);
                tmpMessage = msg.MessageString;
                Console.WriteLine("Server recieved: " + tmpMessage);
                Console.WriteLine("Server sent:     " + "You said: " + msg.MessageString);
                stopServer = true;
            };
            while (!stopServer) {
                
            }
            server.Stop();
            Console.Read();
        }

        public static void StopServer() {
            stopServer = true;
        }
    }
}
