using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionController : MonoBehaviour
{
    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<Animator>().SetTrigger("get");
    }
}
