using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Online;
using Fusion;
using System.Collections;
/// <summary>
/// 数字キーを入力したときに任意のユニットを選択状態にするためのスクリプト（バグレポ25対応）
/// </summary>
public class CharacterSelectOnNumber : NetworkBehaviour
{
    private ObserveCharacters _characters;
    // Start is called before the first frame update
    public void Awake()
    {
        StartCoroutine(loadGame());
        Debug.Log("loadGameStart");
    }

    /// <summary>
    /// 数字キーを入力したときに任意のユニットを選択状態にする
    /// </summary>
    public void selectOnNumber()
    {
        int key = int.Parse(Input.inputString) - 1;//入力されたキーをint型に変換,配列なので1引く

        if (GameLauncher.Runner.GameMode == GameMode.Host)
        {
            for (int i = 0; i < _characters.myUiCharacters.Length; i++)
            {
                if (i != key)
                    continue;
                else
                {
                    var characterMove = _characters.myUiCharacters[i].GetComponent<Unit.CharacterMove>();
                    characterMove.iSelect(true);
                }
            }
        }
        //else if (GameLauncher.Runner.GameMode == GameMode.Client)
        //{
            for (int i = 0; i < 5; i++)
            {
                if (i != key)
                    continue;
                else
                {
                    RPC_getClientSelectOnNumber(key);
                }
            }
        //}
    }

    IEnumerator loadGame()
    {
        yield return new WaitUntil(() => GameLauncher.Runner != null);
        GameObject.Find("Characters").TryGetComponent<ObserveCharacters>(out _characters);
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)
                     || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
            .Subscribe(_ => selectOnNumber()).AddTo(this);

        if (_characters != null)
            Debug.Log(_characters);
        else
            Debug.Log("null");
    }

    [Rpc(sources:RpcSources.All, targets:RpcTargets.StateAuthority)]
    public void RPC_getClientSelectOnNumber(int key)
    {
        var obj = _characters.enemyUiCharacters[key];
        if (obj == null)
        {
            Debug.Log("nanimonai");
            return;
        }
        NetworkId id = obj.GetComponent<NetworkObject>().Id;
        RPC_setClientSelectOnNumber(id);
    }
    
    [Rpc(sources:RpcSources.StateAuthority, targets:RpcTargets.All)]
    public void RPC_setClientSelectOnNumber(NetworkId selectId)
    {
        if (GameLauncher.Runner.GameMode == GameMode.Host)
            return;

        if (GameLauncher.Runner.GameMode == GameMode.Client)
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag("Unit");
            foreach (GameObject character in characters) 
            {
                NetworkObject myNetworkObject = character.GetComponent<NetworkObject>();
                if (myNetworkObject.Id != selectId)
                { 
                    continue;
                }                   
                else
                {
                    var myCharacterMove = character.GetComponent<Unit.CharacterMove>();
                    myCharacterMove.iSelect(true);
                }
            }
        }
    }
    
}
