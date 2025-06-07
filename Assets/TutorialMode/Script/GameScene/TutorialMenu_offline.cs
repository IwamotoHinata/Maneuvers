using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class TutorialMenu_offline : MonoBehaviour
    {
        [SerializeField] private GameObject _background1; //�w�i
        [SerializeField] private GameObject _button0; //���j���[
        [SerializeField] private GameObject _button1; //�L�����Z��
        [SerializeField] private GameObject _button2; //���C�����j���[�ɖ߂�

        private bool _MenuFlg = false; //���j���[����

        public void OnButton0()
        {
            if (!_MenuFlg) //���j���[�\��
            {
                _MenuFlg = true;

                _background1.SetActive(true);
                _button1.SetActive(true);
                _button2.SetActive(true);
            }
            else //���j���[��\��
            {
                _MenuFlg = false;

                _background1.SetActive(false);
                _button1.SetActive(false);
                _button2.SetActive(false);
            }
        }

        public void OnButton1()
        {
            _MenuFlg = false;

            _background1.SetActive(false);
            _button1.SetActive(false);
            _button2.SetActive(false);
        }

        public void OnButton2()
        {
            //���j���[�ɖ߂�
            GridTransition.Instance.nextTransition = "MainMenu";
            GameObject.Find("GridTransition").GetComponent<Animator>().SetTrigger("isTransition");
        }
    }
}
