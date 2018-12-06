using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeague {
    class Session {
        NetConnection[] playersConnections = new NetConnection[2];
        bool[] isLeft = new bool[2];

        private enum GameMessageType : byte {
            SESSION_INITIALITZE = 1
        }

        public Session(NetConnection playerConnection1, NetConnection playerConnection2) {
            this.playersConnections[0] = playerConnection1 ?? throw new ArgumentNullException(nameof(playerConnection1), "Cannot be null.");
            this.playersConnections[1] = playerConnection2 ?? throw new ArgumentNullException(nameof(playerConnection2), "Cannot be null.");
            Console.WriteLine(this.playersConnections[0].RemoteEndPoint.Address+ 
                " is now in a session with " +
                this.playersConnections[1].RemoteEndPoint.Address);

            var r = new Random();
            if (r.Next(1) == 1) {
                isLeft[0] = true;
                isLeft[1] = false;
                Console.WriteLine(this.playersConnections[0].RemoteEndPoint.Address + " is on the left side");
                Console.WriteLine(this.playersConnections[1].RemoteEndPoint.Address + " is on the right side");
            } else {
                isLeft[0] = false;
                isLeft[1] = true;
                Console.WriteLine(this.playersConnections[0].RemoteEndPoint.Address + " is on the right side");
                Console.WriteLine(this.playersConnections[1].RemoteEndPoint.Address + " is on the left side");
            }

            var data0 = new byte[] { (byte)GameMessageType.SESSION_INITIALITZE, BitConverter.GetBytes(isLeft[0])[0] };
            var data1 = new byte[] { (byte)GameMessageType.SESSION_INITIALITZE, BitConverter.GetBytes(isLeft[1])[0] };
            var msg0 = Server.server.CreateMessage();
            var msg1 = Server.server.CreateMessage();
            msg0.Write(2);
            msg1.Write(2);
            msg0.Write(data0);
            msg1.Write(data1);
            Server.server.SendMessage(msg0, playersConnections[0], NetDeliveryMethod.ReliableOrdered);
            Server.server.SendMessage(msg1, playersConnections[1], NetDeliveryMethod.ReliableOrdered);
        }

        public void NewData(PlayerConnection pc, int dataLength, byte[] data) {
            var recipent = pc.NetConnection == playersConnections[0] ? playersConnections[1] : playersConnections[0];
            var msg = Server.server.CreateMessage();
            msg.Write(dataLength);
            msg.Write(data);
            Server.server.SendMessage(msg, recipent, NetDeliveryMethod.UnreliableSequenced);
        }
    }
}
