using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class FigureBehaviour : MonoBehaviour
{
    public string filePath;
    private string[][] data;
    private string[] lines;
    private int maxLines;
    private int startingTuple = -1;

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

    public string playerName;
    private Animator animator;

    public GameObject icon;

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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
        playerName = data[5][0].Substring(8);
    }

    public void Move(int i)
    {
        if (i >= lines.Length)
        {
            i = 0;
        }

        if ((i >= 9) && (!String.IsNullOrEmpty(lines[i])))
        {
            if (data[i][0].Length == 7)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[i][0], "m:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }
            else if (data[i][0].Length == 8)
            {
                prevTime = currentTime;
                currentTime = DateTime.ParseExact(data[i][0], "mm:ss.ff", CultureInfo.InvariantCulture);
                timeTaken = (currentTime - prevTime).TotalMilliseconds;
            }

            if (data[i][5] != " ----")
            {
                XPos = GetLat(Convert.ToDouble(data[i][5].Substring(1)));
            }

            if (data[i][6] != " ----")
            {
                YPos = GetLong(Convert.ToDouble(data[i][6].Substring(1)));
            }

            if (((data[i][6] != " ----") && (data[i][5] != " ----")) && (startingTuple != -1))
            {
                startingTuple = i;
            }
            
            Vector3 newPos = Vector3.Lerp(gameObject.transform.position, new Vector3(XPos, Dimensions.figureElevation, YPos), (float)(Math.Sqrt(Math.Pow(XPos, 2) + Math.Pow(YPos, 2)) / timeTaken));
            // animator.SetFloat("Speed", (float)(Math.Sqrt(Math.Pow(XPos, 2) + Math.Pow(YPos, 2)) / timeTaken));
            gameObject.transform.LookAt(newPos);

            animator.SetFloat("Speed", newPos.magnitude);

            distTravelledX += Mathf.Abs(gameObject.transform.position.x - objPrevX);
            distTravelledY += Mathf.Abs(gameObject.transform.position.y - objPrevY);

            if ((distTravelledX > 0.1) || (distTravelledY > 0.1)) {

                icon.transform.position = gameObject.transform.position;
                icon.transform.localPosition += new Vector3(0, 60f, 0);

                var rotation = Quaternion.LookRotation(newPos - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Dimensions.speed * Time.deltaTime);

                distTravelledX = 0;
                distTravelledY = 0;
            }

            if (isWatched)
            {
                if (((distTravelledX > 0.1) || (distTravelledY > 0.1)) || (firstClick))
                {              
                    var rotation = Quaternion.LookRotation(newPos - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Dimensions.speed * Time.deltaTime);

                    distTravelledX = 0;
                    distTravelledY = 0;
                    firstClick = false;
                }
                objPrevX = gameObject.transform.position.x;
                objPrevY = gameObject.transform.position.y;
            }
            gameObject.transform.position = newPos;

        }
        i += 1;
        maxLines = lines.Length;
    }

    public void Rewind(int i, int changeFactor)
    {
        float prevPosX;
        float prevPosY;

        if ((i >= 9) && (!String.IsNullOrEmpty(lines[i])))
        {
            if ((i >= changeFactor) && (data[i - changeFactor][5] != " ----"))
            {
                prevPosX = GetLat(Convert.ToDouble(data[i - changeFactor][5].Substring(1)));
                prevPosY = GetLong(Convert.ToDouble(data[i - changeFactor][6].Substring(1)));
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(prevPosX, Dimensions.figureElevation, prevPosY), 10f);
            }

            else
            {
                if (startingTuple != -1)
                {
                    prevPosX = GetLat(Convert.ToDouble(data[startingTuple][5].Substring(1)));
                    prevPosY = GetLong(Convert.ToDouble(data[startingTuple][6].Substring(1)));
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(prevPosX, Dimensions.figureElevation, prevPosY), 10f);
                }
            }
        }
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
