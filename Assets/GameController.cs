using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GameController : MonoBehaviour
{
    public enum InteractionMode { Observing, FirstPerson, Menu };
    private InteractionMode interactionMode = InteractionMode.Observing;

    public GameObject icon;
    public GameObject figure;
    public GameObject rewindText;

    private GameObject[] icons = new GameObject[6];
    private GameObject[] figures = new GameObject[6];

    private Vector3 distTravelledCurrentTurn = new Vector3(0, 0, 0);

    private int tupleIndex = 0;
    private bool isRewinding;
    private int rewindStartIndex = -1;

    private int fastestPlayerCode;

    private bool followModeOn = false;

    private bool playerUIActivated = false;
    private GameObject fastestPlayer;

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
        if (tupleIndex >= rewindStartIndex)
        {
            isRewinding = false;
            rewindStartIndex = -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rewind();
        }
        else
        {
            MoveAllFigures();
        }
        tupleIndex += 1;
        // Thread.Sleep(40);
    }

    public void MoveAllFigures()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] != null)
            {
                icons[i].GetComponent<IconBehaviour>().Move(i, tupleIndex);
            }
        }
    }

    public void Rewind()
    {
        if (rewindStartIndex == -1)
        {
            rewindStartIndex = tupleIndex;
        }
        isRewinding = true;
        tupleIndex -= Dimensions.rewindFactor;
        if (tupleIndex < 9)
        {
            tupleIndex = 9;
        }
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<IconBehaviour>().Teleport(tupleIndex);
        }
    }

    public void MoveFigure(GameObject figure, Vector3 pos, float speed)
    {
        figure.GetComponent<FigureBehaviour>().Move(pos, speed);
    }

    public void TeleportFigure(GameObject figure, Vector3 pos)
    {
        figure.GetComponent<FigureBehaviour>().Teleport(pos);
    }

    public void PlayerUIAutomation(int playerCode, Vector3 value)
    {
        if (playerCode == 0)
        {
            this.distTravelledCurrentTurn = new Vector2(0, 0);
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
                SetPlayerUIActivated(true);
            }
            else
            {
                SetPlayerUIActivated(false);
            }
        }
    }

    public GameObject FastestPlayer()
    {
        return fastestPlayer;
    }

    public bool FollowModeOn()
    {
        return followModeOn;
    }

    public void SetFollowModeOn(bool value)
    {
        followModeOn = value;
    }
    public bool PlayerUIActivated()
    {
        return playerUIActivated;
    }

    public void SetPlayerUIActivated(bool isActive)
    {
        playerUIActivated = isActive;
    }

    public InteractionMode CurrentInteractionMode()
    {
        return interactionMode;
    }

    public void SetInteractionMode(InteractionMode mode)
    {
        interactionMode = mode;
    }

    public bool IsRewinding()
    {
        return isRewinding;
    }

}
