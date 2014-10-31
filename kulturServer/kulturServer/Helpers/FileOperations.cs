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

        public static bool TextOverlayExists(string fileName)
        {
            return File.Exists(TextOverlayString(fileName));
        }

        public static string TextOverlayString(string fileName)
        {
            return System.IO.Path.GetDirectoryName(fileName) + @"\" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ImageOperations.OVERLAY_FLAG + System.IO.Path.GetExtension(fileName);
        }

        public static class ImageOperations
        {
            public const string OVERLAY_FLAG = "-txt";
            //must make this work with images other than jpgs
            public static void ApplyTextToImage(string text, string fileName)
            {
                Font futura = new Font("FuturaExtended", 20);

                try
                {
                    var image = Image.FromFile(fileName);
                    var e = Graphics.FromImage(image);

                    //text should be centered!
                    e.DrawString(text, futura, Brushes.White, new PointF(0, 0));

                    //there has to be a better way..
                    string temp = System.IO.Path.GetDirectoryName(fileName) + @"\" + System.IO.Path.GetFileNameWithoutExtension(fileName) + OVERLAY_FLAG + ".jpg";

                    System.Diagnostics.Debug.WriteLine("Added file with text overlay: " + temp);

                    image.Save(temp, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (OutOfMemoryException e)
                {
                    System.Diagnostics.Debug.WriteLine("Couldn't open file to apply text!");
                    Handlers.ExceptionLogger.LogException(e, Models.Fault.Server);
                }
            }
        }
    }
}
