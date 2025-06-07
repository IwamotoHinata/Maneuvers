using UnityEngine;

public class TankShield : MonoBehaviour
{

    public int shieldHP = 100;

    void Start()
    {
        //90�b��ɃI�u�W�F�N�g������
        Invoke(nameof(DestroyShield), 90);

    }

    //Invoke�Ŏg��
    private void DestroyShield()
    {
        Destroy(this.gameObject);
        Debug.Log("����������" + Time.time);
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
