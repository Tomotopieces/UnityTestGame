using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private PolygonCollider2D ladderTop;
    public GameObject player;
    private Animator playerAnimator;

    void Start()
    {
        ladderTop = transform.gameObject.GetComponent<PolygonCollider2D>();
        playerAnimator = player.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        crouchCheck();
        pointCheck();
    }

    //使玩家下落
    private void crouchCheck()
    {
        if (player.GetComponent<CircleCollider2D>().IsTouchingLayers(9))
        {
            if (playerAnimator.GetBool("crouching"))
            {
                transform.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            }
        }
    }

    private void pointCheck()
    {
        if (transform.gameObject.GetComponent<BoxCollider2D>().OverlapPoint(player.transform.position))
        {
            playerAnimator.SetBool("climbable", true);
            ladderTop.isTrigger = true;
        }
    }

    //离开梯子
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 2;
            playerAnimator.SetBool("idle", true);
            playerAnimator.SetBool("climbingIdle", false);
            playerAnimator.SetBool("climbing", false);
            playerAnimator.SetBool("climbable", false);
            ladderTop.isTrigger = false;
        }
    }
}
