using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    private int hitpoints;  // hullpoint
    private bool forceField; // Onko pelaaja ylipäätään hankkinut suojakenttää?

    // Pelin alussa 2 hp testailua varten
    private void Start()
    {
        hitpoints = 10;
        forceField = false;
    }

    // Kontrollien vaihtaminen / lisääminen: Edit > Project Settings > Input
    // Fire1: left ctrl & mouse left, Fire2: left alt & mouse right, Fire3: left shift & mouse mid
    // Muut Inputit tähän?
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
        
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    // Palauttaa hp:t, käytetään atm vain gamecontrollerissa
    public int getHp()
    {
        return hitpoints;
    }
    // Suojakentän latautumine ntime vai wave
    // Vähentää x hp
    public void damage(int damage)
    {
        // Suojakenttä? 
        if (forceField)
        {

        }

        else if (!forceField)
        {
            hitpoints = hitpoints - damage;
            Debug.Log("Alukeen osuma, hp: " + hitpoints);
        }
    }
}