﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float speed;
    private GameController gameController;
    private PlayerController playerController;

    public Texture[] textures;
    
    
    Renderer pickUpRenderer;

    public enum PickUpTypes
    {
        weapon, health, boost, score, random, total
    }
    public PickUpTypes type;


    void Start()
    {
        pickUpRenderer = GetComponent<Renderer>();

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

        if (type == PickUpTypes.random)
        {
            int chance = Random.Range(0, 4); 
            type = (PickUpTypes)chance;
            pickUpRenderer.material.mainTexture = textures[chance];
            
        }
    }

    // Player collects (aka 'Collides with other') pickup
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (type)
                {
                    case PickUpTypes.weapon:
                        Debug.Log("Player picked weapon");
                        upgradeWeapon();
                        break;
                    case PickUpTypes.health:
                        Debug.Log("Player picked hp");
                        health();
                        break;
                    case PickUpTypes.boost:
                        Debug.Log("Player picked boost");
                        speedBoost();
                        break;
                    case PickUpTypes.score:
                        Debug.Log("Player picked score");
                        gameController.AddScore(100);
                        break;
            }
            Destroy(gameObject);
        }
    }


    // Methods for connecting pickups to playerController
    public void speedBoost()
    {
        playerController.changeSpeed(1);
    }

    public void upgradeWeapon()
    {
        playerController.upgradeWeapon();
    }

    // Currently weapon pickup upgrades players weapon level
    /*
    public void newWeapon()
    {
        playerController.newWeapon();
    }
    */

    public void health()
    {
        playerController.increaseHp();
    }
}
