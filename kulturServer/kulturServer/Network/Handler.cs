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
        protected TcpClient tcpClient;
        protected NetworkStream clientStream;
        protected byte[] PacketHeader;

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

        protected void SendConfirmPacket()
        {
            this.clientStream.WriteByte(255);
            this.clientStream.Flush();
        }

        protected void CloseConnection()
        {
            if (this.tcpClient.Connected)
                this.tcpClient.Close();
        }

        public sealed class CommType
        {
            public readonly string Extension;
            private readonly byte value;

            //these values have not been set for CommType... Not sure what should replace Extension!
            public static readonly CommType JPG = new CommType(0, ".jpg");
            public static readonly CommType PNG = new CommType(1, ".png");
            public static readonly CommType BITMAP = new CommType(2, ".bmp");

            private static readonly Dictionary<byte, CommType> instance = new Dictionary<byte, CommType>();

            public CommType(byte value, string Extension)
            {
                //what is this magic?
                instance[value] = this;

                this.value = value;
                this.Extension = Extension;
            }

            public static CommType GetCommType(byte CommType)
            {
                if (instance.ContainsKey(CommType))
                    return instance[CommType];
                throw new Exception("Communication Type not found!");
            }
        }
    }
}
