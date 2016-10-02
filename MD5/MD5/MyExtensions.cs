using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MD5
{
    static class MyExtensions
    {
        public static byte[] ReadBytes(this FileStream fileStream)
        {
            byte[] bytes = new byte[fileStream.Length];
            for (long i = 0; i < fileStream.Length; i++)
            {
                bytes[i] = (byte)fileStream.ReadByte();
            }
            return bytes;
        }
        public static UInt32 ShiftLeftCircularly(this UInt32 x, int numOfBits)
        {
            return x << numOfBits | x >> (32 - numOfBits);
        }
    }
}
