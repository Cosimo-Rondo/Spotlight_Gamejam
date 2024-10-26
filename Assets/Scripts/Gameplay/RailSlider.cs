using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RailSlider : MonoBehaviour
{
    public bool looped = false;
    private List<Vector2> vertices = new List<Vector2>();
    public LineRenderer lineRenderer;
    public Transform target;
    public int initialSegment = 0;
    [Range(0, 1)]
    public float initialSegmentOffset = 0f;
    private int currentSegment = 0;
    private float segmentOffset = 0f;
    private float cornerThreshold = 0.01f;
    private float moveSensitivity = 1f;
    public Transform frame;
    void Awake()
    {
        if (target == null) target = transform;
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            vertices.Add(lineRenderer.GetPosition(i));
        }
        currentSegment = initialSegment;
        segmentOffset = initialSegmentOffset * LengthOfSegment(initialSegment);
        frame = GetComponentInParent<Frame>().transform;
    }

    void Update()
    {
        UpdateVertices();
        UpdateTargetPos();
    }
    void UpdateVertices()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            vertices[i] = frame.TransformPoint(lineRenderer.GetPosition(i));
        }
    }
    public void UpdateTargetPos()
    {
        if (target == null) return;
        int nextIndex = NextIndex(currentSegment);
        float t = segmentOffset / Vector2.Distance(vertices[currentSegment], vertices[nextIndex]);
        Vector2 pos = Vector2.Lerp(vertices[currentSegment], vertices[nextIndex], t);
        target.position = new Vector3(pos.x, pos.y, target.position.z);
    }

    public void MoveAlongRail(Vector2 mouseDelta)
    {
        int nextIndex = NextIndex(currentSegment);
        int lastIndex = LastIndex(currentSegment);
        float segmentLength = LengthOfSegment(currentSegment);
        float projectionOnCurrentSegment = Vector2.Dot(mouseDelta, (vertices[nextIndex] - vertices[currentSegment]).normalized);
        if (segmentOffset < cornerThreshold)
        {
            if (lastIndex != -1)
            {
                float projectionOnLastSegment = Vector2.Dot(mouseDelta, (vertices[lastIndex] - vertices[currentSegment]).normalized);
                if (Mathf.Abs(projectionOnLastSegment) > Mathf.Abs(projectionOnCurrentSegment))
                {
                    currentSegment = lastIndex;
                    segmentOffset = LengthOfSegment(lastIndex) - projectionOnLastSegment * moveSensitivity;
                    if (segmentOffset < 0f) segmentOffset = 0f;
                    return;
                }
            }
        }
        else if (segmentOffset > segmentLength - cornerThreshold)
        {
            int nextNextIndex = NextIndex(nextIndex);
            if (nextNextIndex != -1)
            {
                float projectionOnNextSegment = Vector2.Dot(mouseDelta, (vertices[nextNextIndex] - vertices[nextIndex]).normalized);
                if (projectionOnNextSegment > -projectionOnCurrentSegment)
                {
                    currentSegment = nextIndex;
                    segmentOffset = projectionOnNextSegment * moveSensitivity;
                    if (segmentOffset > LengthOfSegment(nextIndex)) segmentOffset = LengthOfSegment(nextIndex);
                    return;
                }
            }
        }
        segmentOffset += projectionOnCurrentSegment * moveSensitivity;
        segmentOffset = Mathf.Clamp(segmentOffset, 0f, segmentLength);
    }
    int NextIndex(int i)
    {
        if (!looped && i == vertices.Count - 1) return -1;
        return (i + 1) % vertices.Count;
    }
    int LastIndex(int i)
    {
        if (!looped && i == 0) return -1;
        return (i + vertices.Count - 1) % vertices.Count;
    }
    float LengthOfSegment(int i)
    {
        return Vector2.Distance(vertices[i], vertices[NextIndex(i)]);
    }
}
