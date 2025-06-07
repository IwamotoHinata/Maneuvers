using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TextLoader
{
    private string LoadText(string path)
    {
        var asset = Resources.Load(path) as TextAsset;
        if (asset == null) throw new NullReferenceException("指定したシナリオファイルのパスに誤りがあります");
        return asset.text;
    }

    public Dictionary<int,string> LoadTextAsDic(string path)
    {
        var rawText = LoadText(path);
        var keyAndValue = rawText.Split(new string[] { "[EOS]" },StringSplitOptions.RemoveEmptyEntries);
        return ToDictionary(keyAndValue);
    }

    private Dictionary<int,string> ToDictionary(string[] keyAndValue)
    {
        var dic = new Dictionary<int,string>();
        foreach (var v in keyAndValue)
        {
            var sep = v.Split(new[] {','}, 2);
            dic.Add(int.Parse(sep[0]),sep[1]);
        }
        return dic;
    }
}
