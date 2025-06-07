using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MouseOverTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text tips;

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (this.gameObject.name)
        {
            case "Story":
                tips.text = "�F��������A�푰�𒴂����p�Y杁B";
                break;
            case "Battle":
                tips.text = "�L�~�̐헪�͂ǂ��܂Œʗp���邩�B�������r�����B";
                break;
            case "RandomMatch":
                tips.text = "�ǂ����̂��ꂩ�ƃI�����C���ΐ�I";
                break;
            case "CostomMatch":
                tips.text = "�����t�ł��F�B�ƃI�����C���ΐ�I";
                break;
            case "Tutorial":
                tips.text = "�܂��͂�������B�퓬�̃L�z���������ŏK�����悤�B";
                break;
            case "Deck":
                tips.text = "��p�ɉ����ă��j�b�g�E�X�L����g�ݍ��킹�悤�B";
                break;
            case "QuitButton":
                tips.text = "�Q�[���I��";
                break;
            case "OpenOptionButton":
                tips.text = "�J�����̊��x�≹�ʂȂǁB���D�݂ɃJ�X�^�}�C�Y�B";
                break;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tips.text = "";
    }
}
