using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text waveText;
    private PlayerController playerController;
    public Text shieldStrText;
    public Slider shieldStrSlider;
    //public RectTransform newFillRect;

    private int score;
    private bool gameOver;
    private bool restart;
    private bool laneSelection;

    public int waveCount;   // Monesko taso on käynnissä
    public int laneCount;   //Mikä linjasto valittu

    private int _diff;
    private int width = 3;
    private int height = 10;
    private int hazard_number; //how many hazards/wave

    private GameObject[] hazardsAsteroidLane; //Gameobject-array, syy: monia eri vihollisia
    private GameObject[] hazardsSpaceLane;
    private GameObject[] hazardsAlienLane;
    private GameObject[] hazardsCurrentLane;

    //hazards
    public GameObject asteroid_1;
    public GameObject asteroid_2;
    public GameObject asteroid_3;
    public GameObject enemyShip_1;
    public GameObject pickup;   // Pickup

    public GameObject[,][] map;
    public GameObject[] tempList;
    private GameObject[] currentHazardList;
    System.Random rnd = new System.Random();

    private Rect rect;
    public Texture mapSpaceTexture;
    public Texture mapPlayerTexture;
    public Texture mapAsteroidTexture;
    public Texture mapAlienTexture;
    public Rect[,] rectangles;
        
    public int mapSize;

    void Start()
    {
        gameOver = false;
        restart = false;
        laneSelection = false;
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

        score = 0;
        waveCount = 0;          // Wave count
        laneCount = 1;
        waveText.text = "";     // UI:hin tulostuva teksti
        UpdateScore();
        StartCoroutine(SpawnWaves());

        //Create Enemy list
        hazardsAsteroidLane = new GameObject[5];
        hazardsAsteroidLane[0] = asteroid_1;
        hazardsAsteroidLane[1] = asteroid_2;
        hazardsAsteroidLane[2] = asteroid_3;
        hazardsAsteroidLane[3] = enemyShip_1;
        hazardsAsteroidLane[4] = pickup;   // Pickup

        hazardsSpaceLane = new GameObject[4];
        hazardsSpaceLane[0] = enemyShip_1;
        hazardsSpaceLane[1] = enemyShip_1;
        hazardsSpaceLane[2] = enemyShip_1;
        hazardsSpaceLane[3] = pickup;   // Pickup

        hazardsAlienLane = new GameObject[4];
        hazardsAlienLane[0] = enemyShip_1;
        hazardsAlienLane[1] = pickup;
        hazardsAlienLane[2] = pickup;
        hazardsAlienLane[3] = pickup;   // Pickup

        hazardsCurrentLane = hazardsSpaceLane;

        #region Create table and fill it with list of waves
        //Create map and fill it with list of random hazards

        map = new GameObject[width, height][];
        rectangles = new Rect[width, height];
        
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        double screenWidth = 3 * mapSize;// - (width / 2)* mapSize;
        double screenHeight = 3 * mapSize;// - (height / 2) * mapSize;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Create random hazards by difficulty
                if (j == 0)
                {
                    hazardsCurrentLane = hazardsAsteroidLane;
                    Debug.Log("Lane: 1");
                }
                else if (j == 1)
                {
                    hazardsCurrentLane = hazardsSpaceLane;
                    Debug.Log("Lane: 2");
                }
                else if (j == 2)
                {
                    hazardsCurrentLane = hazardsAlienLane;
                    Debug.Log("Lane: 3");
                }

                map[j,i]=WaveList(i);
                //Create visual map
                rectangles[j, height - 1 - i] = new Rect((float)screenWidth/2 + j * mapSize, (float)screenHeight/2 + i * mapSize,  mapSize,  mapSize);
            }
        }
        #endregion
    }

    void OnGUI()
    {
        if (laneSelection==true)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //Create random hazards by difficulty
                    rect = rectangles[j, i];

                    if (j == laneCount && i == waveCount)
                    {
                        GUI.DrawTexture(rect, mapPlayerTexture);//,1,1,true,0,0,0,0,0,0);
                    }
                    else if(j == 1)
                    {
                        GUI.DrawTexture(rect, mapSpaceTexture);
                    }
                    else if(j == 0)
                    {
                        GUI.DrawTexture(rect, mapAsteroidTexture);
                    }
                    else if (j == 2)
                    {
                        GUI.DrawTexture(rect, mapAlienTexture);
                    }
                }
            }
        }

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

        //if (laneSelection == true)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                laneCount = 0;
                Debug.Log("Lane: 0");
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                laneCount = 1;
                Debug.Log("Lane: 1");
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                laneCount = 2;
                Debug.Log("Lane: 2");
            }
        }
        OnGUI(); //Update map to GUI
    }

    #region WaveList(int diff) - Generate list by diff/wave
    public GameObject[] WaveList(int diff)
    {
        hazard_number = 10;
        tempList = new GameObject[hazard_number];
        _diff = diff;

        for (int i = 0; i < hazard_number; i++)
        {
            double enemy = Mathf.Floor((rnd.Next(0, (hazardsCurrentLane.Length-(hazardsCurrentLane.Length/3)) *10) *(_diff / hazardsCurrentLane.Length / 2))/10);
            if (enemy < 0) {enemy = 0;}
            if (enemy > hazardsCurrentLane.Length-1) {enemy = hazardsCurrentLane.Length-1;}

            //add hazard to list
            tempList[i] = hazardsCurrentLane[(int)enemy];
        }

        return tempList;
    }
    #endregion

    IEnumerator SpawnWaves()
    {
        //laneSelection = true;
        yield return new WaitForSeconds(startWait);
        while (waveCount< hazard_number)
        {
            // Näyttää monesko taso menossa
            waveText.text = "Wave: " + (waveCount+1) + "\n" + "Select Path "+"\n"+"(Numbad 1-3)";
            laneSelection = true;

            yield return new WaitForSeconds(5);
            waveText.text = "";
            laneSelection = false;
            //Create wave
            for (int i = 0; i < hazardCount; i++)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    currentHazardList = map[laneCount, waveCount];
                    Instantiate(currentHazardList[i], spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
                yield return new WaitForSeconds(waveWait);


            waveCount++;

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

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
