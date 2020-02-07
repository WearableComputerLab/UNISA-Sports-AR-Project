using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool followingTarget = false;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            followingTarget = false;
        }

        if (followingTarget)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(-10, 20, -30), 0.125f);
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
