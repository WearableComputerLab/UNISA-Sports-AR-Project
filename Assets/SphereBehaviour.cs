using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class SphereBehaviour : MonoBehaviour
{
    public string filePath;
    private string[][] data;
    private string[] lines;

    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again
    private float objPrevX;
    private float objPrevY;

    public bool isWatched = false;
    private bool firstClick;

    public string playerName;


    public void ReadFile()
    {
        TextAsset file = Resources.Load(filePath) as TextAsset;
        lines = file.text.Split('\n');
        data = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
        }
        playerName = data[5][0].Substring(8);
    }

    public void Observe()
    {
        GameObject trackedObj = Camera.main.GetComponent<CameraController>().target;
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if (trackedObj != null)
        {
            trackedObj.GetComponent<Renderer>().material = origMat;
        }

        Camera.main.GetComponent<CameraController>().Follow(gameObject);
        objPrevX = gameObject.transform.position.x;
        objPrevY = gameObject.transform.position.y;
        isWatched = true;
        firstClick = true;
    }

    public void DisplayDetails()
    {
        GameObject detailsBox = GameObject.Find("GameObject").GetComponent<FigureController>().playerDetails;
        TextBehaviour tb = detailsBox.GetComponent<TextBehaviour>();

        tb.activationTime = FigureController.timer;

        detailsBox.SetActive(true);
        tb.SetPlayer(gameObject);
        tb.SetTitle(playerName);

    }


}
