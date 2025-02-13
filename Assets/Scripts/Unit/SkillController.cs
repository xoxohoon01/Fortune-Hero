using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEditor.EditorTools;

public class SkillController : MonoBehaviour
{
    MeshFilter mesh;
    SkillData currentSkillData;
    GameObject targetObject;
    new Rigidbody rigidbody;

    private void NormalizeMeshSize(Vector3 targetSize)
    {
        mesh = GetComponentInChildren<MeshFilter>();
        if (mesh.sharedMesh != null)
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
            Destroy(gameObject);
        }
    }

    private void OnHit()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + (Vector3.up * (transform.localScale.y/2)), transform.localScale / 2, transform.rotation);
        
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject == targetObject)
            {
                targetObject.GetComponent<MonsterController>().hp -= 100;
                Destroy(gameObject);
            }
        }
    }

    public void Initailize(SkillData skillData, GameObject target = null)
    {
        currentSkillData = skillData;
        targetObject = target;

        Instantiate(Resources.Load(currentSkillData.prefabPath), transform);
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // 와이어 큐브를 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero + (Vector3.up * (transform.localScale.y / 2)), transform.localScale);
    }
}
