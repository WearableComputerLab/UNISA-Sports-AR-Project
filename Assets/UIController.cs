using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private GUIStyle titleStyle;
    private GUIStyle statsStyle;

    float screenX = Screen.width;
    float screenY = Screen.height;
    float horizMiddle = Screen.width * 0.98f;

    private void OnGUI()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();

        titleStyle = new GUIStyle(GUI.skin.button); // Maybe merge these into one and make it class scope?
        titleStyle.fontSize = 75;

        statsStyle = new GUIStyle(GUI.skin.label);
        statsStyle.fontSize = 49;
        
        if (gameController.FollowModeOn())
        {
            CameraController camctrl = Camera.main.GetComponent<CameraController>();
            GameObject target = camctrl.Target();

            IconBehaviour fpb = target.GetComponent<IconBehaviour>();
            ActivatePlayerUI(fpb, gameController.FollowModeOn());

            if (GUI.Button(RotateButton(0), "<", titleStyle))
            {
                camctrl.RotateAroundPlayer(0);
            }
            if (GUI.Button(RotateButton(1), ">", titleStyle))
            {
                camctrl.RotateAroundPlayer(1);
            }

            if (GUI.Button(MainViewButton(), "Main View", titleStyle))
            {
                Camera.main.GetComponent<CameraController>().LeaveFollowMode();
            }

            if (gameController.CurrentInteractionMode() == GameController.InteractionMode.FirstPerson)
            {
                if (GUI.Button(ZoomButton(), "Zoom Out", titleStyle))
                {
                    gameController.SetInteractionMode(GameController.InteractionMode.Observing);
                    camctrl.EnterWideFollowMode(target);
                }
            }
            else if (gameController.CurrentInteractionMode() == GameController.InteractionMode.Observing)
            {
                if (GUI.Button(ZoomButton(), "Zoom In", titleStyle))
                {
                    gameController.SetInteractionMode(GameController.InteractionMode.FirstPerson);
                    camctrl.EnterFirstPersonMode(target);
                }
            }
        }
        else
        {
            if (gameController.PlayerUIActivated())
            {
                IconBehaviour ib = gameController.FastestPlayer().GetComponent<IconBehaviour>();
                ActivatePlayerUI(ib, gameController.FollowModeOn());
            }
        }
        if (GUI.Button(ReplayButton(), "Replay", titleStyle))
        {
            gameController.Rewind();
        }
        if (gameController.IsRewinding()) // Convert to UI
        {
            Notification("Replaying");
        }
    }

    private void ActivatePlayerUI(IconBehaviour ib, bool followModeOn)
    {
        if (Settings.annotate) // Ensure user has enabled annotations
        {

            if (followModeOn)
            {
                GUI.Box(PlayerUIRect(), "Following " + ib.Name(), titleStyle);
            }
            else
            {
                GUI.Box(PlayerUIRect(), ib.Name(), titleStyle);
            }
        }
    }

    private Rect PlayerUIRect()
    {
        return new Rect((screenX * 0.05f), (screenY * 0.05f), (screenX * 0.52f), (screenY * 0.1f));
    }

    private Rect RotateButton(int direction)
    {
        if (direction == 0)
        {
            return new Rect((screenX * 0.85f), (ToolbarY() - ToolbarSizeY() - (screenY * 0.02f)), screenX * 0.085f, screenY * 0.06f);
        }
        if(direction == 1)
        {
            return new Rect((screenX * 1.085f), (ToolbarY() - ToolbarSizeY() - (screenY * 0.02f)), screenX * 0.085f, screenY * 0.06f);
        }
        else
        {
            return new Rect(0, 0, 0, 0);
        }
    }

    private Rect ZoomButton()
    {
        return new Rect((screenX * 1.5f), ToolbarY(), (screenX * 0.32f), ToolbarSizeY());
    }

    private Rect MainViewButton()
    {
        return new Rect((screenX * 0.85f), ToolbarY(), (screenX * 0.32f), ToolbarSizeY());
    }

    private Rect ReplayButton()
    {
        return new Rect(screenX * 0.05f, ToolbarY(), (screenX * 0.32f), ToolbarSizeY());
    }

    private float ToolbarY()
    {
        return screenY * 0.35f;
    }

    private float ToolbarSizeY()
    {
        return screenY * 0.07f;
    }

    public void Notification(string message)
    {
        GUI.Box(new Rect((screenX * 1.4f), (screenY * 0.05f), (screenX * 0.24f), (screenY * 0.08f)), message, titleStyle);
    }

}
