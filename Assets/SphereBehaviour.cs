using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class SphereBehaviour : MonoBehaviour
{
    public string filePath;

    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again
    private float objPrevX;
    private float objPrevY;

    public bool isWatched = false;
    private bool firstClick;

    public string playerName;
    public GameObject figure;

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
