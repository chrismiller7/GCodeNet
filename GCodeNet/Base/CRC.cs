using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCodeNet
{
    public static class CRC
    {
        public static byte Calculate(string line)
        {
            var bytes = Encoding.UTF8.GetBytes(line);
            int cs = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                cs = cs ^ bytes[i];
            }
            cs = cs & 0xff;
            return (byte)cs;
        }
    }
}
