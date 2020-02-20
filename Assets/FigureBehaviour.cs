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

    public void Move(Vector3 pos, float speed, float adjustedSpeed)
    {
        animator.SetFloat("Speed", adjustedSpeed);
        Vector3 newPos = Vector3.Lerp(gameObject.transform.position, pos, speed);
        var playerRotation = Quaternion.LookRotation(newPos - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, speed * Time.deltaTime);
        gameObject.transform.position = newPos;
    }

    

    public void Teleport(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
