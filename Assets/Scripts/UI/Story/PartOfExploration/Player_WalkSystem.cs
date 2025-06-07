using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WalkSystem : MonoBehaviour
{

    private Animator anim;
    private float myspeed;
    private Vector3 latestPos;
    public float WalkChangeSpeed;

    private bool R;
    private bool L;
    private bool Up;
    private bool Down;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myspeed = ((this.transform.position - latestPos) / Time.deltaTime).magnitude;

        if (myspeed > WalkChangeSpeed)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        
            //左右両方に入力があるか、またはどちらもないか
        if ((L && R) || !(L || R))
        {
                //左右入力無し
                //上下入力確認
            if ((Up && Down) || !(Up || Down))
            {
                    //上下入力無し
                    //☆☆処理パターン何もせず☆☆

            }
            else
            {
                if (Up)
                {
                        //上入力アリ
                        //☆☆処理パターン上☆☆

                anim.SetBool("Up", true);
                anim.SetBool("Down", false);
                anim.SetBool("Right", false);
                anim.SetBool("Left", false);
                }
                else
                {
                        //下入力アリ
                        //☆☆処理パターン下☆☆

                anim.SetBool("Up", false);
                anim.SetBool("Down", true);
                anim.SetBool("Right", false);
                anim.SetBool("Left", false);
                }
            }

        }
        else
        {
                //左右入力有り
            if (L)
            {
                    //左
                    //上下入力確認
                if ((Up && Down) || !(Up || Down))
                {
                        //上下入力無し
                        //☆☆処理パターン左☆☆

                anim.SetBool("Up", false);
                anim.SetBool("Down", false);
                anim.SetBool("Right", false);
                anim.SetBool("Left", true);
                }
                else
                {
                    if (Up)
                    {
                            //上入力アリ
                            //☆☆左上☆☆

                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", false);
                    anim.SetBool("Left", true);
                    }
                    else
                    {
                            //下入力アリ
                            //☆☆左下☆☆

                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", false);
                    anim.SetBool("Left", true);
                    }
                }
            }
            else
            {
                    //右
                    //上下入力確認
                if ((Up && Down) || !(Up || Down))
                {
                        //上下入力無し
                        //☆☆処理パターン右☆☆

                 anim.SetBool("Up", false);
                 anim.SetBool("Down", false);
                 anim.SetBool("Right", true);
                anim.SetBool("Left", false);
                }
                else
                {
                    if (Up)
                    {
                            //上入力アリ
                            //☆☆右上☆☆

                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                    }
                    else
                    {
                            //下入力アリ
                            //☆☆右下☆☆
                    anim.SetBool("Up", false);
                    anim.SetBool("Down", false);
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                    }
                }
            }
        }
        //以下がボタンの作成ポジション
        //左
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            L = true;
        }
        else
        {
            L = false;
        }

        //右
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            R = true;
        }
        else
        {
            R = false;
        }

        //上
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Up = true;
        }
        else
        {
            Up = false;
        }

        //下
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Down = true;
        }
        else
        {
            Down = false;
        }


        latestPos = this.transform.position;

    }





}
