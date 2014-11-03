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
        protected byte ROBOT_ID;

        protected const int BUF_SIZE = 4096;
        protected const byte CONFIRM_BYTE = 255;

        public Handler(byte[] PacketHeader, TcpClient tcpClient)
        {
            this.PacketHeader = PacketHeader;
            this.tcpClient = tcpClient;
            this.clientStream = tcpClient.GetStream();
            this.ROBOT_ID = this.PacketHeader[0];
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

        protected byte[] GetAllBytes()
        {
            List<byte> myBites = new List<byte>();
            while (true)
            {
                if (GetByteBurstOfDefaultSize(myBites) == 0)
                {
                    break;
                }
            }
            System.Diagnostics.Debug.WriteLine("Received packet of " + myBites.Count + " length.");
            return myBites.ToArray();
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
            catch (Exception e)
            {
                Handlers.ExceptionLogger.LogException(e, Models.Fault.Client);
            }

            if (bytesRead == 1 && message[0] == CONFIRM_BYTE)
                return 0;

            myBites.AddRange(message.Take(bytesRead));

            System.Diagnostics.Debug.WriteLine(bytesRead);

            return bytesRead;
        }

        protected byte[] GetByteBurstOfSetSize(int size)
        {
            byte[] message = new byte[size];

            int bytesRead = 0;

            try
            {
                bytesRead = clientStream.Read(message, 0, size);
            }
            catch (Exception e)
            {
                Handlers.ExceptionLogger.LogException(e, Models.Fault.Client);
            }

            return message;
        }

        protected void SendAllBytes(byte[] myBytes)
        {
            int bytesSent = 0;
            while (bytesSent < myBytes.Length)
            {
                int bytesToSend = Math.Min(myBytes.Length - bytesSent, BUF_SIZE);
                this.clientStream.Write(myBytes, 0, bytesToSend);
                this.clientStream.Flush();
                bytesSent += bytesToSend;
            }

            System.Diagnostics.Debug.WriteLine("Send packet of {0} length.", bytesSent);
        }

        protected void SendConfirmPacket()
        {
            this.clientStream.WriteByte(CONFIRM_BYTE);
            this.clientStream.Flush();
        }

        protected void SendFailPacket()
        {
            this.clientStream.WriteByte(0);
            this.clientStream.Flush();
        }

        protected void SendSingeBytePacket(byte myByte)
        {
            this.clientStream.WriteByte(myByte);
            this.clientStream.Flush();
        }

        protected void CloseConnection()
        {
            if (this.tcpClient.Connected)
                this.tcpClient.Close();
        }
    }
}
