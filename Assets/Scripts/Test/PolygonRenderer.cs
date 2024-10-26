using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonRenderer : MonoBehaviour
{
    public Polygon polygon;
    private LineRenderer lineRenderer;
    public bool looped = false;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.loop = looped;

        // 初始化为正三角形
        lineRenderer.positionCount = 3;
        lineRenderer.SetPositions(new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(-0.866f, -0.5f, 0),
            new Vector3(0.866f, -0.5f, 0)
        });

        UpdateVertices();
    }

    void Update()
    {
        UpdateVertices();
    }

    public void UpdateLineRenderer(Vector3[] newPositions)
    {
        lineRenderer.positionCount = newPositions.Length;
        lineRenderer.SetPositions(newPositions);
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        if (lineRenderer != null)
        {
            polygon.vertices.Clear();
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(positions);
            for (int i = 0; i < positions.Length; i++)
            {
                // 将本地坐标转换为世界坐标
                Vector3 worldPosition = transform.TransformPoint(positions[i]);
                polygon.vertices.Add(new Vector2(worldPosition.x, worldPosition.y));
            }
        }
    }
}
