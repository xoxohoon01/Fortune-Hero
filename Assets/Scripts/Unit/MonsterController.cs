using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DataTable_FortuneHero;
using static UnityEngine.GraphicsBuffer;
using System.Threading;

public class MonsterController : UnitController
{
    Monster currentMonster;
    MonsterData currentMonsterData;

    NavMeshAgent agent;
    Animator animator;

    private Transform target;
    private bool isAttack;
    private float attackDelay = 0;

    public float hp;

    public void Initialize(MonsterData monster , int level = 1)
    {
        Monster newMonster = new Monster(monster.ID);
        newMonster.level = level;
        status = new Status(newMonster);
        currentMonsterData = monster;
        hp = currentMonsterData.hp;
        attackDelay = currentMonsterData.attackSpeed;

        ObjectPoolManager.Instance.GetObject(currentMonsterData.prefabPath, transform);
    }

    public void TargetFinding()
    {
        if (StageManager.Instance.monsterObjects.Count > 0)
        {
            GameObject firstTarget = null;
            float minDist = 0;
            for (int i = 0; i < StageManager.Instance.heroObjects.Length; i++)
            {
                if (StageManager.Instance.heroObjects[i] != null)
                {
                    if (firstTarget == null)
                    {
                        firstTarget = StageManager.Instance.heroObjects[i];
                        target = firstTarget.transform;
                        minDist = Vector3.Distance(transform.position, target.transform.position);
                    }
                    if (Vector3.Distance(transform.position, StageManager.Instance.heroObjects[i].transform.position) < minDist)
                    {
                        target = StageManager.Instance.heroObjects[i].transform;
                        minDist = Vector3.Distance(transform.position, target.position);
                    }
                }
            }
        }
        else
        {
            target = null;
            agent.isStopped = true;
            animator.SetBool("isMove", false);
            isAttack = false;
        }
    }

    public void PathFinding()
    {
        if (target != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) <= currentMonsterData.attackRange)
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isMove", false);
            isAttack = false;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("isMove", false);

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

            if (currentMonsterData.type == 1)
            {
                agent.avoidancePriority = 2;
            }
            else if (currentMonsterData.type == 2)
            {
                agent.avoidancePriority = 3;
            }
        }
    }

    public void Animation()
    {
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
                attackDelay = (1 / currentMonsterData.attackSpeed);
            }
        }

        if (!StageManager.Instance.isStart)
            animator.ResetTrigger("Attack");

        attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0);
    }

    public override void OnAttackEvent()
    {
        if (target != null)
        {
            SkillData skill = DataManager.Instance.Skill.Get(currentMonsterData.skillList[0]);
            if (skill.type == 1)
            {
                HeroController hero = target.GetComponent<HeroController>();

                if (skill.damageType == 1)
                    hero.status.hp -= Mathf.Max(status.physicalDamage - status.physicalArmor, 0);
                else if (skill.damageType == 2)
                    hero.status.hp -= Mathf.Max(status.magicalDamage - status.magicalArmor, 0);
            }
            else if (skill.type == 2)
            {
                SkillController skillObject = ObjectPoolManager.Instance.GetObject("Units/Skill").GetComponent<SkillController>();
                skillObject.transform.position = transform.position + skill.offset;
                skillObject.Initailize(skill, this, target?.gameObject);
            }
        }
    }

    public override void IsAlive()
    {
        if (status.hp <= 0)
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
        TargetFinding();
        PathFinding();

        Animation();

        IsAlive();
    }
}
