using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Toimii tällä hetkellä kuten DestroyByContact, mutta vain osuessa pelaajan alukseen
public class Pickup : MonoBehaviour {

    private GameController gameController;
    private PlayerController playerController;

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.Log("Cannot Find 'GameController' script");
        }
        playerController = GameObject.FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player picked pickup");
            Destroy(gameObject);
        }
    }
}
