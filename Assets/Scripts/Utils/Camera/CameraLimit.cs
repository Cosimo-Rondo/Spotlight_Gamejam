using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CameraLimit : MonoBehaviour
{
    // Start is called before the first frame update
    //public CameraPoint cameraPoint;
    public Camera cam
    {
        get
        {
            return Camera.main;
            // if (cameraPoint == null) return null;
            // return cameraPoint.targetCamera.cam;
        }
    }
    public UnityEvent onLimitReached;
    public UnityEvent onLimitExceeded;
    enum LimitState
    {
        Reached,
        Exceeded,
        Unknown
    }
    LimitState limitState = LimitState.Unknown;
    void Update()
    {
        if (cam == null) return;
        if (Mathf.Abs(cam.transform.position.x - transform.position.x) < 0.01f)
        {
            if (limitState != LimitState.Reached)
            {
                //Debug.Log(gameObject.name + " Reached");
                onLimitReached.Invoke();
                limitState = LimitState.Reached;
            }
        }
        else if (Mathf.Abs(cam.transform.position.x - transform.position.x) > 0.01f)
        {
            if (limitState != LimitState.Exceeded)
            {
                //StartCoroutine(TestDelayTrigger());
                //Debug.Log(gameObject.name + " Exceeded");
                onLimitExceeded.Invoke();
                limitState = LimitState.Exceeded;
            }
        }
    }
    IEnumerator TestDelayTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        onLimitExceeded.Invoke();
        limitState = LimitState.Exceeded;
    }
}
