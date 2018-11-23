using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject pickup;   // Pickup
    public Vector3 spawnValues; // Spawnien koordinaattien raja-arvot
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text waveText;   // Teksti joka tulee tason alkaessa näytölle

    private int score;
    private bool gameOver;
    private bool restart;

    //Wave counters
    
    public int waveCount;   // Monesko taso on käynnissä
    /*
    private SpaceController currentSpace;
    private GameObject[,] currentMap;
    private GameObject[] currentWave;
    private GameObject currentWaveHazard;//Mikä hazardi on vuorossa
    */

    private int _diff;
    private int width = 3;
    private int height = 10;
    private int hazard_number = 10;

    public GameObject[,][] map;
    public GameObject[] list;
    private GameObject[] currentHazardList;
    System.Random rnd = new System.Random();


    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";

        score = 0;
        waveCount = 0;          // Wave count
        waveText.text = "";     // UI:hin tulostuva teksti
        UpdateScore();
        StartCoroutine(SpawnWaves());

        //Get Map

        //currentSpace = GameObject.FindObjectOfType<SpaceController>();
        //currentMap = currentSpace.getMap();
        //currentMap= SpaceController.getMap(); //Errori Visual Studiossa ei haittaa. Tomii Unityssä normaalisti


        ////*** MAP
        //Create map and fill it with list of random hazards
        map = new GameObject[width, height][];
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Create random hazards by difficulty
                //GameObject[] lista = { pickup, pickup, asteroid, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid };
                //map[i, j] = lista;
                map[j,i]=WaveList(i);
            }
        }
    
        //map[0, 0] = new GameObject[] { pickup, pickup, pickup, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid };
        //map[0, 1] = new GameObject[] { pickup, pickup, asteroid, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid, pickup, pickup, asteroid, pickup, asteroid };
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public GameObject[] WaveList(int diff)
    {
        list = new GameObject[hazard_number + 1];
        _diff = diff;

        for (int i = 0; i < hazard_number; i++)
        {
            
            int enemy = rnd.Next(1, 3);
            //add hazard to list

            switch (enemy)
            {
                case 1: list[i] = pickup; break;
                case 2: list[i] = asteroid; break;
            }
            /*
            if (i % 2 == 0)
            {
                list[i] = pickup;
            }
            else
            {
                list[i] = asteroid;
            }
            */
        }

        return list;
    }

    IEnumerator SpawnWaves()
    {

        yield return new WaitForSeconds(startWait);
        while (true)
        {
            // Näyttää monesko taso menossa
            waveText.text = "Wave: " + waveCount;
            yield return new WaitForSeconds(2);
            waveText.text = "";


            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                currentHazardList = map[1, waveCount];
                Instantiate(currentHazardList[i], spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            //WaveCount++
            waveCount++;
            // Spawnataan pickup jokaisen aallon jälkeen
            Vector3 pickUpSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion pickUpSpawnRotation = Quaternion.identity;

            //Instantiate(pickup, pickUpSpawnPosition, pickUpSpawnRotation);

            if (gameOver == true)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }   
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}