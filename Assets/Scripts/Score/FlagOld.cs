/*
    Flag（旗）にアタッチする
 */
using UnityEngine;

public enum Camp
{
    A,
    B,
    NONE,
}
public class FlagOld : MonoBehaviour
{
    // コンポーネント
    [SerializeField]
    private GameObject scoreManagerObj;

    public static Camp holder = Camp.NONE;  // フラグの所有者
    Color[] flagColor = { Color.blue, Color.red, Color.white }; //フラグの色（A:青,B:赤,どちらでもない:白）

    /* フラグの所有者をセットする */
    private void SetFlagHost(Camp camp)
    {
        holder = camp;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            SetFlagHost(other.GetComponent<FlagOnPlayer>().myCamp);
            gameObject.GetComponent<Renderer>().material.color = flagColor[(int)holder];    //色を変更
        }
    }

}
