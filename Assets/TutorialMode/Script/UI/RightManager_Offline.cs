using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightManager_Offline : MonoBehaviour
{
    private int Senario = 0;
    public Sprite face1; //ƒLƒƒƒ‰‚ÌŠç
    public Sprite face2;
    public Sprite face3;
    private Image Face;
    // Start is called before the first frame update
    void Start()
    {
        Face = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Senario)
        {
            case 1:
                {
                    Face.sprite = face1;
                    break;
                }
            case 2:
                {
                    Face.sprite = face2;
                    break;
                }
            case 3:
                {

                    break;
                }
            case 4:
                {

                    break;
                }
            case 5:
                {

                    break;
                }
        }
    }
    public void changeSenario()
    {
        Senario++;
    }
}
