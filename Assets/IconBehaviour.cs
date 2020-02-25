using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class IconBehaviour : MonoBehaviour
{
    public GameObject figure;

    private string filePath;
    private string[][] data;
    private string[] lines;

    private float XPos;
    private float YPos;
    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again
    private float objPrevX;
    private float objPrevY;

    private float distTravelledX = 0; // Dist on a given turn
    private float distTravelledY = 0; // Dist on a given turn

    private float distTravelledXAcc = 0; // Dist accumulating to a threshold. Only move camera or rotate player if threshold reached, otherwise movements too small to account for
    private float distTravelledYAcc = 0; // Dist accumulating to a threshold. Only move camera or rotate player if threshold reached, otherwise movements too small to account for

    private DateTime prevTime;
    private DateTime currentTime;
    private double timeTaken = 0;

    private bool isWatched = false;
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

    public void Move(int playerCode, int timeIndex)
    {
        if (timeIndex >= lines.Length)
        {
            timeIndex = 0;
        }

        if ((timeIndex >= 9) && (!String.IsNullOrEmpty(lines[timeIndex])))
        {
            float speed = (float)(Math.Sqrt(Math.Pow(XPos, 2) + Math.Pow(YPos, 2)) / timeTaken);
            Vector3 newPos = Vector3.Lerp(gameObject.transform.position, GetNewPos(timeIndex), speed);

            // Tell fig to move
            GameController.MoveFigure(figure, new Vector3(newPos.x, 0, newPos.z), AdjustSpeed(gameObject.transform.position, newPos));

            distTravelledX = Mathf.Abs(gameObject.transform.position.x - objPrevX);
            distTravelledY = Mathf.Abs(gameObject.transform.position.y - objPrevY);

            distTravelledXAcc += Mathf.Abs(gameObject.transform.position.x - objPrevX);
            distTravelledYAcc += Mathf.Abs(gameObject.transform.position.y - objPrevY);

            GameObject detailsBox = GameObject.Find("Controller").GetComponent<GameController>().playerDetails;

            if (isWatched)
            {
                if (((distTravelledXAcc > 0.1) || (distTravelledYAcc > 0.1)) || (firstClick))
                {
                    speed = 0.8f;

                    Quaternion playerRotation = Quaternion.LookRotation(newPos - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, speed * Time.deltaTime);

                    distTravelledXAcc = 0;
                    distTravelledYAcc = 0;
                    firstClick = false;
                }

                objPrevX = gameObject.transform.position.x;
                objPrevY = gameObject.transform.position.y;
            }
            else
            {
                if ((distTravelledX > 5) || (distTravelledY > 5))
                {
                    GameController.EvaluateGreatestDist(playerCode, new Vector3(distTravelledX, distTravelledY));
                    detailsBox.SetActive(true);
                }
                else
                {
                    GameController.EvaluateGreatestDist(playerCode, new Vector3(0, 0));
                    detailsBox.SetActive(false);
                }

            }
            distTravelledX = 0;
            distTravelledY = 0;

            if (((!float.IsNaN(newPos.x)) && (!float.IsNaN(newPos.y))) && (!float.IsNaN(newPos.z))) // Ensure values are valid
            {
                gameObject.transform.position = newPos;
            }
        }
    }

    private float AdjustSpeed(Vector3 vec1, Vector3 vec2)
    {
        return new Vector2(vec1.x - vec2.x, vec1.z - vec2.z).magnitude;
    }

    public void Teleport(int timeIndex)
    {
        if ((timeIndex >= 9) && (!String.IsNullOrEmpty(lines[timeIndex])))
        {
            gameObject.transform.position = GetNewPos(timeIndex);
            GameController.TeleportFigure(figure, new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z));
        }
    }

    public void Observe()
    {
        RestoreDefaultMaterial();
        Camera.main.GetComponent<CameraController>().EnterFollowMode(gameObject);
        objPrevX = gameObject.transform.position.x;
        objPrevY = gameObject.transform.position.y;
        isWatched = true;
        firstClick = true;
    }

    public void ObserveFirstPerson()
    {
        RestoreDefaultMaterial();
        Camera.main.GetComponent<CameraController>().EnterFirstPersonMode(gameObject);
               
        objPrevX = gameObject.transform.position.x;
        objPrevY = gameObject.transform.position.y;
        isWatched = true;
        firstClick = true;
    }

    public void RestoreDefaultMaterial()
    {
        GameObject trackedObj = Camera.main.GetComponent<CameraController>().target;
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if (trackedObj != null)
        {
            trackedObj.GetComponent<Renderer>().material = origMat;
        }
    }

    public void DisplayDetails()
    {
        GameObject detailsBox = GameObject.Find("Controller").GetComponent<GameController>().playerDetails;
        TextBehaviour tb = detailsBox.GetComponent<TextBehaviour>();
        tb.SetActivationTime(GameController.Timer());
        detailsBox.SetActive(true);
        tb.SetPlayer(gameObject);
        tb.SetTitle(data[5][0].Substring(8));
    }

    public void SetIsWatched(bool isWatched)
    {
        this.isWatched = isWatched;
    }

    public void SetFilePath(string filePath)
    {
        this.filePath = filePath;
    }

    private Vector3 GetNewPos(int timeIndex)
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
            XPos = GetLat(Convert.ToDouble(data[timeIndex][5].Substring(1)));
            gameObject.SetActive(true);
            figure.SetActive(true);
        }
        else
        {
            XPos = Dimensions.fieldLowBoundX;
            gameObject.SetActive(false);
            figure.SetActive(false);
        }

        if (data[timeIndex][6] != " ----")
        {
            YPos = GetLong(Convert.ToDouble(data[timeIndex][6].Substring(1)));
            gameObject.SetActive(true);
            figure.SetActive(true);
        }
        else
        {
            YPos = 0;
            gameObject.SetActive(false);
            figure.SetActive(false);
        }
        return new Vector3(XPos, Dimensions.sphereElevation, YPos);
    }

    public string Name()
    {
        return lines[5].Substring(8);
    }

    private float GetLong(double longCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (longCoord - Dimensions.centrePointLong) * Dimensions.scaleFactor;
        return (float)scaledCoord;
    }

    private float GetLat(double latCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (latCoord + Dimensions.centrePointLat) * Dimensions.scaleFactor;
        return (float)scaledCoord;
    }
}
