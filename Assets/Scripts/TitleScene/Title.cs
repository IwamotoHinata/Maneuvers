using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �s������
/// �@�ŏ��̓��͎�t�i�^�C�g����sprite�̕ύX,�e�L�X�g�̓����x�������I�ɕύX�j
/// �A�^�C�g���֌W�̃I�u�W�F�N�g��j�󂵂ă��C�����j���[�Ɉڍs
/// </summary>
public class Title : MonoBehaviour
{
    [SerializeField] GameObject _titlePanel;
    [SerializeField] GameObject _text;
    [SerializeField] GameObject _fadeOutPanel;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _deckManager;//�f�b�L�Ґ���json�t�@�C����ǂݍ���or�V�K�쐬�̂��߂Ɉꎞ�I�ɏo��

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startTitleBGM());
        StartCoroutine(waitUserInput());
        StartCoroutine(changeTextColor());
        _mainMenuPanel.SetActive(false);
    }
    IEnumerator startTitleBGM()
    {
        yield return new WaitUntil(() => BGMPlayer.Instance != null);
        BGMPlayer.Instance.playBGM(BGMType.Title);
    }

    /// <summary>
    /// ���[�U����̓��͂�҂�
    /// ���͂�����Ή�ʂ�1�b�����ăz���C�g�A�E�g�����ă��C�����j���̃p�l����\��������
    /// </summary>
    /// <returns></returns>
    IEnumerator waitUserInput()
    {
        yield return new WaitUntil(() => Input.anyKey);
        var fadeOut = _fadeOutPanel.GetComponent<Image>();
        SoundManager.Instance.shotSe(SeType.TitleDecision);

        //��b�����ĉ�ʂ��z���C�g�A�E�g
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            fadeOut.color += new Color(0, 0, 0, 0.01f);

            if (fadeOut.color == Color.white)
            {
                Destroy(_fadeOutPanel);
                break;
            }
                
        }

        //���C�����j���[�̃Z�b�e�B���O���s��
        _mainMenuPanel.SetActive(true);
        Destroy(_titlePanel);
        Destroy(_deckManager);
    }

    /// <summary>
    /// �e�L�X�g�̐F��ύX������R���[�`��
    /// 5�b�œ����x��0�`100�܂ŉ���������
    /// </summary>
    /// <returns></returns>
    IEnumerator changeTextColor()
    {
        var text_tmp = _text.GetComponent<TMP_Text>();
        float setTime = 5;//�����x������������̂ɂ����鎞��
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            float alpha = Mathf.Abs(Mathf.Cos(2 * Mathf.PI * (1 / (setTime * 2)) * Time.time));
            text_tmp.color = new Color(0, 0, 0, alpha);

            if (_text == null)
                break;
        }
    }
}
