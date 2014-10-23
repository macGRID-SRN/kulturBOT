using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace kulturServer.Network
{
    class Handler
    {
        protected Models.iRobotCreate iRobotCreate;
        protected TcpClient tcpClient;
        protected NetworkStream clientStream;
        protected byte[] PacketHeader;

        protected const int BUF_SIZE = 4096;
        protected const byte CONFIRM_BYTE = 255;

        public Handler(byte[] PacketHeader, TcpClient tcpClient)
        {
            this.PacketHeader = PacketHeader;
            this.tcpClient = tcpClient;
            this.clientStream = tcpClient.GetStream();
        }

        /// <summary>
        /// This method is meant to be overridden, the implementation of whatever you are trying to "handle" whether it should be receive an image or WHATEVER should be done by overriding this method.
        /// </summary>
        /// <returns>If it was successful or not. Obviously this method should be overridden.</returns>
        public virtual bool PerformAction()
        {
            return false;
        }

        protected void GetAllBytes(List<byte> myBites)
        {
            while (true)
            {
                if (GetByteBurstOfDefaultSize(myBites) == 0)
                {
                    break;
                }
            }
            System.Diagnostics.Debug.WriteLine("Received packet of " + myBites.Count + " length.");
        }

        protected int GetByteBurstOfDefaultSize(List<byte> myBites)
        {
            byte[] message = new byte[BUF_SIZE];

            int bytesRead = 0;

            try
            {
                bytesRead = clientStream.Read(message, 0, BUF_SIZE);
                if (bytesRead != 1)
                    this.SendConfirmPacket();
            }
            catch
            {
                throw new Exception("Something with the socket or comms went wrong!");
            }

            if (bytesRead == 1 && message[0] == CONFIRM_BYTE)
                return 0;

            myBites.AddRange(message);

            return bytesRead;
        }

        protected void SendConfirmPacket()
        {
            this.clientStream.WriteByte(CONFIRM_BYTE);
            this.clientStream.Flush();
        }

        protected void CloseConnection()
        {
            if (this.tcpClient.Connected)
                this.tcpClient.Close();
        }

    }
}
