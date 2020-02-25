using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    private bool followingTarget = false;
    private static Vector3 posOffset = new Vector3(-2, -9, 2);
    private static Vector3 rotOffset = new Vector3(3, 5, 4);

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            LeaveFollowMode();
        }

        if (followingTarget)
        {
            Material newMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
            target.GetComponent<Renderer>().material = newMat;
            transform.LookAt(target.transform);
        }
    }

    public void EnterFollowMode(GameObject target)
    {
        Camera.main.transform.SetParent(target.transform);
        if (!GameController.FollowMode())
        {
            Camera.main.transform.localPosition += posOffset;
            Camera.main.transform.Rotate(rotOffset);
        }
        this.target = target;
        followingTarget = true;
        GameController.SetFollowMode(true);
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
        GameController.SetFollowMode(false);
    }
}
