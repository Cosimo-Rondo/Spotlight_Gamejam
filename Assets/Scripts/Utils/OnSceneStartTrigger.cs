using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSceneStartTrigger : MonoBehaviour
{
    public UnityEvent onSceneStart = new UnityEvent();
    void Start()
    {
        onSceneStart.Invoke();
    }
}
