using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    public GameObject oya;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void StartSearch()
    {
        oya.gameObject.GetComponent<Enemy>().Search();

    }
   



}
