using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToClickPoint();
        }

        bool isWalking = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
        
        if (isWalking)
        {
            // È¸Àü
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    void MoveToClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }
    }
}
