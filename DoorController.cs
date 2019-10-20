using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject enterDialog;
    public int linkSceneNumber;
    public Transform linkToPoint;

    private bool getPlayer = false;
    private GameObject player;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && getPlayer)
        {
            getPlayer = false;
            SceneManager.LoadScene(linkSceneNumber);
        }
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if(collison.tag == "Player")
        {
            player = collison.gameObject;
            enterDialog.SetActive(true);
            getPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        if (collison.tag == "Player")
        {
            enterDialog.SetActive(false);
            getPlayer = false;
        }
    }
}
