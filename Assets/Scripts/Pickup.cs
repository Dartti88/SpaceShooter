using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Toimii tällä hetkellä kuten DestroyByContact, mutta vain osuessa pelaajan alukseen
public class Pickup : MonoBehaviour {

    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.Log("Cannot Find 'GameController' script");
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
