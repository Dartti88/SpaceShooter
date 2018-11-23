using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Tommi
public class SpaceController : MonoBehaviour {

    public GameObject asteroid;
    public GameObject pickup;

    private int _diff;
    private int width=3;
    private int height=10;
    private int hazard_number = 10;

    private GameObject[,] map;

    // Use this for initialization
    void Start () {

        //Create map and fill it with list of random hazards
        map = new GameObject[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Create random hazards by difficulty
                WaveList(i);
            }
        }

    }

    public GameObject[,] getMap()
    {
        return map;
    }

    public GameObject getHazardFromList(GameObject list,int enemy)
    {
        return null;// list[enemy];
    }

    public GameObject getHazardList(int x_wave, int y_wave)
    {
        return map[x_wave, y_wave];
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void WaveList(int diff)
    {
        GameObject[] list = new GameObject[hazard_number+1];
        _diff = diff;

        for (int i = 0; i < hazard_number; i++)
        {
            //add hazard to list
            if (i % 2 == 0)
            {
                list[i] = pickup;
            }
            else
                {
                    list[i] = asteroid;
                }
        }
    }
}
