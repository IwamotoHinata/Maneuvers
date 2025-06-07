using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkSystem : MonoBehaviour
{
    public float WalkChangeSpeed;
    private Animator anim;
    private Vector3 latestPos;
    private Vector3 speed;
    private float myspeeed;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        latestPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        myspeeed = ((this.transform.position - latestPos) / Time.deltaTime).magnitude;

        if (myspeeed > WalkChangeSpeed)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }


        //進行方向の確認
        //そもそも移動しているのか？
        if (!(this.transform.position == latestPos))
        {
            //進行方向が右か左か？
            if (this.transform.position.x - latestPos.x > 0)
            {
                //右
                //上下方向の速度に勝っているか？
                if (Math.Abs(this.transform.position.x - latestPos.x) > Math.Abs(this.transform.position.y - latestPos.y))
                {
                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                }
                else
                {
                    //上か下か？
                    if (this.transform.position.y - latestPos.y > 0)
                    {
                        //上
                        anim.SetBool("Up", true);
                        anim.SetBool("Down", false);
                        anim.SetBool("Right", false);
                        anim.SetBool("Left", false);
                    }
                    else
                    {
                        //下
                        anim.SetBool("Up", false);
                        anim.SetBool("Down", true);
                        anim.SetBool("Right", false);
                        anim.SetBool("Left", false);
                    }
                }
            }
            else
            {
                //左
                //上下方向の速度に勝っているか？
                if (Math.Abs(this.transform.position.x - latestPos.x) > Math.Abs(this.transform.position.y - latestPos.y))
                {
                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", false);
                    anim.SetBool("Left", true);
                }
                else
                {
                    //上か下か？
                    if (this.transform.position.y - latestPos.y > 0)
                    {
                        //上
                        anim.SetBool("Up", true);
                        anim.SetBool("Down", false);
                        anim.SetBool("Right", false);
                        anim.SetBool("Left", false);
                    }
                    else
                    {
                        //下
                        anim.SetBool("Up", false);
                        anim.SetBool("Down", true);
                        anim.SetBool("Right", false);
                        anim.SetBool("Left", false);
                    }
                }

            }

        }

        latestPos = this.transform.position;

    }
}
