using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject enterDialog;
    public int linkToScene;
    public float linkToX;
    public float linkToY;

    private bool getPlayer = false;
    private GameObject player;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && getPlayer)
        {
            getPlayer = false;
            DataManager.positionX = linkToX;
            DataManager.positionY = linkToY;
            SceneManager.LoadScene(linkToScene);
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
