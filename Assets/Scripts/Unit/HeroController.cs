using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DataTable_FortuneHero;

public class HeroController : UnitController
{
    private Hero currentHero;
    private HeroData currentHeroData;
    public NavMeshAgent agent { get; private set; }
    private Animator animator;

    private Transform target;
    private bool isAttack;
    private float attackDelay;

    public float hp;

    public void Initialize(Hero hero)
    {
        status = new Status(hero);
        currentHero = hero;
        currentHeroData = DataManager.Instance.Hero.Get(currentHero.ID);
        Instantiate(Resources.Load<GameObject>(currentHeroData.prefabPath), transform);
    }

    public void TargetFinding()
    {
        if (StageManager.Instance.monsterObjects.Count > 0)
        {
            float minDist = Vector3.Distance(transform.position, StageManager.Instance.monsterObjects[0].transform.position);
            for (int i = 0; i < StageManager.Instance.monsterObjects.Count; i++)
            {
                if (StageManager.Instance.monsterObjects[i] != null)
                {
                    if (Vector3.Distance(transform.position, StageManager.Instance.monsterObjects[i].transform.position) < minDist)
                    {
                        target = StageManager.Instance.monsterObjects[i].transform;
                        minDist = Vector3.Distance(transform.position, target.position);
                    }
                }
            }
            if (target == null)
                target = StageManager.Instance.monsterObjects[0].transform;
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

            if (Vector3.Distance(transform.position, target.position) <= currentHeroData.attackRange)
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

            if (currentHeroData.type == 1)
            {
                agent.avoidancePriority = 1;

            }
            else if (currentHeroData.type == 2)
            {
                agent.avoidancePriority = 2;
            }
        }
        else
        {
            animator.SetBool("isMove", true);

            if (currentHeroData.type == 1)
            {
                agent.avoidancePriority = 2;
            }
            else if (currentHeroData.type == 2)
            {
                agent.avoidancePriority = 3;
            }
        }
    }

    public void Animation()
    {
        if (isAttack)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

            if (attackDelay <= 0)
            {
                animator.SetTrigger("Attack");
                attackDelay = currentHeroData.attackSpeed;
            }
        }

        attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0);
    }

    public override void OnAttackEvent()
    {
        if (target != null)
        {
            SkillData skill = DataManager.Instance.Skill.Get(currentHeroData.skillList[0]);
            if (skill.type == 1)
            {
                MonsterController monster = target.GetComponent<MonsterController>();
                monster.hp -= currentHeroData.physicalDamage;
            }
            else if (skill.type == 2)
            {
                GameObject skillObject = Instantiate(Resources.Load<GameObject>("Units/Skill"));
                skillObject.transform.position = transform.position + skill.offset;
                skillObject.GetComponent<SkillController>().Initailize(skill, this, target?.gameObject);
            }
        }
    }

    private void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        // 고정 초기화
        agent.angularSpeed = 500;

        // 캐릭터별 초기화
        agent.speed = currentHeroData.moveSpeed;
        agent.stoppingDistance = currentHeroData.attackRange;
        if (currentHeroData.type == 1)
        {
            agent.acceleration = 50;
        }
        else if (currentHeroData.type == 2)
        {
            agent.acceleration = 10;
        }
    }

    private void Update()
    {
        TargetFinding();
        PathFinding();

        Animation();
    }
}
