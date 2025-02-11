using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTest : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                animator.SetBool("isMove", true);
            }
        }

        if (Vector3.Distance(transform.position, agent.destination) <= 0.2f)
        {
            animator.SetBool("isMove", false);
        }
    }
}
