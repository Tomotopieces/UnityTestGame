using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public GameObject player;
    private Animator playerAnimator;

    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
    }

    //进入梯子
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerAnimator.SetBool("climbable", true);
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
        }
    }
}
