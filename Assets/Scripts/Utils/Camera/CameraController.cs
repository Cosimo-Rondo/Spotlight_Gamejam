using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public CameraPoint initialCameraPoint;
    public Vector3 targetPos;
    public float smoothSpeed = 0.125f;
    public CameraPoint currentCameraPoint;
    public VisualElementAnimator switchSceneMask;

    void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        if (switchSceneMask == null)
        {
            switchSceneMask = transform.Find("SwitchSceneMask").GetComponent<VisualElementAnimator>();
        }
    }

    void Start()
    {
        if (initialCameraPoint != null)
        {
            SwitchToCameraPoint(initialCameraPoint, 0.5f);
        }
    }

    void Update()
    {
        Vector3 desiredPosition = new Vector3(targetPos.x, targetPos.y, cam.transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, smoothSpeed);
        cam.transform.position = smoothedPosition;
    }

    public void SwitchToCameraPoint(CameraPoint cameraPoint, float duration = 0f)
    {
        if (currentCameraPoint != null)
        {
            currentCameraPoint.OnLeaveThis();
        }
        cameraPoint.OnEnterThis();
        currentCameraPoint = cameraPoint;
        DOTween.To(() => targetPos.x, x => targetPos.x = x, cameraPoint.position.x, duration).SetEase(cameraPoint.switchEase);
        DOTween.To(() => targetPos.y, x => targetPos.y = x, cameraPoint.position.y, duration).SetEase(cameraPoint.switchEase);
        DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, cameraPoint.size, duration).SetEase(cameraPoint.switchEase);
    }

    public void MoveCamera(Vector3 deltaPos)
    {
        targetPos += deltaPos;
        targetPos = currentCameraPoint.ClampPosition(targetPos);
    }
}
