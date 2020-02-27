using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject target;

    private bool followingTarget = false;

    private static Vector3 posOffset = new Vector3(-130f, 100f, -130f);
    private static Vector3 rotOffset = new Vector3(20f, 0f, 0f);
    private static Vector3 lookAtOffset = new Vector3(0f, 55f, 0f);

    private static Vector3 rotOffsetFirstPerson = new Vector3((float)-19.5, 0, 0);

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            LeaveFollowMode();
        }

        if (followingTarget)
        {
            target.GetComponent<IconBehaviour>().ToggleMaterial(true);

            if (GameController.trackingMode == GameController.TrackingMode.Observing)
            {
                transform.LookAt(target.transform.position + lookAtOffset);
                Camera.main.transform.Rotate(rotOffset);
            }
        }
    }

    public void EnterFollowMode(GameObject target)
    {
        if (GameController.FollowModeOn())
        {
            LeaveFollowMode();
        }

        Camera.main.transform.position = target.transform.position + posOffset;
        Camera.main.transform.SetParent(target.transform);

        this.target = target;
        followingTarget = true;
        GameController.SetFollowModeOn(true);
    }

    public void EnterFirstPersonMode(GameObject target)
    {
        if (GameController.FollowModeOn())
        {
            LeaveFollowMode();
        }

        Camera.main.transform.position = target.transform.position;
        Camera.main.transform.Rotate(rotOffsetFirstPerson);
        Camera.main.transform.SetParent(target.transform);

        this.target = target;
        followingTarget = true;
        GameController.SetFollowModeOn(true);

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
        GameController.SetFollowModeOn(false);
    }

    public GameObject Target()
    {
        return target;

    }
}
