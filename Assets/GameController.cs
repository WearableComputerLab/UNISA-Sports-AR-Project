using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameController : MonoBehaviour
{
    public GameObject icon;
    public GameObject figure;
    public GameObject playerDetails;
    GameObject[] icons = new GameObject[6];
    private int timeIndex = 0;
    public bool isRewinding;
    private int rewindStartIndex = -1;
    public GameObject RewindText;

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

        IconBehaviour pb;

        for (int i = 0; i < filePaths.Length; i++)
        {
            icons[i] = Instantiate(icon);
            pb = icons[i].GetComponent<IconBehaviour>();
            pb.filePath = filePaths[i];
            pb.ReadFile();
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timeIndex >= rewindStartIndex)
        {
            isRewinding = false;
            rewindStartIndex = -1;
        }

        if(isRewinding)
        {
            RewindText.SetActive(true);
        }
        else
        {
            RewindText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rewind();
        }
        else
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].GetComponent<IconBehaviour>().Move(timeIndex);
            }
        }

        timeIndex += 1;
        Thread.Sleep(40);
    }

    


    // WAS WORKING, NOT ANYMORE :(
    private void Rewind()
    {
        if (rewindStartIndex == -1)
        {
            rewindStartIndex = timeIndex;
        }
        int oldIndex = timeIndex;
        isRewinding = true;
        timeIndex -= Dimensions.rewindFactor;
        if (timeIndex < 9)
        {
            timeIndex = 9;
        }
        print("Rewinding from " + oldIndex + " to " + timeIndex);
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<IconBehaviour>().Teleport(timeIndex);
        }

    }

}
