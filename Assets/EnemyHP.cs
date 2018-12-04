using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public int hp;

	// Use this for initialization
	void Start () {
		
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
            Destroy(this.gameObject);
            //Destroy(other.gameObject);
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
