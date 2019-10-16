using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : EnemyController
{
    public float speed;

    void FixedUpdate()
    {
        MoveMent();
    }

    private void MoveMent()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time) * speed * Time.deltaTime);
    }
}
