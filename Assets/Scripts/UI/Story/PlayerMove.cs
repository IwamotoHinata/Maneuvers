using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float SPEED = 0.01f;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("IsUp",true);
            animator.SetBool("IsLeft",false);
            animator.SetBool("IsDown",false);
            animator.SetBool("IsRight",false);
            transform.position += Vector3.up*SPEED;
        }
        if(Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsLeft",true);
            animator.SetBool("IsUp",false);
            animator.SetBool("IsDown",false);
            animator.SetBool("IsRight",false);
            transform.position += Vector3.left*SPEED;
        }
        if(Input.GetKey(KeyCode.S))
        {
            animator.SetBool("IsDown",true);
            animator.SetBool("IsLeft",false);
            animator.SetBool("IsUp",false);
            animator.SetBool("IsRight",false);
            transform.position += Vector3.down*SPEED;
        }
        if(Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsRight",true);
            animator.SetBool("IsUp",false);
            animator.SetBool("IsLeft",false);
            animator.SetBool("IsDown",false);
            transform.position += Vector3.right*SPEED;
        }
    }
}
