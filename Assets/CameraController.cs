using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    private bool followingTarget = false;
    private static Vector3 offset = new Vector3(0, -5, 10);

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
        Camera.main.transform.localPosition = offset * -1;
        this.target = target;
        followingTarget = true;
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
    }
}
