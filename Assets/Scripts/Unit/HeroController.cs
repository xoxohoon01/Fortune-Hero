using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
    private Hero currentHero;
    public NavMeshAgent agent { get; private set; }
    private Animator animator;

    public void Initialize(Hero hero)
    {
        currentHero = hero;
    }

    private void Awake()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        // 고정 초기화
        agent.angularSpeed = 500;

        // 캐릭터별 초기화
        agent.speed = DataManager.Instance.Hero.Get(currentHero.ID).moveSpeed;
        agent.stoppingDistance = DataManager.Instance.Hero.Get(currentHero.ID).attackRange;
        if (DataManager.Instance.Hero.Get(currentHero.ID).type == 1)
        {
            agent.acceleration = 50;
        }
        else if (DataManager.Instance.Hero.Get(currentHero.ID).type == 2)
        {
            agent.acceleration = 10;
        }
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

        if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            animator.SetBool("isMove", false);
            agent.avoidancePriority = 3;
        }
        else
        {
            animator.SetBool("isMove", true);
            if (DataManager.Instance.Hero.Get(currentHero.ID).type == 1)
            {
                agent.avoidancePriority = 1;
            }
            else if (DataManager.Instance.Hero.Get(currentHero.ID).type == 2)
            {
                agent.avoidancePriority = 2;
            }
        }
    }
}
