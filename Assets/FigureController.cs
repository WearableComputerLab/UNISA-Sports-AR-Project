using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FigureController : MonoBehaviour
{
    GameObject[] playerSpheres = new GameObject[6];
    GameObject[] playerFigures = new GameObject[6];

    public GameObject playerSphere;
    public GameObject playerFigure;
    public GameObject playerDetails;

    private int timeIndex = 0;
    private int changeFactor = 700;

    public static float timer = 0;

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

        FigureBehaviour fb;
        SphereBehaviour sb;

        for (int i = 0; i < filePaths.Length; i++)
        {
            playerFigures[i] = Instantiate(playerFigure);
            fb = playerFigures[i].GetComponent<FigureBehaviour>();
            fb.filePath = filePaths[i];
            fb.ReadFile();
            playerFigures[i].name = fb.playerName + " Figure";

            playerSpheres[i] = Instantiate(playerSphere);
            sb = playerSpheres[i].GetComponent<SphereBehaviour>();
            sb.figure = playerFigures[i];
            sb.playerName = fb.playerName;
            playerSpheres[i].name = sb.playerName + " Icon";

            fb.icon = playerSpheres[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rewind();
        }
        else
        {
            for (int i = 0; i < playerFigures.Length; i++)
            {
                playerFigures[i].GetComponent<FigureBehaviour>().Move(timeIndex);
            }
        }
        timeIndex += 1;
        //        Thread.Sleep(40);
    }

    private void Rewind()
    {
        for (int i = 0; i < playerFigures.Length; i++)
        {
            playerFigures[i].GetComponent<FigureBehaviour>().Rewind(timeIndex, changeFactor);
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
}    
