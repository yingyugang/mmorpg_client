using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class VersionCSV {
        public static string[] columnNameArray = new string[4];
        public static List<VersionCSV> LoadDatas(string text){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open(text);
            List<VersionCSV> dataList = new List<VersionCSV>();
            columnNameArray = new string[4];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                if (csvFile.mapData[i].data.Count < columnNameArray.Length){
					Debug.LogWarning("csvFile.mapData[i].data.Count :" + csvFile.mapData[i].data.Count + " columnNameArray.Length:" + columnNameArray.Length);
                    continue;
                }
                VersionCSV data = new VersionCSV();
                data.FileName = csvFile.mapData[i].data[0];
                int.TryParse(csvFile.mapData[i].data[1],out data.FileSize);
                int.TryParse(csvFile.mapData[i].data[2],out data.IsCSV);
                data.HashCode = csvFile.mapData[i].data[3];
                dataList.Add(data);
            }
            return dataList;
        }
        public string FileName;
        public int FileSize;
        public int IsCSV;
        public string HashCode;
    }
}
