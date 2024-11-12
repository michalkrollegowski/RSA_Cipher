using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Cipher
{
    public static class BigIntegerExtensions
    {
        public static int GetBitLength(this BigInteger bigint)
        {
            byte[] bytes = bigint.ToByteArray();
            return 8 * bytes.Length - LeadingZeroCount(bytes[bytes.Length - 1]);
        }

        private static int LeadingZeroCount(byte value)
        {
            int count = 0;
            for (int i = 7; i >= 0; i--)
            {
                if ((value & (1 << i)) == 0)
                    count++;
                else
                    break;
            }
            return count;
        }
    }
}
