using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRaycaster : MonoBehaviour
{
    public LayerMask mask;
    GameObject hitObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.transform.gameObject;
            PlayerBehaviour objBehaviour = hitObject.GetComponent<PlayerBehaviour>();

            if (objBehaviour != null)
            {
                objBehaviour.DisplayDetails();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.transform.gameObject;
                PlayerBehaviour objBehaviour = hitObject.GetComponent<PlayerBehaviour>();

                if (objBehaviour != null)
                {
                    objBehaviour.Observe();
                }
            }
        }
    }
}
