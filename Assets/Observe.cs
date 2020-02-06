using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observe : MonoBehaviour
{
    public Camera[] cameras;
    private bool cameraUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) && (!cameraUsed))
        {
            cameraUsed = true; 
            Instantiate(cameras[1], gameObject.transform.position, Quaternion.identity);

        }

        if((Input.GetKeyDown(KeyCode.Escape)) && (cameraUsed))
        {
            cameraUsed = false;
            cameras[1].enabled = false;
            cameras[0].enabled = true;
        }
    }
}
