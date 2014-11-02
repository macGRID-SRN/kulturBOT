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
    class InformationHandler : Handler
    {
        public InformationHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        private InformationType myInfoHandler;

        public override bool PerformAction()
        {
            this.myInfoHandler = InformationType.GetUpdateType(PacketHeader[3]);

            var myObj = this.myInfoHandler.Type;

            var myHandler = (Handler)Activator.CreateInstance(myObj, PacketHeader, tcpClient);

            return myHandler.PerformAction();

            return true;
        }

        public sealed class InformationType
        {
            public readonly Type Type;
            private readonly byte value;

            private static readonly Dictionary<byte, InformationType> instance = new Dictionary<byte, InformationType>();

            public static InformationType RecentTweetHandler = new InformationType(2, typeof(RecentTweetHandler));

            private InformationType(byte value, Type t)
            {
                //what is this magic?
                instance[value] = this;
                this.Type = t;
                this.value = value;
            }

            public static InformationType GetUpdateType(byte UpdateTypeByte)
            {
                if (instance.ContainsKey(UpdateTypeByte))
                    return instance[UpdateTypeByte];
                throw new Exception("Update Type not found!");
            }

        }
    }
}