using System.Collections;
using UnityEngine;
using Fusion;
using Unit;

public class CultistLeader : BasicLeader
{
    private PlayerRef _attackerAuthority;
    private NetworkObject _selectedCharacter;   //�I�𒆂�Unit
    private Vector3 _tempPos;
    // Start is called before the first frame update
    protected override void UseUlt()
    {
        if (_selectedCharacter != null)
        {
            Debug.Log("����");
            _selectedCharacter.GetComponent<CharacterMove>().iSelect(false); //�I������
            RemoveMySelectedCharacter();    //�I�𒆂�Unit�����Z�b�g
        }
        StartCoroutine(Telepote());
    }

    IEnumerator Telepote()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));  //�E�N���b�N�����܂ő҂�
        var mousePosition = Input.mousePosition;
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            _tempPos = hit.point;    //�}�E�X���N���b�N�����ʒu���L��
        }

        yield return new WaitUntil(() => _selectedCharacter != null);
        LeaderObserver.RPC_TeleportUnit(_tempPos, _selectedCharacter);
    }
    
    public void SetMySelectedCharacter(NetworkObject myCharacter)   //�I�𒆂�Unit���L��
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

