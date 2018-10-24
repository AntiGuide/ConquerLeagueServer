using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeague {
    class PlayerConnection {
        public Session Session { get; set; }
        public NetConnection NetConnection { get; set; }

        public PlayerConnection(NetConnection NetConnection, Session Session = null) {
            this.NetConnection = NetConnection;
            this.Session = Session;
        }
    }
}
