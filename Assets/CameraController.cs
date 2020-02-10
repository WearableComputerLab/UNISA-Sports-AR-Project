using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool followingTarget = false;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        Material origMat = Resources.Load("NormalSphere", typeof(Material)) as Material;

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            followingTarget = false;
            target.GetComponent<Renderer>().material = origMat;
        }

        if (followingTarget)
        {
            Material newMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
            target.GetComponent<Renderer>().material = newMat;
            gameObject.transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0, 20, -30), 0.125f);
    


    }

        else
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, new Vector3(7, 209, -309), 0.125f);
        }

    }

    public void Follow(GameObject target)
    {
        this.target = target;
        followingTarget = true;
    }

}
