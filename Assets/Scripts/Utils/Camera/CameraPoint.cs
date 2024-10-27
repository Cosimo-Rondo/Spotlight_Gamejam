using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Vector2 position;
    public float size = 5f;
    public float switchDuration = 1f;
    public float cameraPanSpeed = 1f;
    public Ease switchEase = Ease.InOutQuad;
    public CameraController targetCamera;
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform topLimit;
    public Transform bottomLimit;

    void Awake()
    {
        position = transform.position;
        if (targetCamera == null)
        {
            targetCamera = Camera.main.GetComponent<CameraController>();
        }
    }

    public void OnLeaveThis()
    {
        if (leftLimit != null)
        {
            leftLimit.GetComponent<CameraLimit>().cameraPoint = null;
        }
        if (rightLimit != null)
        {
            rightLimit.GetComponent<CameraLimit>().cameraPoint = null;
        }
        if (topLimit != null)
        {
            topLimit.GetComponent<CameraLimit>().cameraPoint = null;
        }
        if (bottomLimit != null)
        {
            bottomLimit.GetComponent<CameraLimit>().cameraPoint = null;
        }
    }
    public void OnEnterThis()
    {
        if (leftLimit != null)
        {
            leftLimit.GetComponent<CameraLimit>().cameraPoint = this;
        }
        if (rightLimit != null)
        {
            rightLimit.GetComponent<CameraLimit>().cameraPoint = this;
        }
        if (topLimit != null)
        {
            topLimit.GetComponent<CameraLimit>().cameraPoint = this;
        }
        if (bottomLimit != null)
        {
            bottomLimit.GetComponent<CameraLimit>().cameraPoint = this;
        }
    }

    public void SwitchToThis()
    {
        targetCamera.SwitchToCameraPoint(this, switchDuration);
    }
    public void SwitchToThis(float duration)
    {
        targetCamera.SwitchToCameraPoint(this, duration);
    }
    public Vector3 ClampPosition(Vector3 pos)
    {
        Vector3 leftLimitPos = leftLimit != null ? leftLimit.position : Vector3.negativeInfinity;
        Vector3 rightLimitPos = rightLimit != null ? rightLimit.position : Vector3.positiveInfinity;
        Vector3 topLimitPos = topLimit != null ? topLimit.position : Vector3.positiveInfinity;
        Vector3 bottomLimitPos = bottomLimit != null ? bottomLimit.position : Vector3.negativeInfinity;
        return new Vector3(Mathf.Clamp(pos.x, leftLimitPos.x, rightLimitPos.x), Mathf.Clamp(pos.y, bottomLimitPos.y, topLimitPos.y), pos.z);
    }
    public void MoveCamera(Vector3 deltaPos)
    {
        targetCamera.MoveCamera(deltaPos * cameraPanSpeed * Time.deltaTime);
    }
    public void MoveCameraLeft()
    {
        MoveCamera(Vector3.left);
    }
    public void MoveCameraRight()
    {
        MoveCamera(Vector3.right);
    }
}
