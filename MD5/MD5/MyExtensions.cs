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
        public static UInt32 ShiftLeftCircularly(this UInt32 x, int numOfBits)
        {
            return (x << numOfBits) | (x >> (32 - numOfBits));
        }
        

    }
}
