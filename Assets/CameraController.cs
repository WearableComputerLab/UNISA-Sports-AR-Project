using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool followingTarget = false;
    public GameObject target;
    private static Vector3 offset = new Vector3(0, -12, 0);



    // Update is called once per frame
    void Update()
    {
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            followingTarget = false;
            target.GetComponent<SphereBehaviour>().isWatched = false;
            target.GetComponent<Renderer>().material = origMat;
            gameObject.transform.position = new Vector3(Dimensions.cameraDefaultX, Dimensions.cameraDefaultY, Dimensions.cameraDefaultZ);
            gameObject.transform.rotation = Quaternion.Euler(30.628f, 0, 0);
            gameObject.transform.parent = null;
        }

        if (followingTarget)
        {
            var sb = target.GetComponent<SphereBehaviour>();
            Material newMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
            target.GetComponent<Renderer>().material = newMat;
            Follow(target);
        }
    }

    public void LaunchFollowMode(GameObject target)
    {
        gameObject.transform.SetParent(target.GetComponent<FigureBehaviour>().transform);

        if (!followingTarget)
        {
            gameObject.transform.position -= offset;
        }

        this.target = target;
        followingTarget = true;       
    }

    public void Follow(GameObject figure)
    {
        FigureBehaviour fb = figure.GetComponent<FigureBehaviour>();

        if ((fb.distTravelledX > 0.1) && (fb.distTravelledX > 0.1))
        {
            transform.LookAt(figure.transform);
        }

    }
}
