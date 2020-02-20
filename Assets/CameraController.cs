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
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            followingTarget = false;
            target.GetComponent<IconBehaviour>().setIsWatched(false);
            target.GetComponent<Renderer>().material = origMat;
            Camera.main.transform.position = new Vector3(Dimensions.cameraDefaultX, Dimensions.cameraDefaultY, Dimensions.cameraDefaultZ);
            Camera.main.transform.rotation = Quaternion.Euler(Dimensions.cameraRotX, Dimensions.cameraRotY, Dimensions.cameraRotZ);
            Camera.main.transform.parent = null;
        }

        if (followingTarget)
        {
            Material newMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
            target.GetComponent<Renderer>().material = newMat;
            transform.LookAt(target.transform);
        }
    }

    public void Follow(GameObject target)
    {
        Camera.main.transform.SetParent(target.transform);
        Camera.main.transform.localPosition = offset * -1;
        this.target = target;
        followingTarget = true;
       
    }
}
