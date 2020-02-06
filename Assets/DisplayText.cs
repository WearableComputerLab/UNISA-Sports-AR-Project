using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    public GameObject profileCanvas;
    private bool canvasShown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if ((Input.GetMouseButtonDown(0)) && (!canvasShown))
        {
            canvasShown = true;
            Instantiate(profileCanvas, gameObject.transform);
            profileCanvas.GetComponent<UnityEngine.UI.Text>().text = "Hello";

        }
        else
        {
            canvasShown = false;
            profileCanvas.SetActive(false);

        }

    }
}
