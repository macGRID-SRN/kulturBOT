using System;
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
        public const string fileDirectory = @"kulturbotIMG\";

        public ImageHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        public override bool PerformAction()
        {
            ImageFormat myFormat = ImageFormat.GetImageFormat(this.PacketHeader[2]);
            this.SendConfirmPacket();

            var TotalByteList = new List<byte>();

            GetAllBytes(TotalByteList);

            string fileName = GetImgDir() + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + myFormat.Extension;

            File.WriteAllBytes(fileName, TotalByteList.ToArray());


            int imageID;
            using (var db = new Models.Database())
            {
                var robot = db.Robots.FirstOrDefault(l => l.ID == this.ROBOT_ID);

                var image = new Models.Image()
                {
                    iRobot = robot,
                    FileDirectory = fileName,
                    TimeAdded = DateTime.UtcNow,
                    TimeCreated = DateTime.UtcNow,
                    TimeTaken = DateTime.UtcNow
                };

                db.Images.Add(image);

                db.SaveChanges();
                imageID = image.ID;
            }

            //need to consider order of operations here, should the connection remain open while writing to file? to db?
            CloseConnection();

            Helpers.Twitter.PostTweetWithImage(this.ROBOT_ID, imageID);

            return true;
        }

        public string GetImgDir()
        {
            string rootC = @"c:\";
            if (!Directory.Exists(rootC + fileDirectory))
            {
                Directory.CreateDirectory(rootC + fileDirectory);
            }

            return rootC + fileDirectory;
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
