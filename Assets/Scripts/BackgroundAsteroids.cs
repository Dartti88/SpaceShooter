using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAsteroids : MonoBehaviour {

    public Vector3 spawnValuesLower;
    public Vector3 spawnValuesUpper;
    public float spawnWait;
    public GameObject asteroid_1;
    public GameObject asteroid_2;
    public GameObject asteroid_3;

    private GameObject[] asteroidList;

    System.Random rnd = new System.Random();

    // Use this for initialization
    void Start () {
        //GameObject[] asteroidList = { asteroid_1, asteroid_2, asteroid_3 };
        Debug.Log("Asteroidit start()");
        StartCoroutine(SpawnAsteroids());
    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnAsteroids()
    {
        Debug.Log("SpawnAsteroids()");
        for (int j = 0; j < 10; j++)
        {
            Vector3 spawnPositionLower = new Vector3(Random.Range(-spawnValuesLower.x, spawnValuesLower.x), spawnValuesLower.y, spawnValuesLower.z);
            Vector3 spawnPositionUpper = new Vector3(Random.Range(-spawnValuesUpper.x, spawnValuesUpper.x), spawnValuesUpper.y, spawnValuesUpper.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(asteroid_1, spawnPositionLower, spawnRotation);
            yield return new WaitForSeconds(spawnWait);

            Instantiate(asteroid_2, spawnPositionUpper, spawnRotation);
            yield return new WaitForSeconds(spawnWait);

        }
    }
}
