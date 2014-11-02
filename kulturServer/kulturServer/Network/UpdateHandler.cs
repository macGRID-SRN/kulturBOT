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
    class UpdateHandler : Handler
    {
        public UpdateHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        private UpdateType myUpdate;

        public override bool PerformAction()
        {
            this.myUpdate = UpdateType.GetUpdateType(PacketHeader[2]);

            var myObj = this.myUpdate.Type;

            var myHandler = (Handler)Activator.CreateInstance(myObj, PacketHeader, tcpClient);

            return myHandler.PerformAction();
        }

        public sealed class UpdateType
        {
            public readonly Type Type;
            private readonly byte value;

            private static readonly Dictionary<byte, UpdateType> instance = new Dictionary<byte, UpdateType>();

            public static UpdateType InformationUpdate = new UpdateType(2, typeof(InformationHandler));

            private UpdateType(byte value, Type t)
            {
                //what is this magic?
                instance[value] = this;
                this.Type = t;
                this.value = value;
            }

            public static UpdateType GetUpdateType(byte UpdateTypeByte)
            {
                if (instance.ContainsKey(UpdateTypeByte))
                    return instance[UpdateTypeByte];
                throw new Exception("Update Type not found!");
            }

        }
    }
}
