using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public Transform[] targets;
    public float minFOV = 50f;
    public float maxFOV = 85f;
    public float smoothSpeed = 2f;
    public float zoomSpeed = 8f;
    public Vector3 offset = new Vector3(0, 15, 10);
    public float minDistance = 5;
    public float maxDistance = 20;

    private Camera camera;

    public void Initialize()
    {
        camera = Camera.main;
    }

    public void SetTarget()
    {
        targets = new Transform[4];
        for (int i = 0;  i < StageManager.Instance.heroObjects.Length; i++)
        {
            if (StageManager.Instance.heroObjects[i] != null)
                targets[i] = StageManager.Instance.heroObjects[i].transform;
        }
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
            return targets[0].position;

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform target in targets)
            bounds.Encapsulate(target.position);

        return bounds.center;
    }

    // ĳ���� �� �ִ� �Ÿ� ���
    float GetGreatestDistance()
    {
        float maxDistance = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            for (int j = i + 1; j < targets.Length; j++)
            {
                float distance = Vector3.Distance(targets[i].position, targets[j].position);
                maxDistance = Mathf.Max(maxDistance, distance);
            }
        }
        return 2 + Mathf.Clamp(maxDistance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (targets == null || targets.Length == 0) return;

        Vector3 centerPoint = GetCenterPoint();
        float distance = GetGreatestDistance();

        // ī�޶� ������ ���� (Y�� �����ϰ� Z�� ����)
        camera.transform.position = centerPoint + offset;

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, Mathf.Clamp(distance * zoomSpeed, minFOV, maxFOV), Time.deltaTime * 2);
    }
}
