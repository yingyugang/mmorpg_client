using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BaseLib
{
    class MySqlClient
    {
        public string servIP = "";
        public UInt16 servPort = 3306;
        public string userName = "";
        public string passwd = "";
        public string database = "";

        MySqlConnection _db;

        public bool connect()
        {
            string connectionString = string.Format("Server = {0}; Database = {1}; User ID = {2}; Password = {3};Charset=utf8", servIP, database, userName, passwd); ;
            _db = new MySqlConnection(connectionString);
            _db.Open();
            return true;
        }

        public void close()
        {
            if (_db != null)
            {
                _db.Close();
                _db = null;
            }
        }

        MySqlDataReader exeQuery(string strSql)
        {
            try
            {
                return new MySqlCommand(strSql,_db).ExecuteReader();
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
        }

        public string[] getTableCsvData(string table)
        {
            MySqlDataReader data = null;
            try
            {
                List<string> rowList = new List<string>();
                string sql = "select * from {0}";
                sql = string.Format(sql,table);
                data = exeQuery(sql);
                if (data == null)
                    return null;
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        int count = data.FieldCount;
                        string line = ""; ;
                        for (int index=0; index < count; index++)
                        {
                            if(!Convert.IsDBNull(data.GetValue(index)))
                                line += data.GetString(index);;
                            line += ",";
                        }
                        rowList.Add(line);
                    }
                }
                data.Close();
                return rowList.ToArray();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        public string[] getTableColList(string table)
        {
            MySqlDataReader data = null;
            try
            {
                List<string> colList = new List<string>();
                string sql = "select column_name from INFORMATION_SCHEMA.COLUMNS where table_schema='{0}' and TABLE_name='{1}' order by ordinal_position";
                sql = string.Format(sql,database,table);
                data = exeQuery(sql);
                if (data == null)
                    return null;
                if (data.HasRows)
                {
                    while (data.Read())
                        colList.Add(data.GetString("column_name"));
                }
                data.Close();
                return colList.ToArray();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        public string[] getTableList()
        {
            MySqlDataReader data = null;
            try
            {
                List<string> tableList = new List<string>();
                string sql = "select table_name from INFORMATION_SCHEMA.tables where table_schema ='{0}' and table_name like 'dict%'";
                sql = string.Format(sql, database);
                data = exeQuery(sql);
                if (data == null)
                    return null;
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        string table = data.GetString("table_name");
                        if (table.Length > 0)
                            tableList.Add(table);
                    }
                }
                data.Close();
                return tableList.ToArray();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        public string getColComment(string table,string col)
        {
            MySqlDataReader data = null;
            try
            {
                string sql = "select data_type,COLUMN_comment from INFORMATION_SCHEMA.COLUMNS where table_schema='{0}' and TABLE_name='{1}' and COLUMN_name = '{2}'";
                sql = string.Format(sql, database, table, col);
                data = exeQuery(sql);
                if (data == null)
                    return null;
                string result = "";
                if (data.HasRows)
                {
                    data.Read();
                    result = data.GetString("data_type");
                    result += ";";
                    result += data.GetString("COLUMN_comment");
                }
                return result;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        public string getTableComment(string table)
        {
            MySqlDataReader data = null;
            try
            {
                string sql = "select table_comment from INFORMATION_SCHEMA.tables where table_schema ='{0}' and TABLE_name = '{1}'";
                sql = string.Format(sql, database, table);
                data = exeQuery(sql);
                if (data == null)
                    return null;
                string result = "";
                if (data.HasRows)
                {
                    data.Read();
                    result = data.GetString("table_comment");
                }
                return result;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return null;
            }
            finally
            {
                if(data!=null)
                    data.Close();
            }
        }
    }
}
