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
    private int teamNo = 1;

    private float XPos;
    private float ZPos;
    // Measures how much distance relevant obj has travelled since camera has moved, should be 1 before camera moves again
    private float objPrevX;
    private float objPrevY;

    private float distTravelledX = 0; // Dist on a given turn
    private float distTravelledY = 0; // Dist on a given turn

    private float distTravelledXAcc = 0; // Dist accumulating to a threshold. Only move camera or rotate player if threshold reached, otherwise movements too small to account for
    private float distTravelledYAcc = 0; // Dist accumulating to a threshold. Only move camera or rotate player if threshold reached, otherwise movements too small to account for

    private DateTime prevTime;
    private DateTime currentTime;
    private double timeElapsed;

    private float speed;

    private bool isWatched = false;
    private bool isFirstClick;

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();
    }

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
            Vector3 newPos = Vector3.Lerp(gameObject.transform.position, GetNewPos(timeIndex), 1f * Time.deltaTime);

            SetSpeed(CalculateSpeed(newPos, gameObject.transform.position));

            // Tell fig to move
            gameController.MoveFigure(figure, new Vector3(newPos.x, 0, newPos.z), Speed());

            RecordDistanceTravelled();

            if (isWatched)
            {
                if (((distTravelledXAcc > 0.1) || (distTravelledYAcc > 0.1)) || (isFirstClick))
                {

                    Quaternion playerRotation = Quaternion.LookRotation(newPos - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Speed() * Time.deltaTime);

                    distTravelledXAcc = 0;
                    distTravelledYAcc = 0;
                    SetIsFirstClick(false);
                }

                objPrevX = gameObject.transform.position.x;
                objPrevY = gameObject.transform.position.y;
            }
            else
            {
                gameController.PlayerUIAutomation(playerCode, new Vector3(distTravelledX, distTravelledY));
                distTravelledX = 0;
                distTravelledY = 0;
            }

            if (((!float.IsNaN(newPos.x)) && (!float.IsNaN(newPos.y))) && (!float.IsNaN(newPos.z))) // Ensure values are valid
            {
                gameObject.transform.position = newPos;
            }
        }
    }

    private void RecordDistanceTravelled()
    {
        distTravelledX = Mathf.Abs(gameObject.transform.position.x - objPrevX);
        distTravelledY = Mathf.Abs(gameObject.transform.position.y - objPrevY);

        distTravelledXAcc += Mathf.Abs(gameObject.transform.position.x - objPrevX);
        distTravelledYAcc += Mathf.Abs(gameObject.transform.position.y - objPrevY);
    }

    public void Teleport(int timeIndex)
    {
        if ((timeIndex >= 9) && (!String.IsNullOrEmpty(lines[timeIndex])))
        {
            gameObject.transform.position = GetNewPos(timeIndex);
            gameController.TeleportFigure(figure, new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z));
        }
    }
        
    public string Name()
    {
        return lines[5].Substring(8);
    }

    public string Details()
    {
        return "";
    }

    private float Speed()
    {
        return speed;
    }
    private void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private float CalculateSpeed(Vector3 newPos, Vector3 oldPos)
    {
        return new Vector2(newPos.x - oldPos.x, newPos.z - oldPos.z).magnitude / (float)timeElapsed;
    }

    public void ObserveWide()
    {
        RestoreDefaultMaterial();
        Camera.main.GetComponent<CameraController>().EnterFollowMode(gameObject);
        InitializeObserveMode();
    }

    public void ObserveFirstPerson()
    {
        RestoreDefaultMaterial();
        Camera.main.GetComponent<CameraController>().EnterFirstPersonMode(gameObject);
        InitializeObserveMode();
    }

    private void InitializeObserveMode()
    {
        objPrevX = gameObject.transform.position.x;
        objPrevY = gameObject.transform.position.y;
        SetIsWatched(true);
        SetIsFirstClick(true);
    }

    private void RestoreDefaultMaterial() // Should probably be handled by the Gamectrler
    {
        GameObject trackedObj = Camera.main.GetComponent<CameraController>().Target();

        if (trackedObj != null)
        {
            trackedObj.GetComponent<IconBehaviour>().ToggleMaterial(false);
        }
    }

    public void ToggleMaterial(bool isSelected)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (isSelected)
        {
            renderer.material = Settings.selectedMat;
        }
        else
        {
            if (teamNo == 1)
            {
                renderer.material = Settings.team1Mat;
            }
            else if (teamNo == 2)
            {
                renderer.material = Settings.team2Mat;
            }
        }
    }

    public void SetIsWatched(bool isWatched)
    {
        this.isWatched = isWatched;
    }

    public void SetIsFirstClick(bool isFirstClick)
    {
        this.isFirstClick = isFirstClick;
    }

    public void SetFilePath(string filePath)
    {
        this.filePath = filePath;
    }

    private Vector3 GetNewPos(int timeIndex)
    {
        float XPos = GetNewXPos(timeIndex);
        float ZPos = GetNewZPos(timeIndex);

        return new Vector3(XPos, Dimensions.sphereElevation, ZPos);
    }

    private float GetNewXPos(int timeIndex)
    {
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
        return XPos;
    }

    private float GetNewZPos(int timeIndex)
    {
        if (data[timeIndex][6] != " ----")
        {
            ZPos = GetLong(Convert.ToDouble(data[timeIndex][6].Substring(1)));
            gameObject.SetActive(true);
            figure.SetActive(true);
        }
        else
        {
            ZPos = 0;
            gameObject.SetActive(false);
            figure.SetActive(false);
        }
        return ZPos;
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

    private double CalculateTimeElapsed(int timeIndex)
    {
        if (data[timeIndex][0].Length == 7)
        {
            currentTime = DateTime.ParseExact(data[timeIndex][0], "m:ss.ff", CultureInfo.InvariantCulture);
            timeElapsed = (currentTime - prevTime).TotalMilliseconds;
        }
        else if (data[timeIndex][0].Length == 8)
        {
            currentTime = DateTime.ParseExact(data[timeIndex][0], "mm:ss.ff", CultureInfo.InvariantCulture);
            timeElapsed = (currentTime - prevTime).TotalMilliseconds;
        }
        prevTime = currentTime;
        return timeElapsed;
    }
}