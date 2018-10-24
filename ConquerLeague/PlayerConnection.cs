using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerLeague {
    class PlayerConnection {
        public Session Session {
            get { return session; }
            set { session = value; }
        }

        public NetConnection NetConnection {
            get { return netConnection; }
            set { netConnection = value; }
        }

        private NetConnection netConnection;
        private Session session;

        public PlayerConnection(NetConnection netConnection, Session session = null) {
            this.netConnection = netConnection;
            this.session = session;
        }


    }
}
