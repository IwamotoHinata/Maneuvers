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
    /// inputText��enter�L�[�������ꂽ�Ƃ��ɌĂ΂��֐�
    /// </summary>
    public void GetRoomName()
    {

        Debug.Log(_inputField.text);
        //room���̍X�V
        GameLauncher.Instance.roomName = _inputField.text;

        //InputField�𔒎��ɖ߂�(�K�v������΃R�����g�O��)
        //_inputField.text = "";
    }
}
