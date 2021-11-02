using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicationUI
{
    class CRC16Checker
    {
        private const int poly = 0xA001;    //CRC-16-IBM reversed
        static readonly UInt16[] table = new UInt16[256];
        // Create lookup table
        public CRC16Checker()
        {
            UInt16 val;
            UInt16 temp;
            for (UInt16 i = 0; i < table.Length; ++i)
            {
                val = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((val ^ temp) & 0x0001) != 0)
                    {
                        val = (UInt16)((val >> 1) ^ poly);
                    }
                    else
                    {
                        val >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = val;
            }
        }
        // Lookup table
        public static UInt16 ComputeChecksum(byte[] bytes)
        {
            UInt16 crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (UInt16)((crc >> 8) ^ table[index]);
            }
            return crc;
        }
        // Bit by bit
        public static UInt16 ComputChecksum(byte[] buffer, int len)
        {
            UInt16 crc = 0xFFFF;
            for (int i = 0; i < len; i++)
            {
                crc ^= (UInt16)buffer[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= poly;
                    }
                    else crc >>= 1;
                }
            }
            return crc;
        }
    }
}
