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

    public Text hitpointsText;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text waveText;
    private PlayerController playerController;
    public Text shieldStrText;
    public Slider shieldStrSlider;
    public Text highScoreText;
    //public RectTransform newFillRect;

    private int hitpoints;
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
    public GameObject asteroid_4;
    public GameObject asteroid_5;
    public GameObject asteroid_6;
    public GameObject enemyShip_1;
    public GameObject enemyShip_2;
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
    private enum Lane
    {
        asteroid,
        space,
        alien,
        total
    }

    void Start()
    {
        gameOver = false;
        restart = false;
        laneSelection = false;
        restartText.text = "";
        gameOverText.text = "";
        highScoreText.text = "";

        playerController = GameObject.FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }


        // Setting shield slider to player current shield strength, then assigning sliders max value to shield max value
        shieldStrText.text = "Shield: " + playerController.getShieldStr() + "%";
        shieldStrSlider.value = playerController.getShieldStr();
        shieldStrSlider.maxValue = 100;
        /*hitpoints = playerController.getHp();
        showHitpoints();*/

        score = 0;
        waveCount = 0;          // Wave count
        laneCount = 1;
        waveText.text = "";     // UI:hin tulostuva teksti
        UpdateScore();
        StartCoroutine(SpawnWaves());

        //Create Enemy list
        hazardsAsteroidLane = new GameObject[6];
        hazardsAsteroidLane[0] = asteroid_1;
        hazardsAsteroidLane[1] = asteroid_2;
        hazardsAsteroidLane[2] = asteroid_3;
        hazardsAsteroidLane[3] = asteroid_4;
        hazardsAsteroidLane[4] = asteroid_5;
        hazardsAsteroidLane[5] = asteroid_6;

        hazardsSpaceLane = new GameObject[3];
        hazardsSpaceLane[0] = enemyShip_1;
        hazardsSpaceLane[1] = enemyShip_2;
        hazardsSpaceLane[2] = enemyShip_2;

        hazardsAlienLane = new GameObject[2];
        hazardsAlienLane[0] = enemyShip_1;
        hazardsAlienLane[1] = enemyShip_2;

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

                map[j, i] = WaveList(i);
                //Create visual map
                rectangles[j, height - 1 - i] = new Rect((float)screenWidth / 2 + j * mapSize, (float)screenHeight / 2 + i * mapSize, mapSize, mapSize);
            }
        }
        #endregion
    }

    //voidOnGui()
    #region 
    void OnGUI()
    {
        if (laneSelection == true)
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
                    else if (j == (int)Lane.space)
                    {
                        GUI.DrawTexture(rect, mapSpaceTexture);
                    }
                    else if (j == (int)Lane.asteroid)
                    {
                        GUI.DrawTexture(rect, mapAsteroidTexture);
                    }
                    else if (j == (int)Lane.alien)
                    {
                        GUI.DrawTexture(rect, mapAlienTexture);
                    }
                }
            }
        }

    }
    #endregion

    void Update()
    {


        if (restart == true)
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
                laneCount = (int)Lane.asteroid;
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                laneCount = (int)Lane.space;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                laneCount = (int)Lane.alien;
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
            double enemy = Mathf.Floor((rnd.Next(0, (hazardsCurrentLane.Length - (hazardsCurrentLane.Length / 3)) * 10) * ((_diff / (hazardsCurrentLane.Length - 1)) + 1)) / 10);
            if (enemy < 0) { enemy = 0; }
            if (enemy > hazardsCurrentLane.Length - 1) { enemy = hazardsCurrentLane.Length - 1; }

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
        while (waveCount < hazard_number)
        {
            // Näyttää monesko taso menossa
            waveText.text = "Wave: " + (waveCount + 1) + "\n" + "Select Path " + "\n" + "(Numbad 1-3)";
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

    /*void showHitpoints()
    {
        hitpointsText.text = "Hitpoints: " + playerController.getHp();
    }*/

    public void GameOver()
    {
        //HighScore(score);
        //highScoreText.text = "High score: " + PlayerPrefs.GetInt("Record");

        gameOverText.text = "Game Over!";
        gameOver = true;
        highScoreText.text = "High scores: \n" + ListHighScore(score);

    }

    // PlayerPrefs.SetInt("Key1", int);
    // Haetaan GetInt("Key1");
    // HasKey("Key1");

    // Simple highscore method that gets called when game ends.
    // Compares current score to previous (if there is one) and shows higher
    /*
    public void HighScore(int score)
    {

        if (!PlayerPrefs.HasKey("Record"))
        {
            PlayerPrefs.SetInt("Record", score);
        }
        else if (score > PlayerPrefs.GetInt("Record"))
        {
            PlayerPrefs.SetInt("Record", score);
        }
    }
    */

    // Returns max 5 highest scores in order
    public string ListHighScore(int score)
    {
        // Temporary list for previous scores and newest one. If there are less than 5 the rest are filled with 0's
        int[] scoresTemp = new int[6];
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("Score" + i))
            {
                scoresTemp[i] = PlayerPrefs.GetInt("Score" + i);
            }
            else
            {
                scoresTemp[i] = 0;
            }
        }
        scoresTemp[5] = score;

        // List that is going to 5 highest scores in descending order
        int[] scoresFinal = new int[5];
        int tempScore = 0;
        int tempIndex = 0;

        // Sorting algorithm
        for (int j = 0; j <5; j++)
        {
            for (int t = 0; t < scoresTemp.Length; t++)
            {
                if (tempScore < scoresTemp[t])
                {
                    tempScore = scoresTemp[t];
                    tempIndex = t;
                }
            }
            scoresFinal[j] = tempScore;
            tempScore = 0;
            scoresTemp[tempIndex] = 0;
        }

        // Scores are added to string and non zero one are saved to PlayerPrefs 
        string sortedHighScoreString = "";
        for (int i = 0; i < 5; i++)
        {
            if (scoresFinal[i] == 0)
            {
                sortedHighScoreString += i + ": -\n";
            }

            else
            {
                sortedHighScoreString += i + ": " + scoresFinal[i] + "\n";
                PlayerPrefs.SetInt("Score" + i, scoresFinal[i]);
            }
        }
        return sortedHighScoreString;
    }
}
