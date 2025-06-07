using UnityEngine;

public class TankShield : MonoBehaviour
{

    public int shieldHP = 100;

    void Start()
    {
        //90秒後にオブジェクトを消す
        Invoke(nameof(DestroyShield), 90);

    }

    //Invokeで使う
    private void DestroyShield()
    {
        Destroy(this.gameObject);
        Debug.Log("消えた時間" + Time.time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            shieldHP -= 1;

            if (shieldHP < 1)
            {
                Destroy(this.gameObject);

            }
        }
    }
}
