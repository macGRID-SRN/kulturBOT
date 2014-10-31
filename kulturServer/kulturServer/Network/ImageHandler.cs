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
        public const int IMAGE_SEND_MAX_TRIES = 5;
        public const int HASH_LENGTH = 16;

        public ImageFormat myFormat;

        public ImageHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        public override bool PerformAction()
        {
            this.myFormat = ImageFormat.GetImageFormat(this.PacketHeader[2]);
            this.SendConfirmPacket();

            var bytes = this.GetWholeImage(this.GetImageHash());

            //at this point we know the image was gotten properly (or not).
            CloseConnection();

            SaveToDiskDbAndTweet(bytes);

            return true;
        }

        private void SaveToDiskDbAndTweet(byte[] bytes)
        {
            string fileName = this.GetFileName();

            File.WriteAllBytes(fileName, bytes);
            System.Diagnostics.Debug.WriteLine("Wrote file: " + fileName);

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

            System.Diagnostics.Debug.WriteLine("Saved Image to db with ID: " + imageID);

            string TweetText = string.Empty;
            //get stuffs from markov factory
            try
            {
                TweetText = Helpers.Markov.GetNextTwitterPictureMarkov();
            }
            catch
            {
                TweetText = "Wasn't able to generate Markov.";
                System.Diagnostics.Debug.WriteLine("Generating Markov threw an error.");
            }

            Helpers.FileOperations.ImageOperations.ApplyTextToImage(TweetText, fileName);

            try
            {
                Helpers.Twitter.PostTweetWithImage(this.ROBOT_ID, imageID, TweetText);
            }
            catch (Exception e)
            {

            }
        }

        private string GetFileName()
        {
            return GetImgDir() + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + this.myFormat.Extension;
        }

        private byte[] GetWholeImage(byte[] hash)
        {
            this.SendConfirmPacket();
            var counter = 0;
            while (true)
            {
                List<byte> tempList = new List<byte>();

                GetAllBytes(tempList);

                var possibleImage = tempList.ToArray();

                var tempHash = Helpers.Hashing.GetMd5HashBytes(possibleImage);

                if (tempHash.SequenceEqual(hash))
                {
                    System.Diagnostics.Debug.WriteLine("File was received correctly");
                    this.SendConfirmPacket();
                    return possibleImage;
                }
                this.SendFailPacket();
                if (++counter >= IMAGE_SEND_MAX_TRIES)
                    break;
            }

            throw new Exception("Couldn't transfer file properly!");
        }

        private byte[] GetImageHash()
        {
            return this.GetByteBurstOfSetSize(HASH_LENGTH);;
        }

        private string GetImgDir()
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
