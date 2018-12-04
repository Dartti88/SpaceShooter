using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public float speed;
    private GameController gameController;
    private PlayerController playerController;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

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
        if (other.tag == "Player")
        {
            Debug.Log("Player picked pickup");
            Destroy(gameObject);
        }
    }

}