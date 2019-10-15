using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool startup = true;

    private Rigidbody2D playerBody;
    private Animator animator;

    public float speed;
    public float jumpForce;

    public Text CherryCounter;
    public Text GemCounter;

    public int Cherry;
    public int Gem;

    public bool isHurt;

    void Start()
    {
        playerBody = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
    }

    void FixedUpdate()  //void Update()
    {
        if (startup)
        {
            animator.SetBool("idle", true);
            startup = false;
        }
        MoveMent();
    }

    //角色移动
    void MoveMent()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float HorizontalMoveRaw = Input.GetAxisRaw("Horizontal");
        float VertialMoveRaw = Input.GetAxisRaw("Vertical");

        BoxCollider2D head = GetComponent<BoxCollider2D>();

        if (isHurt)
        {
            //受伤恢复
            if (Mathf.Abs(playerBody.velocity.x) < 0.1)
            {
                animator.SetBool("hurting", false);
                animator.SetBool("idle", true);
                animator.SetFloat("running", 0);
                isHurt = false;
            }
            else
            {
                return;
            }
        }

        //横向移动
        if (HorizontalMove != 0)
        {
            animator.SetFloat("running", Mathf.Abs(HorizontalMove));
            float times = 1.0f;
            if (animator.GetBool("crouching"))
            {
                times = 0.2f;
                playerBody.velocity = new Vector2(HorizontalMove * speed * times * Time.deltaTime, playerBody.velocity.y);
            }
            else if (animator.GetBool("climbingIdle"))
            {
                transform.position = new Vector2(transform.position.x + HorizontalMoveRaw * 5 * Time.deltaTime, transform.position.y);
            }
            else
            {
                playerBody.velocity = new Vector2(HorizontalMove * speed * times * Time.deltaTime, playerBody.velocity.y);
            }
        }

        //朝向变化
        if (HorizontalMoveRaw != 0)
        {
            transform.localScale = new Vector3(HorizontalMoveRaw, 1, 1);
        }

        //跳跃
        if (Input.GetButtonDown("Jump") && (animator.GetBool("idle") || animator.GetBool("climbingIdle")))
        {
            playerBody.gravityScale = 2;
            animator.SetBool("idle", false);
            animator.SetBool("jumping", true);
            animator.SetBool("climbingIdle", false);
            animator.SetBool("climbing", false);
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce * Time.deltaTime);
        }

        //下落
        if (playerBody.velocity.y < 0)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jumping", false);
            animator.SetBool("falling", true);
        }

        //落地
        if (animator.GetBool("falling") && Mathf.Abs(playerBody.velocity.y) < 0.1)
        {
            animator.SetBool("falling", false);
            animator.SetBool("idle", true);
        }

        //下蹲
        if (!(animator.GetBool("climbingIdle")) && VertialMoveRaw < 0)
        {
            animator.SetBool("idle", false);
            animator.SetBool("crouching", true);
            head.enabled = false;
        }

        //起立
        if (animator.GetBool("crouching") && !(VertialMoveRaw < 0))
        {
            animator.SetBool("idle", true);
            animator.SetBool("crouching", false);
            head.enabled = true;
        }

        //上梯子
        if(animator.GetBool("climbable") && VertialMoveRaw > 0)
        {
            playerBody.gravityScale = 0;
            playerBody.velocity = new Vector2(0, 0);
            animator.SetBool("idle", false);
            animator.SetBool("jumping", false);
            animator.SetBool("falling", false);
            animator.SetBool("climbingIdle", true);
        }

        //爬梯子
        if(animator.GetBool("climbingIdle"))
        {
            if(VertialMoveRaw != 0)
            {
                animator.SetBool("climbing", true);
                transform.position = new Vector2(transform.position.x, transform.position.y + VertialMoveRaw * 5 * Time.deltaTime);
            }
            else
            {
                animator.SetBool("climbing", false);
            }
        }
    }

    //触发Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //樱桃
        if (collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            Cherry++;
            CherryCounter.text = Cherry.ToString();
        }

        //宝石
        if(collision.tag == "Gem")
        {
            Destroy(collision.gameObject);
            Gem++;
            GemCounter.text = Gem.ToString();
        }
    }

    //退出Trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    //触发Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //触碰敌人
        if (collision.gameObject.tag == "Enemy")
        {
            //踩
            if (animator.GetBool("falling") && transform.position.y >= (collision.gameObject.transform.position.y + collision.gameObject.transform.GetComponent<BoxCollider2D>().size.y / 2))
            {
                animator.SetBool("idle", false);
                animator.SetBool("jumping", true);
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce * Time.deltaTime);
                Destroy(collision.gameObject);
            }
            //受伤
            else
            {
                isHurt = true;
                playerBody.gravityScale = 2;
                animator.SetBool("idle", false);
                animator.SetBool("jumping", false);
                animator.SetBool("climbingIdle", false);
                animator.SetBool("climbing", false);
                animator.SetBool("hurting", true);
                playerBody.velocity = new Vector2((transform.position.x > collision.gameObject.transform.position.x ? 1 : -1) * 200 * Time.deltaTime, jumpForce * Time.deltaTime);
            }
        }
    }
}

