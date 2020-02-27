using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureBehaviour : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Move(Vector3 pos, float speed)
    {
        animator.SetFloat("Speed", speed);
        Vector3 newPos = Vector3.Lerp(gameObject.transform.position, pos, speed);

        if (((!float.IsNaN(newPos.x)) && (!float.IsNaN(newPos.y))) && (!float.IsNaN(newPos.z)))
        {
            gameObject.transform.LookAt(newPos);
            Quaternion playerRotation = Quaternion.LookRotation(newPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, speed * Time.deltaTime);// Smoothly rotate towards the target point.  
            gameObject.transform.position = newPos;
        }
    }

    public void Teleport(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
