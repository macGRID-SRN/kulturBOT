using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace kulturServer.Helpers
{
    public static class FileOperations
    {
        public static byte[] GetFileBytes(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        public static class ImageOperations
        {
            //must make this work with images other than jpgs
            public static void ApplyTextToImage(string text, string fileName)
            {
                Font futura = new Font("FuturaExtended", 20);

                var image = Image.FromFile(fileName);
                var e = Graphics.FromImage(image);

                e.DrawString(text, futura, Brushes.White, new PointF(0,0));

                //there has to be a better way..
                string temp = System.IO.Path.GetDirectoryName(fileName) + @"\" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "-txt.jpg";

                System.Diagnostics.Debug.WriteLine("Added file with text overlay: " + temp);

                image.Save(temp, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
