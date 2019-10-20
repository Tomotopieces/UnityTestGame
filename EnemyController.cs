using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D enemyBody;
    protected AudioSource deathAudio;
    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        deathAudio = gameObject.GetComponent<AudioSource>();
    }

    //敌人进入死亡阶段
    private void Dying()
    {
        deathAudio.Play();
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    //敌人死亡
    private void Death()
    {
        Destroy(gameObject);
    }
}
