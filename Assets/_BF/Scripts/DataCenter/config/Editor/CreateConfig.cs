using UnityEngine;
using UnityEditor;
using System.IO;
using BaseLib;

public class CreateConfig : EditorWindow 
{
	[MenuItem ("Tools/ConfigTool")]
    static void AddWindow ()
	{
		Rect  wr = new Rect (0,0,500,500);
        CreateConfig window = (CreateConfig)EditorWindow.GetWindowWithRect(typeof(CreateConfig), wr, true, "CreateConfig");	
		window.Show();
	}
	
	string _strCfgPath = "Assets/Resources/Configs/dict";
	string _strCodePath = "Assets/_BF/Scripts/DataCenter/config";
	
	public void Awake () 
	{
	}
	
	void OnGUI () 
	{
        int top = 0;
        int height = 50;
        int index = 0;
        int weight = 200;

        if (GUI.Button(new Rect(80, top + height * index, weight, 30), "选择配置文件目录"))
		{
			_strCfgPath = EditorUtility.OpenFolderPanel("open config Path","Assets","");
		}
        GUILayout.Space(20);
        GUILayout.Label(_strCfgPath, EditorStyles.boldLabel);
        index++;
        if (GUI.Button(new Rect(80, top + height * index, weight, 30), "选择代码目标目录"))
		{
			_strCodePath = EditorUtility.OpenFolderPanel("Save Code Path","Assets","");
			GUILayout.Label(_strCodePath);
		}
        GUILayout.Space(30);
        GUILayout.Label(_strCodePath, EditorStyles.boldLabel);
        index++;
        if (GUI.Button(new Rect(80, top + height * index, weight, 30), "生成源代码"))
		{
			readFileList();
		}
        GUILayout.Space(30);
        GUILayout.Label("ModuleDefine.cs", EditorStyles.boldLabel);
        GUILayout.Label("ConfigDefinec.cs", EditorStyles.boldLabel);
        index++;
        index++;
        if (GUI.Button(new Rect(80, top + height * index, weight, 30), "导出配置文件"))
        {
            loadConfigCsv();
        }
        GUILayout.Space(60);
        GUILayout.Label(_strCfgPath, EditorStyles.boldLabel);
    }

    MySqlClient openDb()
    {
        MySqlClient client = new MySqlClient();
        client.servIP = "192.168.1.151";
        client.database = "xs2_game";
        client.userName = "test";
        client.passwd = "test";
        client.connect();
        return client;
    }
	
	void readFileList()
	{
        MySqlClient db = openDb();

        string strModuleFIle = _strCodePath + "/" + "ModuleDefine.cs";
		//module define
        StreamWriter module = new StreamWriter(strModuleFIle, false);
		module.WriteLine("using System;");
		module.WriteLine("using System.Collections.Generic;");
		module.WriteLine("using System.ComponentModel;");
		module.WriteLine("using System.Reflection;");
		module.WriteLine("");
		module.WriteLine("namespace DataCenter");
		module.WriteLine("{");
		module.WriteLine("	public enum CONFIG_MODULE");
		module.WriteLine("	{");
		
		//enmu define
		StreamWriter config = new StreamWriter(_strCodePath + "/" + "ConfigDefine.cs",false);		
		config.WriteLine("using System;");
		config.WriteLine("");		
		config.WriteLine("namespace DataCenter");		
		config.WriteLine("{");		
		string[] files = Directory.GetFiles(_strCfgPath);
		foreach(string filePath in files)
		{
			//module定义
			FileInfo file = new FileInfo(filePath);
			string []temp = file.Name.Split('.');
            if (temp.Length != 2 || temp[1] != "csv")
                continue;
            string tablecomment = db.getTableComment(temp[0]);
            if (tablecomment != null && tablecomment.Length > 0)
                module.WriteLine(string.Format("        //{0}",tablecomment));
            string strItem = string.Format("		[Description(\"dict/{0}\")] {1},", temp[0], temp[0].ToUpper());
			module.WriteLine(strItem);

            if (tablecomment != null && tablecomment.Length > 0)
                config.WriteLine(string.Format("    //{0}", tablecomment));
            config.WriteLine("	public enum {0}", temp[0].ToUpper());
			config.WriteLine("	{");
			
			//enmu define
			string []items = file.OpenText().ReadLine().Split(',');
			foreach(string item in items)
			{
                if (item == string.Empty)
                    break;
                string colcomment = db.getColComment(temp[0],item);
                if (colcomment != null && colcomment.Length > 0)
                    config.WriteLine(string.Format("        //{0}", colcomment));
				config.WriteLine("		{0},",item.ToUpper());
			}
			config.WriteLine("	}");
			config.WriteLine("");
		}
		
		config.WriteLine("}");
		config.Flush();
		config.Close();
		
		module.WriteLine("	}");
		module.WriteLine("}");
		module.Flush();
		module.Close();
        db.close();
	}

    void loadConfigCsv()
    {
        MySqlClient db = openDb();
        if (db == null)
            return;
        string[] tableList = db.getTableList();
        foreach (string item in tableList)
        {
            string strTable = _strCfgPath + "/" + item + ".csv";
            StreamWriter table = new StreamWriter(strTable, false);

            string cols = "";
            string[] colList = db.getTableColList(item);
            foreach (string col in colList)
            {
                cols += col;
                cols += ",";
            }
            table.WriteLine(cols);
            string[] rowList = db.getTableCsvData(item);
            foreach (string row in rowList)
                table.WriteLine(row);
            table.Flush();
            table.Close();
        }
        db.close();
    }
}