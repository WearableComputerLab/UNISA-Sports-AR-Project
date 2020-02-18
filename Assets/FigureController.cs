using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FigureController : MonoBehaviour
{
    public GameObject playerSphere;
    public GameObject playerFigure;
    public GameObject playerDetails;

    GameObject[] playerSpheres = new GameObject[6];
    GameObject[] playerFigures = new GameObject[6];

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
            playerSpheres[i] = Instantiate(playerSphere);
            playerFigures[i] = Instantiate(playerFigure);

            fb = playerFigures[i].GetComponent<FigureBehaviour>();
            fb.filePath = filePaths[i];
            fb.ReadFile();
            fb.icon = playerSpheres[i];
            playerFigures[i].name = fb.playerName + " Figure";

            sb = playerSpheres[i].GetComponent<SphereBehaviour>();
            sb.figure = playerFigures[i];
            sb.playerName = fb.playerName;

            playerSpheres[i].name = sb.playerName + " Icon";
            playerSpheres[i].transform.localPosition += new Vector3(-0.4f, 1f, -0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
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
}
