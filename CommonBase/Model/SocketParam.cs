using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase.Model
{
    public class SocketParam
    {
        private string ip;
        private int port; 

        public SocketParam(string _ip, int _port)
        {
            ip = _ip;
            port = _port;
        }

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
