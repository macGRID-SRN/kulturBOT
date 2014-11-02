using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace kulturServer.Network
{
    class InstructionHandler : Handler
    {
        public InstructionHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        public override bool PerformAction()
        {

            return false;
        }
    }
}
