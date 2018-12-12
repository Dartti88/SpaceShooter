using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public int hp;
    private int startHp;
    public GameObject randomPickup;
    System.Random rnd = new System.Random();
    public Vector3 spawnValues;
    //public Transform pickupSpawn;

    // Use this for initialization
    void Start () {
        startHp = hp;
    }

	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerTag")
        {
            hp -= 1;
        }

        if (hp <= 0)
        {

            if (startHp > 20)
            {
                //Vector3 spawnPosition = new Vector3(transform.position, transform.position, transform.position);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(randomPickup, transform.position, spawnRotation);
            }
            else
            {
                // 10 % change
                int arpa = rnd.Next(0, 10);
                if (arpa <= 1)
                {
                    //Vector3 spawnPosition = new Vector3(transform.position, transform.position, transform.position);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(randomPickup, transform.position, spawnRotation);
                }
            }

            Destroy(this.gameObject);
        }
    }

    /*
    public void takeDamage(int damage)
    {
        hp -= damage;
        if (hp<=0)
        {
            Destroy(this.gameObject);
        }
    }

    public int getHp()
    {
        return hp;
    }
    */

}
