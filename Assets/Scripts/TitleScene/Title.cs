using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 行うこと
/// ①最初の入力受付（タイトルのspriteの変更,テキストの透明度を周期的に変更）
/// ②タイトル関係のオブジェクトを破壊してメインメニューに移行
/// </summary>
public class Title : MonoBehaviour
{
    [SerializeField] GameObject _titlePanel;
    [SerializeField] GameObject _text;
    [SerializeField] GameObject _fadeOutPanel;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _deckManager;//デッキ編成のjsonファイルを読み込むor新規作成のために一時的に出す

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
    /// ユーザからの入力を待つ
    /// 入力があれば画面を1秒かけてホワイトアウトさせてメインメニュのパネルを表示させる
    /// </summary>
    /// <returns></returns>
    IEnumerator waitUserInput()
    {
        yield return new WaitUntil(() => Input.anyKey);
        var fadeOut = _fadeOutPanel.GetComponent<Image>();
        SoundManager.Instance.shotSe(SeType.TitleDecision);

        //一秒かけて画面をホワイトアウト
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

        //メインメニューのセッティングを行う
        _mainMenuPanel.SetActive(true);
        Destroy(_titlePanel);
        Destroy(_deckManager);
    }

    /// <summary>
    /// テキストの色を変更させるコルーチン
    /// 5秒で透明度を0～100まで往復させる
    /// </summary>
    /// <returns></returns>
    IEnumerator changeTextColor()
    {
        var text_tmp = _text.GetComponent<TMP_Text>();
        float setTime = 5;//透明度を往復させるのにかける時間
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
