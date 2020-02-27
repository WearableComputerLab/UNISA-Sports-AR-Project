using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameController : MonoBehaviour
{
    public GameObject icon;
    public GameObject figure;
    public GameObject rewindText;

    private static GameObject[] icons = new GameObject[6];
    private static GameObject[] figures = new GameObject[6];

    private static Vector3 distTravelledCurrentTurn = new Vector3(0, 0, 0);

    private int timeIndex = 0;
    private bool isRewinding;
    private int rewindStartIndex = -1;

    private static int fastestPlayerCode;

    private static float timer = 0;

    private static bool followModeOn = false;
    public enum TrackingMode { Observing, FirstPerson };
    public static TrackingMode trackingMode = TrackingMode.Observing;

    private static bool playerUIActivated = false;
    private static GameObject fastestPlayer;

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

        IconBehaviour ib;

        for (int i = 0; i < filePaths.Length; i++)
        {
            icons[i] = Instantiate(icon, new Vector3(Dimensions.runOnX, Dimensions.sphereElevation, Dimensions.runOnZ), Quaternion.identity);
            ib = icons[i].GetComponent<IconBehaviour>();
            ib.SetFilePath(filePaths[i]);
            ib.ReadFile();

            figures[i] = Instantiate(figure, new Vector3(Dimensions.runOnX, 0, Dimensions.runOnZ), Quaternion.identity);
            ib.figure = figures[i]; // assign a figure to the relevant icon

            figures[i].name = ib.Name() + " Figure";
            icons[i].name = ib.Name() + " Icon";
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timeIndex >= rewindStartIndex)
        {
            isRewinding = false;
            rewindStartIndex = -1;
        }

        if (isRewinding)
        {
            rewindText.SetActive(true);
        }
        else
        {
            rewindText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rewind();
        }
        else
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i] != null)
                {
                    icons[i].GetComponent<IconBehaviour>().Move(i, timeIndex);
                }
            }
        }
        timeIndex += 1;
        //      Thread.Sleep(40);
    }

    private void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);

        if (followModeOn)
        {
            CameraController camctrl = Camera.main.GetComponent<CameraController>();
            GameObject target = camctrl.Target();

            IconBehaviour fpb = target.GetComponent<IconBehaviour>();
            ActivatePlayerUI(fpb, followModeOn);

            if (GUI.Button(new Rect(280, 1200, 240, 150), "Main View", guiStyle))
            {
                Camera.main.GetComponent<CameraController>().LeaveFollowMode();
            }

            if (trackingMode == TrackingMode.FirstPerson)
            {
                if (GUI.Button(new Rect(500, 1200, 240, 150), "Zoom Out", guiStyle))
                {
                    trackingMode = TrackingMode.Observing;
                    camctrl.EnterFollowMode(target);
                }
            }
            else if (trackingMode == TrackingMode.Observing)
            {
                if (GUI.Button(new Rect(500, 1200, 240, 150), "Zoom In", guiStyle))
                {
                    trackingMode = TrackingMode.FirstPerson;
                    camctrl.EnterFirstPersonMode(target);
                }
            }
        }
        else
        {
            if (playerUIActivated)
            {
                IconBehaviour ib = fastestPlayer.GetComponent<IconBehaviour>();
                ActivatePlayerUI(ib, followModeOn);
            }

            if (GUI.Button(new Rect(45, 1200, 170, 150), "Replay", guiStyle))
            {
                Rewind();
            }
        }
    }

    private void ActivatePlayerUI(IconBehaviour ib, bool followModeOn)
    {
        if (Settings.annotate) // Ensure user has enabled annotations
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.box);
            GUIStyle stats = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 50;
            stats.fontSize = 25;

            if (followModeOn)
            {
                GUI.Box(new Rect(0, 0, 500, 100), "Following " + ib.Name(), guiStyle);
            }
            else
            {
                GUI.Box(new Rect(0, 0, 500, 100), ib.Name(), guiStyle);
            }
        }
    }

    public void Rewind()
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
        //        print("Rewinding from " + oldIndex + " to " + timeIndex);
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<IconBehaviour>().Teleport(timeIndex);
        }
    }

    public static void MoveFigure(GameObject figure, Vector3 pos, float speed)
    {
        figure.GetComponent<FigureBehaviour>().Move(pos, speed);
    }

    public static void TeleportFigure(GameObject figure, Vector3 pos)
    {
        figure.GetComponent<FigureBehaviour>().Teleport(pos);
    }

    public static void EvaluateGreatestDist(int playerCode, Vector3 value)
    {
        if (playerCode == 0)
        {
            distTravelledCurrentTurn = new Vector2(0, 0);
            fastestPlayerCode = 0;
        }

        if (value.magnitude > distTravelledCurrentTurn.magnitude)
        {
            distTravelledCurrentTurn = value;
            fastestPlayerCode = playerCode;
        }

        if (playerCode == (figures.Length - 1))
        {
            fastestPlayer = icons[fastestPlayerCode];

            if ((distTravelledCurrentTurn.x > Dimensions.UIRunThresholdX) || (distTravelledCurrentTurn.z > Dimensions.UIRunThresholdZ))
            {
                SetPlayerUIActivation(true);
            }
            else
            {
                SetPlayerUIActivation(false);
            }
        }
    }
    public static float Timer()
    {
        return timer;
    }

    public static bool FollowModeOn()
    {
        return followModeOn;
    }

    public static void SetFollowModeOn(bool value)
    {
        followModeOn = value;
    }

    public static void SetPlayerUIActivation(bool isActive)
    {
        playerUIActivated = isActive;
    }

}
