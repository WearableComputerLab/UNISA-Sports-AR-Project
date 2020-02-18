using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class SphereBehaviour : MonoBehaviour
{
    public string filePath;

    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again


    public bool isWatched = false;

    public string playerName;
    public GameObject figure;

    public void Move(Vector3 pos)
    {
        gameObject.transform.position = pos;
        gameObject.transform.LookAt(pos);
        gameObject.transform.localPosition += new Vector3(0, 60f, 0);
    }

    public void Observe()
    {
        GameObject trackedObj = Camera.main.GetComponent<CameraController>().target;
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if (trackedObj != null)
        {
            trackedObj.GetComponent<Renderer>().material = origMat;
        }

        Camera.main.GetComponent<CameraController>().LaunchFollowMode(figure);

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
