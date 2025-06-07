using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotController : MonoBehaviour
{
    [SerializeField] private List<Text> saveTexts;
    private Dictionary<string,SaveData> saveDatas = new Dictionary<string, SaveData>();
    
    private void Awake()
    {
        LoadSaveFiles();
        foreach (var sd in saveDatas)
        {
            var text = saveTexts.FirstOrDefault(st => st.name.Equals(sd.Key));
            if (text != null) text.text = sd.Value.place + Environment.NewLine + sd.Value.date;
        }
    }

    private void LoadSaveFiles()
    {
        var saveFiles = SaveUtil.GetSaveDataFiles();
        foreach (var f in saveFiles)
        {
            var name = f.Substring(f.Length-14,9);
            saveDatas.Add(name,SaveUtil.Load(f));
        }
    }
}
