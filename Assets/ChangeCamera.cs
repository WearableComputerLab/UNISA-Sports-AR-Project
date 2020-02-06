using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public Camera[] cameras;
    public Camera currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void changeCamera(int cameraNum)
    {
        currentCamera.enabled = false;
        cameras[cameraNum].enabled = true;
    }
}
