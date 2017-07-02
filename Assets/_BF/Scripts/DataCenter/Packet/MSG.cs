using System;
using System.Collections.Generic;

namespace BaseLib
{
    public class MSG
    {
        public static uint PACK = 0;
        public static uint UNPACK = 1;

        public static byte[] Truncate(byte[] msg)
        {
            if (msg.Length < 2)
                return msg;

            ushort len = BitConverter.ToUInt16(msg, 0);
            byte[] message = new byte[len];

            Array.Copy(msg, message, len);
            return message;
        }

        public static string HexStringToString(string hexStr)
        {
            if (0 == hexStr.Length)
                return "";

            string[] strSplit = hexStr.Split('-');
            byte[] bt = new byte[strSplit.Length];
            for (int i = 0; i < bt.Length; i++)
                bt[i] = byte.Parse(strSplit[i], System.Globalization.NumberStyles.AllowHexSpecifier);

            return System.Text.Encoding.UTF8.GetString(bt);
        }
    }
}
