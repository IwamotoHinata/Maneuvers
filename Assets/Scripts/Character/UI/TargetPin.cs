using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unit;
using UnityEngine.UI;

public class TargetPin : MonoBehaviour
{
    private ReactiveProperty<bool> Visible { get; } = new ReactiveProperty<bool>(false);
    private Vector3 _targetPoint;
    //private bool _hasInputAuthority;
    private CharacterBaseData _myCharacterBaseData;

    private void Start()
    {

        //���W�̈ێ�
        this.UpdateAsObservable()
            .Subscribe(_ => {
                transform.position = _targetPoint;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            });
        Visible.Subscribe(_ => gameObject.SetActive(Visible.Value));
        _myCharacterBaseData = transform.parent.parent.gameObject.GetComponent<CharacterBaseData>();
        if (!_myCharacterBaseData.isHasInputAuthority()) {
            foreach (Transform child in transform)
            {
                Debug.Log(child.gameObject.name + "�̐F��ς��܂�");
                // �q�I�u�W�F�N�g�ɑ΂��鏈���������ɏ���
                child.gameObject.GetComponent<Image>().color = Color.clear;
            }
        }

    }

    public void setPoint(Vector3 point)
    {
            Visible.Value = true;
            _targetPoint = point;
        
    }

    public void reset()
    {
        Visible.Value = false;
    }
}
