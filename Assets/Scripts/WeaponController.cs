using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    // Luodaan muuttujat laukausta ja ampumista varten varten
    public GameObject shot; // Itse laukaus
    public Transform shotSpawn; // Laukauksen sijainti
    public float fireRate; // Kesto laukausten välissä (sek)
    public float delay; // Aika ennen ensimmäistä laukausta (sek)

    // Alustetaan äänet privatella ja startissa (kuten RigidBody)
    private AudioSource audioSource;
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", delay, fireRate); // Unityn sisäänrakennettu ominaisuus, 
                                                  // jolla kutsutaan metodia toistuvasti 
                                                  // (metodin nimi, odotusaika ennen ensimmäistä kutsumista sekunteissa, toistuvuus ko. ajan välein sekunteissa),
                                                  // mahdollista saada tulittamaan myös satunnaisen ajan kuluttua. 
	}

    // Luodaan laukauksen ja ampumisen metodi
    void Fire ()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation); // Luodaan laukaus
        audioSource.Play(); // Lisätään äänet laukaukseen
    }

}
