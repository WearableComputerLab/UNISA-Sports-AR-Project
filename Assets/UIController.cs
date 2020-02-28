using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private GUIStyle titleStyle;
    private GUIStyle statsStyle;

    private void OnGUI()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();
        titleStyle = new GUIStyle(GUI.skin.button); // Maybe merge these into one and make it class scope?
        titleStyle.fontSize = 40;
        statsStyle = new GUIStyle(GUI.skin.label);
        statsStyle.fontSize = 25;

        if (gameController.FollowModeOn())
        {
            CameraController camctrl = Camera.main.GetComponent<CameraController>();
            GameObject target = camctrl.Target();

            IconBehaviour fpb = target.GetComponent<IconBehaviour>();
            ActivatePlayerUI(fpb, gameController.FollowModeOn());

            if (GUI.Button(new Rect(0, 300, 100, 100), "<", titleStyle))
            {
                camctrl.RotateAroundPlayer(0);
            }
            if (GUI.Button(new Rect(300, 300, 100, 100), ">", titleStyle))
            {
                camctrl.RotateAroundPlayer(1);
            }

            if (GUI.Button(new Rect(280, 450, 240, 150), "Main View", titleStyle))
            {
                Camera.main.GetComponent<CameraController>().LeaveFollowMode();
            }

            if (gameController.CurrentInteractionMode() == GameController.InteractionMode.FirstPerson)
            {
                if (GUI.Button(new Rect(580, 450, 240, 150), "Zoom Out", titleStyle))
                {
                    gameController.SetInteractionMode(GameController.InteractionMode.Observing);
                    camctrl.EnterWideFollowMode(target);
                }
            }
            else if (gameController.CurrentInteractionMode() == GameController.InteractionMode.Observing)
            {
                if (GUI.Button(new Rect(580, 450, 240, 150), "Zoom In", titleStyle))
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
        if (GUI.Button(new Rect(45, 450, 170, 150), "Replay", titleStyle))
        {
            gameController.Rewind();
        }
        if (gameController.IsRewinding()) // Convert to UI
        {
            GUI.Box(new Rect(570, 0, 200, 100), "Replaying", titleStyle);
        }
    }

    private void ActivatePlayerUI(IconBehaviour ib, bool followModeOn)
    {
        if (Settings.annotate) // Ensure user has enabled annotations
        {

            if (followModeOn)
            {
                GUI.Box(new Rect(0, 0, 500, 100), "Following " + ib.Name(), titleStyle);
            }
            else
            {
                GUI.Box(new Rect(0, 0, 500, 100), ib.Name(), titleStyle);
            }
        }
    }
}
