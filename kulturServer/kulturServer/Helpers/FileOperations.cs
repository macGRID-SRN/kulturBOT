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

                text = FormatStringToParagraph(text, 50);
                try
                {
                    var image = Image.FromFile(fileName);
                    var e = Graphics.FromImage(image);

                    StringFormat sf = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    RectangleF rekt = new RectangleF(image.Width / 10, image.Height / 3 * 2, image.Width * 4 / 5, image.Height / 3);

                    Font futura = new Font("FuturaExtended", 25);
                    //text should be centered!
                    SolidBrush myBrush = new SolidBrush(Color.AntiqueWhite);

                    e.DrawString(text, futura, myBrush, rekt, sf);

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

            private static string FormatStringToParagraph(string inText, int maxCarWidth)
            {
                List<string> outList = new List<string>();
                string currentString = string.Empty;
                Queue<string> stringQueue = new Queue<string>(inText.Split(' '));
                do
                {
                    if (stringQueue.Peek().Length > maxCarWidth)
                    {
                        outList.Add(currentString);
                        outList.Add(stringQueue.Dequeue());
                        currentString = "  ";
                    }
                    else if (currentString.Length + stringQueue.Peek().Length > maxCarWidth)
                    {
                        outList.Add(currentString);
                        currentString = "  "; // double space for indent
                    }
                    else
                    {
                        currentString += " " + stringQueue.Dequeue();
                    }

                } while (stringQueue.Count > 0);
                if (!string.IsNullOrWhiteSpace(currentString))
                    outList.Add(currentString);
                return string.Join("\n", outList.ToArray());
            }
        }
    }
}
