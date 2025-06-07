using System;

[Serializable]
public class SaveData
{
    public string place;
    public string date;

    public SaveData(string place)
    {
        this.place = place;
        date = DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分");
    }
}