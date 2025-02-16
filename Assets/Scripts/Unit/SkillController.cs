using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class SkillController : MonoBehaviour
{
    MeshFilter mesh;
    ParticleSystem particle;

    UnitController sender;
    SkillData currentSkillData;
    GameObject targetObject;
    new Rigidbody rigidbody;

    bool isEnd;

    private void NormalizeMeshSize(Vector3 targetSize)
    {
        mesh = GetComponentInChildren<MeshFilter>();
        particle = GetComponentInChildren<ParticleSystem>();
        if (mesh?.sharedMesh != null)
        {
            Vector3 meshSize = mesh.sharedMesh.bounds.size;

            if (meshSize.x == 0 || meshSize.y == 0 || meshSize.z == 0)
                return;

            Vector3 scale = new Vector3(targetSize.x / meshSize.x, targetSize.y / meshSize.y, targetSize.z / meshSize.z);
            transform.localScale = scale;
        }
    }

    private void Move()
    {
        if (targetObject != null)
        {
            Vector3 targetVector = (targetObject.transform.position - transform.position).normalized;
            rigidbody.velocity = targetVector * currentSkillData.moveSpeed;
            transform.LookAt(transform.position + targetVector);
        }
        else
        {
            isEnd = true;
        }
    }

    private void OnHit()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + (Vector3.up * (transform.localScale.y/2)), transform.localScale / 2, transform.rotation);
        
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject == targetObject)
            {
                if (currentSkillData.damageType == 1)
                    targetObject.GetComponent<MonsterController>().hp -= currentSkillData.damage * sender.status.physicalDamage;
                if (currentSkillData.damageType == 2)
                    targetObject.GetComponent<MonsterController>().hp -= currentSkillData.damage * sender.status.magicalDamage;
                isEnd = true;
            }
        }
    }

    public void Initailize(SkillData skillData, UnitController sender, GameObject target = null)
    {
        currentSkillData = skillData;
        targetObject = target;
        this.sender = sender;

        ObjectPoolManager.Instance.GetObject(currentSkillData.prefabPath, transform);
        NormalizeMeshSize(currentSkillData.size);
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();

        OnHit();
        if (isEnd)
        {
            if (particle != null)
            {
                particle.Stop();
                if (!particle.IsAlive())
                    Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // 와이어 큐브를 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero + (Vector3.up * (transform.localScale.y / 2)), transform.localScale);
    }
}
