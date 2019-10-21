using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D enemyBody;
    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
    }

    //敌人进入死亡阶段
    private void Dying()
    {
        SoundManager.soundManager.EnemyDeathAudio();
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    //敌人死亡
    private void Death()
    {
        Destroy(gameObject);
    }
}
