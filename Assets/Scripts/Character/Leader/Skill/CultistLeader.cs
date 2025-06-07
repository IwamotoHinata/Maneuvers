using System.Collections;
using UnityEngine;
using Fusion;
using Unit;

public class CultistLeader : BasicLeader
{
    private PlayerRef _attackerAuthority;
    private NetworkObject _selectedCharacter;   //選択中のUnit
    private Vector3 _tempPos;
    // Start is called before the first frame update
    protected override void UseUlt()
    {
        if (_selectedCharacter != null)
        {
            Debug.Log("解除");
            _selectedCharacter.GetComponent<CharacterMove>().iSelect(false); //選択解除
            RemoveMySelectedCharacter();    //選択中のUnitをリセット
        }
        StartCoroutine(Telepote());
    }

    IEnumerator Telepote()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));  //右クリックされるまで待つ
        var mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            _tempPos = hit.point;    //マウスをクリックした位置を記憶
        }

        yield return new WaitUntil(() => _selectedCharacter != null);
        LeaderObserver.RPC_TeleportUnit(_tempPos, _selectedCharacter);
    }
    
    public void SetMySelectedCharacter(NetworkObject myCharacter)   //選択中のUnitを記憶
    {
        _selectedCharacter = myCharacter;
        Debug.Log(_selectedCharacter.name);
    }

    public void RemoveMySelectedCharacter()
    {
        _selectedCharacter = null;
    }



    public void PassiveSkill()
    {

    }

}

