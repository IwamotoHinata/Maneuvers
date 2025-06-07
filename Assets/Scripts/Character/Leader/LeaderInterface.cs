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

    //�E���g�|�C���g�𖈕bsecondsultpoint�������₷
    private IEnumerator GetSecondsUltPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitgetultpointseconds);
            GetUltPoint(secondsultpoint);
        }
    }

    //�E���g�|�C���g��pluspoint�����₷
    public void GetUltPoint(float pluspoint)
    {
        if(ultPoint + pluspoint < MaxultPoint)
            ultPoint += pluspoint; 
        else  ultPoint = MaxultPoint;
    }

    //�E���g�|�C���g��minuspoint�����炷
    public void UseUltPoint(float minuspoint)
    {
        if(ultPoint - minuspoint > 0)
            ultPoint -= minuspoint;
        else  ultPoint = 0;
    }
}
