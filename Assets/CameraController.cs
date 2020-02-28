using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject target;

    private bool followingTarget = false;
    private bool autoRotate;

    private static Vector3 posOffset = new Vector3(-130f, 100f, -130f);
    private static Vector3 rotOffset = new Vector3(20f, 0f, 0f);
    private static Vector3 lookAtOffset = new Vector3(0f, 55f, 0f);

    private static Vector3 rotOffsetFirstPerson = new Vector3((float)-19.5, 0, 0);

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            LeaveFollowMode();
        }

        if (followingTarget)
        {
            FollowTarget();
        }
    }

    public void EnterWideFollowMode(GameObject target)
    {
        autoRotate = true;

        if (gameController.FollowModeOn())
        {
            LeaveFollowMode();
        }

        Camera.main.transform.position = target.transform.position + posOffset;
        Camera.main.transform.SetParent(target.transform);

        this.target = target;
        followingTarget = true;
        gameController.SetFollowModeOn(true);
    }

    public void EnterFirstPersonMode(GameObject target)
    {
        autoRotate = true;

        if (gameController.FollowModeOn())
        {
            LeaveFollowMode();
        }

        Camera.main.transform.position = target.transform.position;
        Camera.main.transform.Rotate(rotOffsetFirstPerson);
        Camera.main.transform.SetParent(target.transform);

        this.target = target;
        followingTarget = true;
        gameController.SetFollowModeOn(true);
    }

    public void LeaveFollowMode()
    {
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;
        followingTarget = false;
        target.GetComponent<IconBehaviour>().SetIsWatched(false);
        target.GetComponent<Renderer>().material = origMat;
        Camera.main.transform.position = new Vector3(Dimensions.cameraDefaultX, Dimensions.cameraDefaultY, Dimensions.cameraDefaultZ);
        Camera.main.transform.rotation = Quaternion.Euler(Dimensions.cameraRotX, Dimensions.cameraRotY, Dimensions.cameraRotZ);
        Camera.main.transform.parent = null;
        gameController.SetFollowModeOn(false);
    }

    private void FollowTarget()
    {
        target.GetComponent<IconBehaviour>().ToggleMaterial(true);

        if (gameController.CurrentInteractionMode() == GameController.InteractionMode.Observing)
        {
            if (autoRotate)
            {
                transform.LookAt(target.transform.position + lookAtOffset);
                Camera.main.transform.Rotate(rotOffset);
            }
        }
    }

    public void RotateAroundPlayer(int direction)
    {
        // 0 rotates to the left, 1 to the right
        Vector3 axis = new Vector3(0, 0, 0);
        autoRotate = false;

        if (direction == 0)
        {
            axis = new Vector3(0, 1, 0);
        }
        else if (direction == 1)
        {
            axis = new Vector3(0, -1, 0);
        }
        gameObject.transform.RotateAround(target.transform.position, axis, 3000 * Time.deltaTime);
    }

    public GameObject Target()
    {
        return target;
    }
}