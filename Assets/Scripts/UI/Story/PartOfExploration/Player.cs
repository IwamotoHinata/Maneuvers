using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerMovepower;
    public float playerMaxspeed;
    
    private Vector3 latestPos;
    private float myspeeed;
    private Rigidbody2D rb;

    private Vector3 speed;
    private bool canWalk = true;
    private Vector3 initPos;
    
    private GameManager manager;
    private ScenarioAudio audio;
    private Fade fade;
    private Coroutine damageRoutine = null;


    private bool Up;
    private bool Down;
    private bool R;
    private bool L;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        manager = GameManager.Instance;
        //audio = manager.GetComponent<ScenarioAudio>();
        fade = Fade.Instance;
        initPos = transform.position;
        latestPos = transform.position;

        Up = false;
        Down = false;
        R = false;
        L = false;

    }
    
    private void FixedUpdate()
    {
        //新型の移動プログラム。これはルート２走法が可能
        //ステップ１速度が出過ぎていないか確認。この場所でまとめて行ってしまうと、反転の場合に不都合な気もするが空気抵抗で急減速するので一フレームしか差は出ない。
        if (rb.velocity.magnitude < playerMaxspeed && canWalk)
        {
            //左右両方に入力があるか、またはどちらもないか
            if (L && R ||!(L || R))
            {
                //左右入力無し
                //上下入力確認
                if (Up && Down ||!(Up || Down))
                {
                    //上下入力無し
                    //☆☆処理パターン何もせず☆☆
                }
                else
                {
                    if (Up)
                        //上入力アリ
                        //☆☆処理パターン上☆☆
                        rb.AddForce(new Vector2(0, playerMovepower));
                    else
                        //下入力アリ
                        //☆☆処理パターン下☆☆
                        rb.AddForce(new Vector2(0, playerMovepower * -1f));
                }
            }
            else
            {
                //左右入力有り
                if (L)
                {
                    //左
                    //上下入力確認
                    if (Up && Down ||!(Up || Down))
                    {
                        //上下入力無し
                        //☆☆処理パターン左☆☆
                        rb.AddForce(new Vector2(playerMovepower * -1f, 0));
                    }
                    else
                    {
                        if (Up)
                            //上入力アリ
                            //☆☆左上☆☆
                            rb.AddForce(new Vector2(playerMovepower * -0.71f, playerMovepower * 0.71f));
                        else
                            //下入力アリ
                            //☆☆左下☆☆
                            rb.AddForce(new Vector2(playerMovepower * -0.71f, playerMovepower * -0.71f));
                    }
                }
                else
                {
                    //右
                    //上下入力確認
                    if (Up && Down || !(Up || Down))
                    {
                        //上下入力無し
                        //☆☆処理パターン右☆☆
                        rb.AddForce(new Vector2(playerMovepower, 0));
                    }
                    else
                    {
                        if (Up)
                            //上入力アリ
                            //☆☆右上☆☆
                            rb.AddForce(new Vector2(playerMovepower * 0.71f, playerMovepower * 0.71f));
                        else
                            //下入力アリ
                            //☆☆右下☆☆
                            rb.AddForce(new Vector2(playerMovepower * 0.71f, playerMovepower * -0.71f));
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

    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Searcher"))
        {
            other.gameObject.GetComponent<Searcher>().StartSearch();
        }
        else if (other.CompareTag("Enemy") && damageRoutine == null && canWalk)
        {
            if (manager.CurrentState.Value == GameState.Exp6)
            {
                transform.position = initPos;
                audio.ShotSE("damage");
                return;
            }
            damageRoutine = StartCoroutine(DamageRoutine(other));
            audio.ShotSE("damage");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && damageRoutine == null && canWalk)
        {
            damageRoutine = StartCoroutine(DamageRoutine(other));
            audio.ShotSE("damage");
        }
    }

    private IEnumerator DamageRoutine(Collider2D other)
    {
        if (manager.CurrentState.Value == GameState.Exp5 || manager.CurrentState.Value == GameState.Exp6)
        {
            fade.SimpleFadeOut(0.2f);
            yield return new WaitForSeconds(0.5f);
            transform.position = initPos;
            fade.SimpleFadeIn(0.5f);
        }
        else
        {
            manager.DamageCount++;
        }
        
        Destroy(other.gameObject);
        yield return new WaitForSeconds(1.5f);
        damageRoutine = null;
    }

    public void StopWalk()
    {
        canWalk = false;
    }
}