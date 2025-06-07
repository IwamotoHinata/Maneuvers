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
                tips.text = "宇宙を巡る、種族を超えた英雄譚。";
                break;
            case "Battle":
                tips.text = "キミの戦略はどこまで通用するか。今こそ腕試し。";
                break;
            case "RandomMatch":
                tips.text = "どこかのだれかとオンライン対戦！";
                break;
            case "CostomMatch":
                tips.text = "合言葉でお友達とオンライン対戦！";
                break;
            case "Tutorial":
                tips.text = "まずはここから。戦闘のキホンをここで習得しよう。";
                break;
            case "Deck":
                tips.text = "戦術に応じてユニット・スキルを組み合わせよう。";
                break;
            case "QuitButton":
                tips.text = "ゲーム終了";
                break;
            case "OpenOptionButton":
                tips.text = "カメラの感度や音量など。お好みにカスタマイズ。";
                break;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tips.text = "";
    }
}
