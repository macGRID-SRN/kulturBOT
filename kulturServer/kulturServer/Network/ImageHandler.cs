using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace kulturServer.Network
{
    class ImageHandler : Handler
    {
        public ImageHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        public override bool PerformAction()
        {
            this.SendConfirmPacket();

            ImageFormat.GetImageFormat(this.PacketHeader[1]);

            //still implementing this
            return true;
        }

        //this is some beaut code adapted from here: http://stackoverflow.com/questions/424366/c-sharp-string-enums
        public sealed class ImageFormat
        {
            public readonly string Extension;
            public readonly byte value;

            public static readonly ImageFormat JPG = new ImageFormat(0, ".jpg");
            public static readonly ImageFormat PNG = new ImageFormat(1, ".png");
            public static readonly ImageFormat BITMAP = new ImageFormat(2, ".bmp");

            private static readonly Dictionary<byte, ImageFormat> instance = new Dictionary<byte, ImageFormat>();

            public ImageFormat(byte value, string Extension)
            {
                //what is this magic?
                instance[value] = this;

                this.value = value;
                this.Extension = Extension;
            }

            public static ImageFormat GetImageFormat(byte ImageFormat)
            {
                if (instance.ContainsKey(ImageFormat))
                    return instance[ImageFormat];
                throw new Exception("Cannot find Image Type!");
            }
        }
    }
}
