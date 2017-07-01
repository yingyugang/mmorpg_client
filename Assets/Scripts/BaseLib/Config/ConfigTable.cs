using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114
#pragma warning disable 3021

namespace BaseLib
{
	public class ConfigTable
	{
        protected int _colCount = 0;
        protected List<ConfigRow> _rowList = new List<ConfigRow>();

        public ConfigTable()
        {
        }

        static public int getEnumCount<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public bool checkColCount<T>() where T : struct, IConvertible
        {
            int nCount = getEnumCount<T>();
            return nCount ==_colCount;
        }

        //初始化读取文件到队列
        public bool init(string strName, bool bExternal = false)
        {
            FileHelper fh = new FileHelper();
            try
            {
                if (bExternal)
                {
                    if (!fh.openFile(strName))
                        return false;
                }
                else
                {
                    if (!fh.open(strName))
                        return false;
                }

                int line = fh.rowAmount;
                for (int y = 0; y < line; y++)
                {
                    int cols = fh.columnAmount;
                    ConfigRow row = new ConfigRow(cols);
                    for (int x = 0; x < cols; x++)
                    {
                        row.addValue(fh.getStr(y, x));
                    }
                    this._rowList.Add(row);
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }

        public ConfigRow[] rows
        {
            get { return this._rowList.ToArray(); }
        }

        public List<ConfigRow> RowList
        {
            get { return this._rowList; }
        }

        //根据条件值返回行
        public ConfigRow getRow(System.Enum key, string value)
        {
            foreach (ConfigRow row in this._rowList)
            {
                string temp = "";
                if (row.getStringValue(key, out temp))
                {
                    if (temp == value)
                        return row;
                }
            }
            return null;
        }

        //根据条件值返回行
        public ConfigRow getRow(System.Enum key,int value)
        {
            foreach (ConfigRow row in this._rowList)
            {
                int temp = 0;
                if (row.getIntValue(key, out temp))
                {
                    if (temp == value)
                        return row;
                }
            }
            return null;
        }

        public ConfigRow[] getRows(ConfigRow[] srcList,System.Enum key, int value)
        {
            if (srcList == null)
                return null;
            List<ConfigRow> listRow = new List<ConfigRow>();
            foreach(ConfigRow item in srcList)
            {
                int outValue = 0;
                if (item.getIntValue(key, out outValue))
                {
                    if (outValue == value)
                        listRow.Add(item);
                }
            }
            return listRow.ToArray();
        }

        public ConfigRow[] getRows(System.Enum key, int value)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();
            foreach (ConfigRow row in this._rowList)
            {
                if(row.equalsValue(key,value))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }

        public ConfigRow getRow(System.Enum key1, int value1, System.Enum key2, int value2)
        {
            foreach (ConfigRow row in this._rowList)
            {
                if (row.equalsValue(key1, value1) && row.equalsValue(key2, value2))
                    return row;
            }
            return null;
        }

        public ConfigRow[] getRows(System.Enum key1, int value1, System.Enum key2, int value2)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();
            foreach (ConfigRow row in this._rowList)
            {
                if (row.equalsValue(key1, value1) && row.equalsValue(key2,value2))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }

        public ConfigRow getRow(System.Enum key1, int value1, System.Enum key2, int value2, System.Enum key3, int value3)
        {
            foreach (ConfigRow row in this._rowList)
            {
                if (row.equalsValue(key1, value1) && row.equalsValue(key2, value2) && row.equalsValue(key3, value3))
                    return row;
            }
            return null;
        }

        public ConfigRow[] getRows(System.Enum key1, int value1, System.Enum key2, int value2, System.Enum key3, int value3)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();
            foreach (ConfigRow row in this._rowList)
            {
                if (row.equalsValue(key1, value1) && row.equalsValue(key2, value2) && row.equalsValue(key3, value3))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }
    }
}
