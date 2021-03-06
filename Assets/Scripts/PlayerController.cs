﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public GameObject missile;
    public Transform shotSpawn;
    public float fireRate;
    private float fireRate2;    // Firerate of secondary weapon

    private float nextFire;
    private float nextFire2;

    public Text hitpointsText;
    private int hitpoints;
    private double shieldStr;
    public Slider shieldStrSlider;

    public bool secondWeapon; // If true enables firing from rightclick
    private int weaponLevel;

    private void Start()
    {
        hitpoints = 3;
        shieldStr = 100.0;
        secondWeapon = false;
        fireRate2 = fireRate * 4;
        weaponLevel = 1;
        hitpointsText.text = "Hitpoints: " + hitpoints;
    }

    // Kontrollien vaihtaminen / lisääminen: Edit > Project Settings > Input
    // Fire1: left ctrl & mouse left, Fire2: left alt & mouse right, Fire3: left shift & mouse mid
    void Update()
    { 
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            // Firing normal weapon checks weapon level 
            //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            Vector3 shotSpawnLeft = new Vector3(shotSpawn.position.x + 0.3f, 0.0f, shotSpawn.position.z);
            Vector3 shotSpawnRight = new Vector3(shotSpawn.position.x - 0.3f, 0.0f, shotSpawn.position.z);
            if (weaponLevel > 5)
            {
                weaponLevel = 5;
            }
            switch (weaponLevel)
            {
                case 1:
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    break;
                case 2:
                    Vector3 shotSpawnLeft2 = new Vector3(shotSpawn.position.x + 0.2f, 0.0f, shotSpawn.position.z);
                    Vector3 shotSpawnRight2 = new Vector3(shotSpawn.position.x - 0.2f, 0.0f, shotSpawn.position.z);
                    Instantiate(shot, shotSpawnLeft2, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnRight2, shotSpawn.rotation);
                    break;
                case 3:
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnLeft, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnRight, shotSpawn.rotation);
                    break;
                case 4:
                    Vector3 shotSpawnLeft4 = new Vector3(shotSpawn.position.x + 0.2f, 0.0f, shotSpawn.position.z);
                    Vector3 shotSpawnRight4 = new Vector3(shotSpawn.position.x - 0.2f, 0.0f, shotSpawn.position.z);
                    Instantiate(shot, shotSpawnLeft4, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnRight4, shotSpawn.rotation);

                    // Shot vasuri
                    // Shot oikea
                    Vector3 shotSpawnLeft5 = new Vector3(shotSpawn.position.x + 0.5f, 0.0f, shotSpawn.position.z - 0.2f);
                    Vector3 shotSpawnRight5 = new Vector3(shotSpawn.position.x - 0.5f, 0.0f, shotSpawn.position.z - 0.2f);
                    Instantiate(shot, shotSpawnLeft5, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnRight5, shotSpawn.rotation);


                    break;
                case 5:
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnLeft, shotSpawn.rotation);
                    Instantiate(shot, shotSpawnRight, shotSpawn.rotation);

                    // SHot vasuri
                    // Shot oikea
                    Vector3 shotSpawnLeft3 = new Vector3(shotSpawn.position.x + 0.5f, 0.0f, shotSpawn.position.z-0.2f);
                    Instantiate(shot, shotSpawnLeft3, shotSpawn.rotation);
                    Vector3 shotSpawnRight3 = new Vector3(shotSpawn.position.x - 0.5f, 0.0f, shotSpawn.position.z-0.2f);
                    Instantiate(shot, shotSpawnRight3, shotSpawn.rotation);

                    break;
            }
            

            GetComponent<AudioSource>().Play();
        }


        // If new weapon has been collected pressing mouse right will fire it
        // New weapon sounds needed
        if (secondWeapon == true && Input.GetButton("Fire2") && Time.time > nextFire2)
        {
            // Firerate for shots is 0.25s
            nextFire2 = Time.time + fireRate2;
            Instantiate(missile, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        // Kutsuu kentän latausmetodia, jos suojakentän % on alle 100
        // Coroutine metodin sisäisen ajastimen takia
        if(shieldStr < 100.0)
        {
            StartCoroutine(rechargeShield());
        }

        if (hitpoints > 3) { hitpoints = 3; } //Set max hitpoints
        hitpointsText.text = "Hitpoints: " + hitpoints;
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

    // 100% suojakenttä estää vahingon alukseen.
    // Osuma nollaa suojakentän
    public void damage(int damage)
    {
        //Debug.Log("damage: " + damage);
        if (shieldStr < 100.0)
        {
            //Debug.Log("hp: " + hitpoints);
            hitpoints = hitpoints - damage;
            hitpointsText.text = "Hitpoints: " + hitpoints;
            //Debug.Log("hp: " + hitpoints);
        }
        shieldStr = 0.0;
        if (weaponLevel > 1)
        {
            weaponLevel--;
        }
        if (speed > 8)
        {
            speed -= 1;
        }

    }

    #region shield charging
    // Kentän palautuminen
    // Kahden sekunnin viive latauksessa kun kenttä on nollassa
    public IEnumerator rechargeShield()
    {
        if (shieldStr == 0)
        {
            //hitpoints = hitpoints - damage;
            //Debug.Log("Alukeen osuma, hp: " + hitpoints);
            yield return new WaitForSeconds(2);
        }
        // Charging speed
        shieldStr += 0.2;
    }
    #endregion

    public void changeSpeed(int speedPlus)
    {
        // Debug.Log("pre speed:" + speed);
        if (speed < 13)
        {
            speed += speedPlus;
        }
        // Debug.Log("aft speed:" + speed);

    }


    public void upgradeWeapon()
    {
        if (weaponLevel < 5)
        {
            weaponLevel++;
        }
    }

    public void newWeapon()
    {
        secondWeapon = true;
        //Debug.Log("weapon2 = " + secondWeapon);
    }

    // Method that is called from pickup class when player collects hp pickup
    public void increaseHp()
    {
        hitpoints++;
        hitpointsText.text = "Hitpoints: " + hitpoints;

    }

    public int getShieldStr()
    {
        return System.Convert.ToInt32(shieldStr);
    }

    public int getHp()
    {
        return hitpoints;
    }
}
