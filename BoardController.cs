using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单向板
public class BoardController : MonoBehaviour
{
    private BoxCollider2D box;
    private bool getPlayer;
    private bool standing;
    private bool passing;
    public Animator player;

    void Start()
    {
        box = gameObject.GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (standing)
        {
            if (player.GetBool("crouching"))
            {
                passing = true;
                box.isTrigger = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.GetBool("crouching"))
        {
            passing = true;
        }

        if (collision.tag == "Player")
        {
            if (!getPlayer && collision.transform.position.y > transform.position.y)
            {
                if (!passing)
                {
                    box.isTrigger = false;
                }
            }
            else
            {
                getPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getPlayer = false;
            box.isTrigger = true;
            passing = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            standing = true;
            getPlayer = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            standing = false;
            getPlayer = false;
            box.isTrigger = true;
            passing = false;
        }
    }
}
