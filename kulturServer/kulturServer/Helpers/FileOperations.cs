using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kulturServer.Helpers
{
    public static class FileOperations
    {
        public static byte[] GetFileBytes(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
    }
}
