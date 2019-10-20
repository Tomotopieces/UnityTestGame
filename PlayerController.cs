using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool startup = true;

    private Rigidbody2D playerBody;
    private Animator animator;

    public AudioSource jumpAudio;
    public AudioSource hurtAudio;
    public AudioSource deathAudio;
    public AudioSource mainBGM;

    public LayerMask ground;
    public Transform topPoint;
    public Transform bottomPoint;

    public float speed;
    public float jumpForce;

    public Text CherryCounter;
    public Text GemCounter;

    public int cherryCounter;
    public int gemCounter;

    //是否受伤或死亡
    public bool isHurt;
    public bool isDead;

    void Start()
    {
        playerBody = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        //跳跃
        if (Input.GetButtonDown("Jump") && (animator.GetBool("idle") || animator.GetBool("climbingIdle") || Physics2D.OverlapCircle(bottomPoint.position, 0.2f, ground)))
        {
            jumpAudio.Play();
            playerBody.gravityScale = 2;
            animator.SetBool("idle", false);
            animator.SetBool("jumping", true);
            animator.SetBool("climbingIdle", false);
            animator.SetBool("climbing", false);
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce * Time.fixedDeltaTime);
        }
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

        if (isDead)
        {
            return;
        }

        if (isHurt)
        {
            //受伤恢复
            if (Mathf.Abs(playerBody.velocity.x) < 0.1 || transform.GetComponent<CircleCollider2D>().IsTouchingLayers(8))
            {
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);
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
                playerBody.velocity = new Vector2(HorizontalMove * speed * times * Time.fixedDeltaTime, playerBody.velocity.y);
            }
            else if (animator.GetBool("climbingIdle"))
            {
                transform.position = new Vector2(transform.position.x + HorizontalMoveRaw * 5 * Time.fixedDeltaTime, transform.position.y);
            }
            else
            {
                playerBody.velocity = new Vector2(HorizontalMove * speed * times * Time.fixedDeltaTime, playerBody.velocity.y);
            }
        }

        //朝向变化
        if (HorizontalMoveRaw != 0)
        {
            transform.localScale = new Vector3(HorizontalMoveRaw, 1, 1);
        }

        //下落
        if (playerBody.velocity.y < 0)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jumping", false);
            animator.SetBool("falling", true);
        }

        //落地
        if (animator.GetBool("falling") && (Mathf.Abs(playerBody.velocity.y) < 0.1 || transform.GetComponent<CircleCollider2D>().IsTouchingLayers(8)))
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            animator.SetBool("falling", false);
            animator.SetBool("idle", true);
        }

        //隘道判断
        if (!Physics2D.OverlapCircle(topPoint.position, 0.2f, ground))
        {
            //下蹲
            if (!(animator.GetBool("climbingIdle")) && Input.GetButton("Crouch"))
            {
                animator.SetBool("idle", false);
                animator.SetBool("crouching", true);
                head.enabled = false;
            }
            //起立
            else if(animator.GetBool("crouching"))
            {
                animator.SetBool("idle", true);
                animator.SetBool("crouching", false);
                head.enabled = true;
            }
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
                transform.position = new Vector2(transform.position.x, transform.position.y + VertialMoveRaw * 5 * Time.fixedDeltaTime);
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
            collision.tag = "Untagged";
            cherryCounter++;
            CherryCounter.text = cherryCounter.ToString();
        }

        //宝石
        if(collision.tag == "Gem")
        {
            collision.tag = "Untagged";
            gemCounter++;
            GemCounter.text = gemCounter.ToString();
        }

        //掉落死亡
        if(collision.tag == "DeathBoundary")
        {
            isDead = true;
            Invoke("Death", 2.5f);
            mainBGM.enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            animator.SetBool("hurting", true);
            deathAudio.Play();
            playerBody.velocity = new Vector2(0, 1000 * Time.fixedDeltaTime);
        }
    }

    //触发Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //触碰敌人
        if (collision.gameObject.tag == "Enemy")
        {
            //踩
            if (transform.position.y >= (collision.gameObject.transform.position.y + collision.gameObject.transform.GetComponent<BoxCollider2D>().size.y / 2))
            {
                animator.SetBool("idle", false);
                animator.SetBool("jumping", true);
                playerBody.velocity = new Vector2(playerBody.velocity.x, jumpForce * Time.fixedDeltaTime);
                collision.gameObject.GetComponent<Animator>().SetTrigger("death");
            }
            //受伤
            else
            {
                hurtAudio.Play();
                isHurt = true;
                playerBody.gravityScale = 2;
                animator.SetBool("idle", false);
                animator.SetBool("jumping", false);
                animator.SetBool("climbingIdle", false);
                animator.SetBool("climbing", false);
                animator.SetBool("hurting", true);
                playerBody.velocity = new Vector2((transform.position.x > collision.gameObject.transform.position.x ? 1 : -1) * 200 * Time.fixedDeltaTime, jumpForce * Time.fixedDeltaTime);
            }
        }
    }

    private void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

