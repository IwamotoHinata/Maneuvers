using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float girlMoveX;
    public float girlMoveY;
    public float girlMoveDistance;
    public GameObject gilr;
    private float girlWeitTime;

    public float enemyMoveX;
    public float enemyMoveY;
    public float enemyMoveDistance;
    public GameObject Enemy;
    private float enemyWeitTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        if (girlWeitTime > girlMoveDistance)
        {
            girlWeitTime = girlWeitTime - girlMoveDistance;
            gilr.transform.position = new Vector3(this.transform.position.x + Random.Range(-1 * girlMoveX, girlMoveX), this.transform.position.y + Random.Range(-1 * girlMoveY, girlMoveY), 0);

        }
        girlWeitTime = girlWeitTime + Time.deltaTime;

        if (enemyWeitTime > enemyMoveDistance)
        {
            enemyWeitTime = enemyWeitTime - enemyMoveDistance;
            Enemy.transform.position = new Vector3(this.transform.position.x + Random.Range(enemyMoveX * -1, enemyMoveX), this.transform.position.y + Random.Range(enemyMoveY * -1, enemyMoveY), 0);
        }
        enemyWeitTime = enemyWeitTime + Time.deltaTime;

    }

}