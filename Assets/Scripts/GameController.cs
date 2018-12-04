using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Vector3 spawnValues; // Spawnien koordinaattien raja-arvot
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text hitpointsText;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text waveText;   // Teksti joka tulee tason alkaessa näytölle
    private PlayerController playerController;
    public Text shieldStrText;
    public Slider shieldStrSlider;
    public RectTransform newFillRect;

    private int hitpoints;
    private int score;
    private bool gameOver;
    private bool restart;

    //Wave counters

    public int waveCount;   // Monesko taso on käynnissä
    private int _diff;
    private int width = 3;
    private int height = 10;
    private int hazard_number; //how many hazards/wave

    private GameObject[] hazardsAsteroidLane; //Gameobject-array, syy: monia eri vihollisia
    private GameObject[] hazardsWarLane;
    private GameObject[] hazardsAlienLane;
    private GameObject[] hazardsCurrentLane;

    //hazards
    public GameObject asteroid_1;
    public GameObject asteroid_2;
    public GameObject asteroid_3;
    public GameObject enemyShip_1;
    public GameObject enemyShip_2;
    public GameObject pickup;   // Pickup

    public GameObject[,][] map;
    public GameObject[] tempList;
    private GameObject[] currentHazardList;
    System.Random rnd = new System.Random();

    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";

        playerController = GameObject.FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }


        // Setting shield slider to player current shield strength, then assigning sliders max value to shield max value
        shieldStrText.text = "Shield: " +  playerController.getShieldStr() + "%";
        shieldStrSlider.value = playerController.getShieldStr();
        shieldStrSlider.maxValue = 100;
        /*hitpoints = playerController.getHp();
        showHitpoints();*/


        score = 0;
        waveCount = 0;          // Wave count
        waveText.text = "";     // UI:hin tulostuva teksti
        UpdateScore();
        StartCoroutine(SpawnWaves());

        //Create Enemy list
        hazardsAsteroidLane = new GameObject[6];
        hazardsAsteroidLane[0] = asteroid_1;
        hazardsAsteroidLane[1] = asteroid_2;
        hazardsAsteroidLane[2] = asteroid_3;
        hazardsAsteroidLane[3] = enemyShip_1;
        hazardsAsteroidLane[4] = enemyShip_2;
        hazardsAsteroidLane[5] = pickup;   // Pickup

        hazardsCurrentLane = hazardsAsteroidLane;

    ////*** MAP
    //Create map and fill it with list of random hazards
        map = new GameObject[width, height][];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Create random hazards by difficulty
                map[j,i]=WaveList(i);
            }
        }
        Debug.Log("Map 0,0: " + map[0, 0]); //DEBUG
        Debug.Log("Map 0,1: " + map[1, 1]); //DEBUG
        Debug.Log("Map 0,2: " + map[2, 2]); //DEBUG
    }

    void Update()
    {
        if (restart==true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        shieldStrText.text = "Shield: " + playerController.getShieldStr() + "%";
        shieldStrSlider.value = playerController.getShieldStr();

        /*hitpoints = playerController.getHp();
        showHitpoints();*/
    }

    public GameObject[] WaveList(int diff)
    {
        hazard_number = 10;

        //Debug.Log("number of different enemies in this lane type: " + hazardsCurrentLane.Length); //DEBUG

        tempList = new GameObject[hazard_number];
        _diff = diff;

        for (int i = 0; i < hazard_number; i++)
        {
            double enemy = Mathf.Floor((rnd.Next(0, (hazardsCurrentLane.Length-1)*10) *(_diff / 5+1))/10);
            //Debug.Log("Pre-Enemy number: " + enemy); //DEBUG
            if (enemy < 0) {enemy = 0;}
            if (enemy > hazardsCurrentLane.Length-1) {enemy = hazardsCurrentLane.Length-1;}

            //Debug.Log("Final-Enemy number: " + enemy); //DEBUG

            //add hazard to list
            tempList[i] = hazardsCurrentLane[(int)enemy];
        }

        return tempList;
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (waveCount< hazard_number)
        {
            // Näyttää monesko taso menossa
            waveText.text = "Wave: " + waveCount;
            yield return new WaitForSeconds(2);
            waveText.text = "";

            //Create wave
            for (int i = 0; i < hazardCount; i++)
            {
                //GameObject hazard = hazards[Random.Range(0, hazards.Length)]; //mahdollistaa eri vihollisten spawnauksen: valitsee vihollisten listalta yhden vihollisen satunnaisesti
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
            /*
            Vector3 pickUpSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion pickUpSpawnRotation = Quaternion.identity;
            Instantiate(pickup, pickUpSpawnPosition, pickUpSpawnRotation);
            */

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

    /*void showHitpoints()
    {
        hitpointsText.text = "Hitpoints: " + playerController.getHp();
    }*/

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
