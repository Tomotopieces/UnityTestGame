using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    private bool startup = true;
    private Animator animator;

    private int direction;
    private Rigidbody2D frogBody;
    private Transform LeftBoundary;
    private Transform RightBoundary;
    private float leftBoundary;
    private float rightBoundary;

    public float speed;
    public float jumpForce;

    void Start()
    {
        direction = -1;
        frogBody = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
        LeftBoundary = transform.GetChild(0);
        RightBoundary = transform.GetChild(1);
        leftBoundary = LeftBoundary.position.x;
        rightBoundary = RightBoundary.position.x;
        Destroy(LeftBoundary.gameObject);
        Destroy(RightBoundary.gameObject);
    }

    void FixedUpdate()
    {
        if (startup)
        {
            animator.SetBool("idle", true);
            startup = false;
        }
        Movement();
    }

    void Movement()
    {
        //起跳
        if (animator.GetBool("idle"))
        {
            frogBody.velocity = new Vector2(direction * speed * Time.deltaTime, jumpForce * Time.deltaTime);
            animator.SetBool("idle", false);
            animator.SetBool("jumping", true);
        }

        //下落
        if (animator.GetBool("jumping"))
        {
            if(frogBody.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }

        //落地
        if (animator.GetBool("falling"))
        {
            if(Mathf.Abs(frogBody.velocity.x) < 0.1)
            {
                animator.SetBool("falling", false);
                animator.SetBool("idle", true);
            }
        }

        //切换方向
        if(transform.position.x < leftBoundary)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            frogBody.velocity = new Vector2(-frogBody.velocity.x, frogBody.velocity.y);
            direction = 1;
        }

        if(transform.position.x > rightBoundary)
        {
            transform.localScale = new Vector3(1, 1, 1);
            frogBody.velocity = new Vector2(-frogBody.velocity.x, frogBody.velocity.y);
            direction = -1;
        }
    }
}
