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
            Console.WriteLine(this.playersConnections[0].RemoteEndPoint.Address.ToString() + 
                " is now in a session with " +
                this.playersConnections[1].RemoteEndPoint.Address.ToString());
        }

        public void NewData(PlayerConnection pc, int dataLength, byte[] data) {
            var recipent = pc.NetConnection == playersConnections[0] ? playersConnections[1] : playersConnections[0];
            //var recipent = pc.NetConnection == playersConnections[0] ? playersConnections[0] : playersConnections[1];
            var msg = Server.server.CreateMessage();
            msg.Write(dataLength);
            msg.Write(data);
            //server.SendMessage(msg, message.SenderConnection, NetDeliveryMethod.ReliableSequenced);
            Server.server.SendMessage(msg, recipent, NetDeliveryMethod.ReliableSequenced);
        }
    }
}
