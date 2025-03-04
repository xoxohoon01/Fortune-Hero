using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    private new Camera camera;

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
        int count = 0;
        int firstSlot = -1;
        for (int i = 0; i < 4; i++)
        {
            if (targets[i] != null)
            {
                if (firstSlot == -1)
                    firstSlot = i;
                count++;
            }
        }

        if (count >= 1)
        {
            Bounds bounds = new Bounds(targets[firstSlot].position, Vector3.zero);
            for (int i = 0; i < 4; i++)
            {
                if (targets[i] != null)
                    bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }


    }

    // 캐릭터 간 최대 거리 계산
    float GetGreatestDistance()
    {
        float maxDistance = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                for (int j = i + 1; j < targets.Length; j++)
                {
                    if (targets[j] != null)
                    {
                        float distance = Vector3.Distance(targets[i].position, targets[j].position);
                        maxDistance = Mathf.Max(maxDistance, distance);
                    }
                }
            }
        }
        return 2 + Mathf.Clamp(maxDistance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (targets == null || targets.Length == 0) return;

        Vector3 centerPoint = GetCenterPoint();
        float distance = GetGreatestDistance();

        // 카메라 포지션 조정 (Y는 유지하고 Z만 변경)
        camera.transform.position = centerPoint + offset;

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, Mathf.Clamp(distance * zoomSpeed, minFOV, maxFOV), Time.deltaTime * 2);
    }
}
