using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBehaviour : MonoBehaviour
{
    public string filePath;
    private string[][] data;
    private string[] lines;
    private int maxLines;
    private int startingTuple = -1;

    //    [SerializeField] to display the values
    private float XPos;
    private float YPos;

    

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

    public void Move(int i)
    {
        if (i >= lines.Length)
        {
            i = 0;
        }

        if ((i >= 9) && (!String.IsNullOrEmpty(lines[i])))
        {
            if (data[i][5] != " ----")
            {
                XPos = getLat(Convert.ToDouble(data[i][5].Substring(1)));
            }

            if (data[i][6] != " ----")
            {
                YPos = getLong(Convert.ToDouble(data[i][6].Substring(1)));
            }

            if (((data[i][6] != " ----") && (data[i][5] != " ----")) && (startingTuple != -1))
            {
                startingTuple = i;
            }
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(XPos, 12, YPos), 10f);

        }

        i += 1;
        maxLines = lines.Length;
    }

    // WHY WON'T THIS WORK?

    public void rewindTenSec(int i)
    {
        float prevPosX;
        float prevPosY;

        if ((i >= 9) && (!String.IsNullOrEmpty(lines[i])))
        {
            if ((i >= 500) && (data[i - 500][5] != " ----"))
            {
                prevPosX = getLat(Convert.ToDouble(data[i - 500][5].Substring(1)));
                prevPosY = getLong(Convert.ToDouble(data[i - 500][6].Substring(1)));
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(prevPosX, 12, prevPosY), 10f);
            }

            else
            {
                if (startingTuple != -1)
                {
                    prevPosX = getLat(Convert.ToDouble(data[startingTuple][5].Substring(1)));
                    prevPosY = getLong(Convert.ToDouble(data[startingTuple][6].Substring(1)));
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(prevPosX, 12, prevPosY), 10f);
                }
            }
        }   
    }
 

    public void OnRaycastHit()
    {
        GameObject.Find("Main Camera").GetComponent<CameraController>().Follow(gameObject);

    }

    private float getLong(double longCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (longCoord - 138.495485) * 200000;
        return (float)scaledCoord;
    }


    private float getLat(double latCoord)
    {
        // Find the difference between the current point and center point, multiply by 200000 to scale it
        double scaledCoord = (latCoord + 34.879921) * 200000;

        return (float)scaledCoord;
    }



}
