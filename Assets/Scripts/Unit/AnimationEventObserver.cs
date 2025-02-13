using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventObserver : MonoBehaviour
{
    public void CallAttackEvent()
    {
        if (transform.parent.GetComponent<UnitController>() != null)
        {
            transform.parent.GetComponent<UnitController>().OnAttackEvent();
        }
    }
}
