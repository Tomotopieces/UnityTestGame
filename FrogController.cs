using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : EnemyController
{
    private bool startup = true;

    private int direction;
    private Transform LeftBoundary;
    private Transform RightBoundary;
    private float leftBoundary;
    private float rightBoundary;

    public float speed;
    public float jumpForce;

    protected override void Start()
    {
        base.Start();
        direction = -1;
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

    private void Movement()
    {
        //下落
        if (animator.GetBool("jumping"))
        {
            if(enemyBody.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }

        //落地
        if (animator.GetBool("falling"))
        {
            if(Mathf.Abs(enemyBody.velocity.x) < 0.1)
            {
                animator.SetBool("falling", false);
                animator.SetBool("idle", true);
            }
        }

        //切换方向
        if(transform.position.x < leftBoundary)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            enemyBody.velocity = new Vector2(-enemyBody.velocity.x, enemyBody.velocity.y);
            direction = 1;
        }

        if(transform.position.x > rightBoundary)
        {
            transform.localScale = new Vector3(1, 1, 1);
            enemyBody.velocity = new Vector2(-enemyBody.velocity.x, enemyBody.velocity.y);
            direction = -1;
        }
    }

    //起跳
    private void startJump()
    {
        if (animator.GetBool("idle"))
        {
            enemyBody.velocity = new Vector2(direction * speed * Time.deltaTime, jumpForce * Time.deltaTime);
            animator.SetBool("idle", false);
            animator.SetBool("jumping", true);
        }
    }
}
