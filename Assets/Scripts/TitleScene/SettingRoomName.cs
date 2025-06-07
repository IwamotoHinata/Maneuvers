using Online;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingRoomName : MonoBehaviour
{
    private TMP_InputField _inputField;

    // Start is called before the first frame update
    void Start()
    {
        _inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
    }

    /// <summary>
    /// inputTextでenterキーが押されたときに呼ばれる関数
    /// </summary>
    public void GetRoomName()
    {

        Debug.Log(_inputField.text);
        //room名の更新
        GameLauncher.Instance.roomName = _inputField.text;

        //InputFieldを白紙に戻す(必要があればコメント外す)
        //_inputField.text = "";
    }
}
