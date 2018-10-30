using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeague {
    class SessionManager {

        private PlayerConnection waitingPlayerConnection;

        private List<PlayerConnection> allPlayerConnections = new List<PlayerConnection>();
        private List<Session> allSessions = new List<Session>();

        public void AddPlayerToMatchmaking(NetConnection netConnection) {
            if (waitingPlayerConnection == null) {
                waitingPlayerConnection = new PlayerConnection(netConnection);
                allPlayerConnections.Add(waitingPlayerConnection);
                return;
            }

            try {
                waitingPlayerConnection.Session = new Session(waitingPlayerConnection.NetConnection, netConnection);
            } catch (ArgumentNullException argE) {
                Console.WriteLine(argE.Message);
            }

            allSessions.Add(waitingPlayerConnection.Session);
            allPlayerConnections.Add(new PlayerConnection(netConnection, waitingPlayerConnection.Session));
            waitingPlayerConnection = null;
            
        }

        public void ForwardMessageToSession(NetConnection senderConnection, int dataLength, byte[] data) {
            foreach (PlayerConnection pc in allPlayerConnections) {
                if (pc.NetConnection == senderConnection) {
                    if (pc.Session != null) {
                        pc.Session.NewData(pc, dataLength, data);
                    }

                    return;
                }
            }
        }
    }
}
