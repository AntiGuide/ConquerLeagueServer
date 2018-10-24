using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeague {
    class Session {
        NetConnection[] playersConnections = new NetConnection[2];

        public Session(NetConnection playerConnection1, NetConnection playerConnection2) {
            this.playersConnections[0] = playerConnection1 ?? throw new System.ArgumentNullException("Cannot be null.", "playerConnection1");
            this.playersConnections[1] = playerConnection2 ?? throw new System.ArgumentNullException("Cannot be null.", "playerConnection2");
        }

        public void NewData(PlayerConnection pc, string data) {
            var recipent = pc.NetConnection == playersConnections[0] ? playersConnections[1] : playersConnections[0];
            Server.server.SendMessage(Server.server.CreateMessage(data), recipent, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
