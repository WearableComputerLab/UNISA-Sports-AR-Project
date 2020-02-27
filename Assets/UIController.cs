using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();
    }

    private void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button); // Maybe merge these into one and make it class scope?
        guiStyle.fontSize = 40;

        if (gameController.FollowModeOn())
        {
            CameraController camctrl = Camera.main.GetComponent<CameraController>();
            GameObject target = camctrl.Target();

            IconBehaviour fpb = target.GetComponent<IconBehaviour>();
            ActivatePlayerUI(fpb, gameController.FollowModeOn());

            if (GUI.Button(new Rect(280, 1200, 240, 150), "Main View", guiStyle))
            {
                Camera.main.GetComponent<CameraController>().LeaveFollowMode();
            }

            if (gameController.CurrentInteractionMode() == GameController.InteractionMode.FirstPerson)
            {
                if (GUI.Button(new Rect(500, 1200, 240, 150), "Zoom Out", guiStyle))
                {
                    gameController.SetInteractionMode(GameController.InteractionMode.Observing);
                    camctrl.EnterFollowMode(target);
                }
            }
            else if (gameController.CurrentInteractionMode() == GameController.InteractionMode.Observing)
            {
                if (GUI.Button(new Rect(500, 1200, 240, 150), "Zoom In", guiStyle))
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

            if (GUI.Button(new Rect(45, 1200, 170, 150), "Replay", guiStyle))
            {
                gameController.Rewind();
            }
        }

        if (gameController.IsRewinding()) // Convert to UI
        {
            GUI.Box(new Rect(640, 0, 150, 90), "Replay", guiStyle);
        }
        
    }

    private void ActivatePlayerUI(IconBehaviour ib, bool followModeOn)
    {
        if (Settings.annotate) // Ensure user has enabled annotations
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.box);
            GUIStyle stats = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 40;
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
}
