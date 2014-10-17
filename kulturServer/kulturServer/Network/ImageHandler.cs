﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace kulturServer.Network
{
    class ImageHandler : Handler
    {
        public ImageHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        public override bool PerformAction()
        {
            ImageFormat myFormat = ImageFormat.GetImageFormat(this.PacketHeader[1]);
            this.SendConfirmPacket();

            var TotalByteList = new List<byte>();

            GetAllBytes(TotalByteList);

            File.WriteAllBytes("test" + myFormat.Extension, TotalByteList.ToArray());

            CloseConnection();
            return true;
        }

        //this is some beaut code adapted from here: http://stackoverflow.com/questions/424366/c-sharp-string-enums
        public sealed class ImageFormat
        {
            public readonly string Extension;
            private readonly byte value;

            private static readonly Dictionary<byte, ImageFormat> instance = new Dictionary<byte, ImageFormat>();

            public static readonly ImageFormat JPG = new ImageFormat(0, ".jpg");
            public static readonly ImageFormat PNG = new ImageFormat(1, ".png");
            public static readonly ImageFormat BITMAP = new ImageFormat(2, ".bmp");


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
