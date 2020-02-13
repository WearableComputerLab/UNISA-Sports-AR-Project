using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject playerDetails;
    GameObject[] players = new GameObject[6];
    private int timeIndex = 0;
    private int changeFactor = 700;

    // Start is called before the first frame update
    void Start()
    {
        string[] filePaths = new string[6];
        filePaths[0] = "Boak 2514 201005011355 (1)";
        filePaths[1] = "Gray 2525 201005011512";
        filePaths[2] = "Hartlett 2620 201005011354";
        filePaths[3] = "Rodan 2541 201005011354";
        filePaths[4] = "StewartP 2515 201005011354";
        filePaths[5] = "Thomas 2531 201005011355";

        PlayerBehaviour pb;

        for (int i = 0; i < filePaths.Length; i++)
        {
            players[i] = Instantiate(player);
            pb = players[i].GetComponent<PlayerBehaviour>();
            pb.filePath = filePaths[i];
            pb.ReadFile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerBehaviour>().rewind(timeIndex, changeFactor);
            }
            if (timeIndex > changeFactor)
            {
                timeIndex -= changeFactor;
            }
            else
            {
                timeIndex = 0;
            }
        }

        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerBehaviour>().Move(timeIndex);
            }
        }
        timeIndex += 1;
        Thread.Sleep(40);
    }
}
