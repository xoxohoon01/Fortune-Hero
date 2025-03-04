using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DataTable_FortuneHero;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : UnitController
{
    MonsterData currentMonsterData;
    NavMeshAgent agent;
    Animator animator;

    private Transform target;
    private bool isAttack;
    private float attackDelay = 0;

    public float hp;

    public void Initialize(MonsterData monster)
    {
        currentMonsterData = monster;
        hp = currentMonsterData.hp;
        attackDelay = currentMonsterData.attackSpeed;

        ObjectPoolManager.Instance.GetObject(currentMonsterData.prefabPath, transform);
    }

    public void PathFinding()
    {
        float minDist = 100;
        for (int i = 0; i < StageManager.Instance.heroObjects.Length; i++)
        {
            if (StageManager.Instance.heroObjects[i] != null)
            {
                GameObject newTarget = StageManager.Instance.heroObjects[i];
                if (Vector3.Distance(transform.position, newTarget.transform.position) < minDist)
                {
                    agent.SetDestination(newTarget.transform.position);
                    minDist = Vector3.Distance(transform.position, newTarget.transform.position);
                    target = newTarget.transform;
                }
            }
        }
    }

    public void Animation()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("isMove", false);
            isAttack = true;

            if (currentMonsterData.type == 1)
            {
                agent.avoidancePriority = 1;
                
            }
            else if (currentMonsterData.type == 2)
            {
                agent.avoidancePriority = 2;
            }
        }
        else
        {
            animator.SetBool("isMove", true);
            isAttack = false;

            if (currentMonsterData.type == 1)
            {
                agent.avoidancePriority = 2;
            }
            else if (currentMonsterData.type == 2)
            {
                agent.avoidancePriority = 3;
            }
        }

        if (isAttack)
        {
            if (target != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
            }

            if (attackDelay <= 0)
            {
                animator.SetTrigger("Attack");
                attackDelay = currentMonsterData.attackSpeed;
            }
        }
    }

    public void IsAlive()
    {
        if (hp <= 0)
        {
            StageManager.Instance.monsterObjects.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        // 고정 초기화
        agent.angularSpeed = 500;

        // 캐릭터별 초기화
        agent.speed = currentMonsterData.moveSpeed;
        agent.stoppingDistance = currentMonsterData.attackRange;
        if (currentMonsterData.type == 1)
        {
            agent.acceleration = 50;
        }
        else if (currentMonsterData.type == 2)
        {
            agent.acceleration = 10;
        }
    }

    void Update()
    {
        PathFinding();
        Animation();

        IsAlive();
    }
}
