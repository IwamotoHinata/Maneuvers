using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class TutorialMenu_offline : MonoBehaviour
    {
        [SerializeField] private GameObject _background1; //背景
        [SerializeField] private GameObject _button0; //メニュー
        [SerializeField] private GameObject _button1; //キャンセル
        [SerializeField] private GameObject _button2; //メインメニューに戻る

        private bool _MenuFlg = false; //メニュー制御

        public void OnButton0()
        {
            if (!_MenuFlg) //メニュー表示
            {
                _MenuFlg = true;

                _background1.SetActive(true);
                _button1.SetActive(true);
                _button2.SetActive(true);
            }
            else //メニュー非表示
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
            //メニューに戻る
            GridTransition.Instance.nextTransition = "MainMenu";
            GameObject.Find("GridTransition").GetComponent<Animator>().SetTrigger("isTransition");
        }
    }
}
