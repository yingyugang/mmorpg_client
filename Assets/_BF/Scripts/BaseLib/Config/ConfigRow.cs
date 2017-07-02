using System;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108

namespace BaseLib
{
    //配置
    public class ConfigRow
    {
        string[] _data = null;
        int m_nIndex = 0;
        int m_nCols = 0;

        private ConfigRow()
        {
        }

        public ConfigRow(int nCols)
        {
            _data = new string[nCols];
            m_nCols = nCols;
        }

        public void addValue(string value)
        {
            if (_data == null || m_nIndex >= m_nCols)
                return;
            this._data[m_nIndex++] = value;
        }

        public bool equalsValue(System.Enum key,int value)
        {
            int index = this.getIndex(key);
            if (index == -1)
                return false;
            int temp = 0;
            if (!System.Int32.TryParse(_data[index], out temp))
                return false;
            return (value == temp);
        }

        public bool getIntValue(System.Enum key, out int value)
        {
            int index = this.getIndex(key);
            if (index == -1)
            {
                value = 0;
                return false;
            }

            if (System.Int32.TryParse(_data[index], out value))
                return true;

            float fValue;
            if (getFloatValue(key, out fValue))
                value = (int)fValue;
            return false;
        }

        public bool getBoolValue(System.Enum key, bool defValue)
        {
            int index = this.getIndex(key);
            if (index != -1)
            {
                bool result = defValue;
                if (System.Boolean.TryParse(_data[index], out result))
                    return result;
            }
            return defValue;            
        }

        public int getIntValue(System.Enum key, int defValue = 0)
        {
            int index = this.getIndex(key);
            if (index != -1)
            {
                int result = defValue;
                if (System.Int32.TryParse(_data[index], out result))
                {
                    return result;
                }
                else
                {
                    float fValue;
                    if (getFloatValue(key, out fValue))
                        return (int)fValue;
                }
            }
            return defValue;
        }

        public bool getFloatValue(System.Enum key,out float value)
        {
            int index = this.getIndex(key);
            if (index == -1)
            {
                value = 0f;
                return false;
            }
            return System.Single.TryParse(_data[index], out value);
        }

        public float getFloatValue(System.Enum key, float defValue = 0f)
        {
            int index = this.getIndex(key);
            if (index != -1)
            {
                float reslut = defValue;
                if (System.Single.TryParse(_data[index], out reslut))
                    return reslut;
            }
            return defValue;
        }

        public string getStringValue(System.Enum key)
        {
            int index = this.getIndex(key);
            if (index != -1)
                return _data[index];
            return "";
        }

        public bool getStringValue(System.Enum key, out string value)
        {
            int index = this.getIndex(key);
            if (index == -1)
            {
                value = "";
                return false;
            }
            value = _data[index];
            return true;
        }

        public int getIndex(System.Enum key)
        {
            if (_data == null)
                return -1;
            int value = Convert.ToInt32(key);
            if (value < 0 || value >= m_nCols || value > m_nIndex)
                return -1;
            return value;
        }

        public T getEnumValue<T>(System.Enum key, T defValue) where T : struct, IConvertible
        {
            int index = this.getIndex(key);
            if (index == -1)
                return defValue;
            string strValue = this._data[index];
            if (strValue == string.Empty)
                return defValue;
            int value = Convert.ToInt32(this._data[index]);
            Type type = typeof(T);
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (value == Convert.ToInt32(item))
                    return item;
            }
            return defValue;
        }
    }
}
