using System;
using System.IO;
using UnityEngine;

public class SaveUtil
{
    private string jsonData;

    public void Save(SaveData data,string fileName)
    { 
        var exportPath = Application.persistentDataPath + "/." + fileName + ".json";
        jsonData = JsonUtility.ToJson(data);
        var sw = new StreamWriter(exportPath);
        sw.Write(jsonData);
        sw.Flush();
        sw.Close();
    }

    public static SaveData Load(string filePath)
    {
        if (!File.Exists(filePath)) throw new Exception("セーブデータのパスが間違っているか、セーブデータが存在しません。");
        var sr = new StreamReader(filePath);
        var data = sr.ReadToEnd();
        sr.Close();
        return JsonUtility.FromJson<SaveData>(data);
    }

    public static string[] GetSaveDataFiles()
    {
        return Directory.GetFiles(Application.persistentDataPath,"*.json");
    }
}