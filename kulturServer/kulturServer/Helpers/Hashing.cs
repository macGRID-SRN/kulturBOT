using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace kulturServer.Helpers
{
    static public class Hashing
    {
        public static Random randy = new Random();

        public static string GetMd5Hash(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return ASCIIEncoding.ASCII.GetString(md5.ComputeHash(stream));
                }
            }
        }

        public static string GetMd5Hash(byte[] fileIn)
        {
            using (var md5 = MD5.Create())
            {
                return ASCIIEncoding.ASCII.GetString(md5.ComputeHash(fileIn));
            }
        }

        public static byte[] GetMd5HashBytes(byte[] fileIn)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(fileIn);
            }
        }

        public static byte[] GetMd5HashBytes(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }
    }
}
