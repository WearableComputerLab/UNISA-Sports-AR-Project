using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class IconBehaviour : MonoBehaviour
{
    public string filePath;
    private string[][] data;
    private string[] lines;
    private float XPos;
    private float YPos;
    private DateTime prevTime;
    private DateTime currentTime;
    private double timeTaken = 0;

    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again
    private float objPrevX;
    private float objPrevY;

    private float distTravelledX = 0;
    private float distTravelledY = 0;

    public bool isWatched = false;
    private bool firstClick;

    public void ReadFile()
    {
        TextAsset file = Resources.Load(filePath) as TextAsset;
        lines = file.text.Split('\n');
        data = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
        }
    }

    public void Move(int timeIndex)
    {
        if (timeIndex >= lines.Length)
        {
            timeIndex = 0;
        }

        if ((timeIndex >= 9) && (!String.IsNullOrEmpty(lines[timeIndex])))
        {
            if (data[timeIndex][0].Length == 7)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[timeIndex][0], "m:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }
            else if (data[timeIndex][0].Length == 8)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[timeIndex][0], "mm:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }

            if (data[timeIndex][5] != " ----")
            {
                XPos = getLat(Convert.ToDouble(data[timeIndex][5].Substring(1)));
            }
            else
            {
                XPos = 0;
            }

            if (data[timeIndex][6] != " ----")
            {
                YPos = getLong(Convert.ToDouble(data[timeIndex][6].Substring(1)));
            }
            else
            {
                YPos = 0;
            }

            Vector3 newPos = Vector3.Lerp(gameObject.transform.position, new Vector3(XPos, 12, YPos), (float)(Math.Sqrt(Math.Pow(XPos, 2) + Math.Pow(YPos, 2)) / timeTaken));

            distTravelledX += Mathf.Abs(gameObject.transform.position.x - objPrevX);
            distTravelledY += Mathf.Abs(gameObject.transform.position.y - objPrevY);

            if (isWatched)
            {

                if (((distTravelledX > 0.1) || (distTravelledY > 0.1)) || (firstClick))
                {
                    //gameObject.transform.LookAt(newPos);
                    float speed = 0.8f;

                    var playerRotation = Quaternion.LookRotation(newPos - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, speed * Time.deltaTime);


                    distTravelledX = 0;
                    distTravelledY = 0;
                    firstClick = false;
                }

                objPrevX = gameObject.transform.position.x;
                objPrevY = gameObject.transform.position.y;
            }
            gameObject.transform.position = newPos;

        }
    }

    public void Teleport(int timeIndex)
    {
        if ((timeIndex >= 9) && (!String.IsNullOrEmpty(lines[timeIndex])))
        {
            if (data[timeIndex][0].Length == 7)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[timeIndex][0], "m:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }
            else if (data[timeIndex][0].Length == 8)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[timeIndex][0], "mm:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }

            if (data[timeIndex][5] != " ----")
            {
                XPos = getLat(Convert.ToDouble(data[timeIndex][5].Substring(1)));
            }
            else
            {
                XPos = 0;
            }

            if (data[timeIndex][6] != " ----")
            {
                YPos = getLong(Convert.ToDouble(data[timeIndex][6].Substring(1)));
            }
            else
            {
                YPos = 0;
            }
            gameObject.transform.position = new Vector3(XPos, 12, YPos);
        }
    }
    public void Observe()
    {
        GameObject trackedObj = GameObject.Find("Main Camera").GetComponent<CameraController>().target;
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if (trackedObj != null)
        {
            trackedObj.GetComponent<Renderer>().material = origMat;
        }

        GameObject.Find("Main Camera").GetComponent<CameraController>().Follow(gameObject);
        objPrevX = gameObject.transform.position.x;
        objPrevY = gameObject.transform.position.y;
        isWatched = true;
        firstClick = true;
    }

    public void DisplayDetails()
    {
        GameObject detailsBox = GameObject.Find("GameObject").GetComponent<GameController>().playerDetails;
        detailsBox.GetComponent<TextBehaviour>().activationTime = GameController.timer;

        detailsBox.SetActive(true);
        detailsBox.GetComponent<TextBehaviour>().SetPlayer(gameObject);
        detailsBox.GetComponent<TextBehaviour>().setTitle(data[5][0].Substring(8));

    }

    private float getLong(double longCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (longCoord - Dimensions.centrePointLong) * Dimensions.scaleFactor;
        return (float)scaledCoord;
    }

    private float getLat(double latCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (latCoord + Dimensions.centrePointLat) * Dimensions.scaleFactor;
        return (float)scaledCoord;
    }

}
