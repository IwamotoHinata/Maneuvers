using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarm : MonoBehaviour
{

    //public float Speed;
    //public float makebackLange;
    public int makecell;
    public GameObject Enemy;
    //public GameObject Player;
    //public GameObject NextPosition;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int r = makecell;r > 0;r--)
        {
            GameObject.Instantiate(Enemy);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        //transform.Translate(new Vector3(Speed, 0, 0) * Time.deltaTime);


    }
}
