using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionController : MonoBehaviour
{
    private bool used;

    private void Death()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        if(GetType() == typeof(CherryController))
        {
            SoundManager.soundManager.CherryAudio();
        }
        else
        {
            SoundManager.soundManager.GemAudio();
        }
        gameObject.GetComponent<Animator>().SetTrigger("get");
        used = true;
    }
}
