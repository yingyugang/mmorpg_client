using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
//using ComponentAce.Compression.Libs.zlib;

namespace BaseLib
{
    class Tools
    {
        static public string GetDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        static public T getEnumValue<T>(int value, T defValue) where T : struct, IConvertible
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (value == Convert.ToInt32(item))
                    return item;
            }
            return defValue;
        }

        static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[1024 * 32];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static bool unzip(ref byte[] data)
        {
//            MemoryStream oms = new MemoryStream();
//            ZOutputStream ozs = new ZOutputStream(oms);
//            MemoryStream ims = new MemoryStream(data);
//            try
//            {
//                CopyStream(ims, ozs);
//                data = null;
//                data = new byte[oms.Length];
//                Array.Copy(oms.GetBuffer(), 0, data, 0, oms.Length);
//            }
//            catch (Exception ex)
//            {
//                UnityEngine.Debug.Log("unzip Exception :" + ex.Message);
//                return false;
//            }
//            finally
//            {
//                ozs.Close();
//                oms.Close();
//                ims.Close();
//            }
            return true;
        }
    }
}