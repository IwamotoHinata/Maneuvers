using System;
using System.IO;
using UnityEngine;
using Unit;
using System.Collections.Generic;

namespace DeckMake
{

    //他のシーンからのデータ取得方法
    /*
        DeckDataManager deckDataManager = new DeckDataManager();    //DeckDataManagerのインスタンスを生成（勝手にjsonからロードする）
        List<DeckData> deckList = deckDataManager.getDeckList();    //自分のデッキリストの取得を行う
     */

    //デッキデータクラス（基本触らない！！）
    [Serializable]
    public class DeckData
    {
        private const int unitNum = 6;   //ユニット編成数
        [SerializeField] string deckName = ""; //デッキ名
        [SerializeField] int leader = 0;  //リーダー
        [SerializeField] int[] unit = { 0, 0, 0, 0, 0, 0 };  //ユニットキャラ

        public string DeckName { get => deckName; set => deckName = value; }
        public int Leader { get => leader; set => leader = value; }
        public int[] Unit { get => unit; set => unit = value; }

    }


    //デッキデータ管理用のクラス（このクラスで主な管理を行う）
    public class DeckDataManager : MonoBehaviour
    {
        //デッキデータのセーブデータ用のラッパー（基本触らない！！）
        [Serializable]
        private class DeckDataWrapper
        {
            public int selectedIndex = 0;
            public List<DeckData> deckList = new List<DeckData>();
        }

        [SerializeField] MyDeck myDeck;
        private const int deckMaxNum = 10;

        string filepath;    // jsonファイルのパス
        string fileName = "deck_data.json"; // jsonファイル名

        void Awake()
        {
            // パス名取得
            filepath = Application.dataPath + "/Resources/" + fileName;

            //ファイルがないとき、ファイル作成
            if (!File.Exists(filepath))
            {
                Save();
            }
            else
            {
                //ファイルを読み込んでdataに格納
                Load();
            }
        }

        // ゲーム終了時に保存
        void OnDestroy()
        {
            //Save();
        }

        // ScriptableObject(MyDeck)のデータをjsonとしてデータを保存
        public void Save()
        {
            //jsonデータ <- ScriptableObject
            DeckDataWrapper jsonData = new DeckDataWrapper();
            jsonData.selectedIndex = myDeck.SelectedIndex;
            jsonData.deckList = new List<DeckData>(myDeck.DeckList);

            string json = JsonUtility.ToJson(jsonData);            // jsonとして変換
            StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
            wr.WriteLine(json);                                     // json変換した情報を書き込み
            wr.Close();                                             // ファイル閉じる
        }

        // jsonファイルからScriptableObject(MyDeck)に読み込み
        public void Load()
        {
            StreamReader rd = new StreamReader(filepath);               // ファイル読み込み指定
            string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
            rd.Close();                                             // ファイル閉じる

            var jsonData = JsonUtility.FromJson<DeckDataWrapper>(json);            // jsonファイルを型に戻して返す

            //ScriptableObject <- jsonデータ
            myDeck.SelectedIndex = jsonData.selectedIndex;
            myDeck.DeckList = jsonData.deckList;
        }
    }

}
