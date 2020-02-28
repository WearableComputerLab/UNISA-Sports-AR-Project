using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRaycaster : MonoBehaviour
{
    public LayerMask mask;
    GameObject hitObject;

    private GameController gameController;
    private void Start()
    {
        gameController = GameObject.Find("Controller").GetComponent<GameController>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.transform.gameObject;
                IconBehaviour objBehaviour = hitObject.GetComponent<IconBehaviour>();
                ObserveObject(objBehaviour);
            }
        }
    }

    private void ObserveObject(IconBehaviour objBehaviour)
    {
        if (gameController.CurrentInteractionMode() == GameController.InteractionMode.Observing)
        {
            if (objBehaviour != null)
            {
                objBehaviour.ObserveWide();
            }
        }
        else if (gameController.CurrentInteractionMode() == GameController.InteractionMode.FirstPerson)
        {
            if (objBehaviour != null)
            {
                objBehaviour.ObserveFirstPerson();
            }
        }
    }
}
