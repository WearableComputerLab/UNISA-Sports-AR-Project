using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRaycaster : MonoBehaviour
{

    public LayerMask mask;

    GameObject hitObject;

        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.transform.gameObject;

                if (hitObject.GetComponent<PlayerBehaviour>() != null)
                {
                    hitObject.GetComponent<PlayerBehaviour>().OnRaycastHit();
                }
                
            }

 //           Debug.DrawRay(transform.position, ray.direction);
        }

       
    }


}
