using UnityEngine;
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
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    private int hitpoints;
    private double shieldStr;
    public Slider shieldStrSlider;

    private void Start()
    {
        hitpoints = 20;
        shieldStr = 100.0;
    }

    // Kontrollien vaihtaminen / lisääminen: Edit > Project Settings > Input
    // Fire1: left ctrl & mouse left, Fire2: left alt & mouse right, Fire3: left shift & mouse mid
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        // Kutsuu kentän latausmetodia, jos suojakentän % on alle 100
        // Coroutine metodin sisäisen ajastimen takia
        if(shieldStr < 100.0)
        {
            StartCoroutine(rechargeShield());
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

    // 100% suojakenttä estää vahingon alukseen.
    // Osuma nollaa suojakentän
    public void damage(int damage)
    {
        //Debug.Log("damage: " + damage);
        if (shieldStr < 100.0)
        {
            //Debug.Log("hp: " + hitpoints);
            hitpoints = hitpoints - damage;
            //Debug.Log("hp: " + hitpoints);
        }
        shieldStr = 0.0;
    }

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

    public int getShieldStr()
    {
        return System.Convert.ToInt32(shieldStr);
    }

    public int getHp()
    {
        return hitpoints;
    }
}
