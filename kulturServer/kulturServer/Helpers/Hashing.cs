using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace kulturServer.Helpers
{
    static class Hashing
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
    }
}
