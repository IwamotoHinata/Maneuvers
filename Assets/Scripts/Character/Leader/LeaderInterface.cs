using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderInterface : MonoBehaviour
{
    private float secondsultpoint = 1;
    private float waitgetultpointseconds = 1;
    [SerializeField] private float MaxultPoint;
    public float ultPoint { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetSecondsUltPoint());
    }

    //ウルトポイントを毎秒secondsultpointだけ増やす
    private IEnumerator GetSecondsUltPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitgetultpointseconds);
            GetUltPoint(secondsultpoint);
        }
    }

    //ウルトポイントをpluspoint分増やす
    public void GetUltPoint(float pluspoint)
    {
        if(ultPoint + pluspoint < MaxultPoint)
            ultPoint += pluspoint; 
        else  ultPoint = MaxultPoint;
    }

    //ウルトポイントをminuspoint分減らす
    public void UseUltPoint(float minuspoint)
    {
        if(ultPoint - minuspoint > 0)
            ultPoint -= minuspoint;
        else  ultPoint = 0;
    }
}
