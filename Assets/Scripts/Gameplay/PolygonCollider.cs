using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PolygonCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public Polygon shape;
    public UnityEvent onFullyContainedByAnotherAreaEvent;
    public UnityEvent onExitAnotherAreaEvent;
    private bool isFullyContainedByAnotherArea = false;
    public bool allowOverlappedByIrrelevantAreas = false;
    Vector3 lastFramePosition;
    void Awake()
    {
        CreateItemPolygon();
    }
    void Update()
    {
        if (lastFramePosition != transform.position)
        {
            lastFramePosition = transform.position;
            CreateItemPolygon();
        }
    }
    private void CreateItemPolygon()
    {
        if (this == null) return;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            shape = new Polygon();
            if (collider is PolygonCollider2D polygonCollider)
            {
                Vector2[] points = polygonCollider.points;
                for (int i = 0; i < points.Length; i++)
                {
                    shape.vertices.Add(transform.TransformPoint(points[i]));
                }
            }
            else if (collider is BoxCollider2D boxCollider)
            {
                Vector2 size = boxCollider.size;
                Vector2 offset = boxCollider.offset;
                Vector2 halfSize = size / 2f;

                shape.vertices.Add(transform.TransformPoint(new Vector2(-halfSize.x, -halfSize.y) + offset));
                shape.vertices.Add(transform.TransformPoint(new Vector2(halfSize.x, -halfSize.y) + offset));
                shape.vertices.Add(transform.TransformPoint(new Vector2(halfSize.x, halfSize.y) + offset));
                shape.vertices.Add(transform.TransformPoint(new Vector2(-halfSize.x, halfSize.y) + offset));
            }
            else
            {
                Debug.LogWarning("Unsupported collider type for creating item polygon.");
            }
        }
        else
        {
            Debug.LogWarning("No collider found for creating item polygon.");
        }
    }
    private void OnFullyContainedByAnotherArea()
    {
        onFullyContainedByAnotherAreaEvent.Invoke();
    }
    private void OnExitAnotherArea()
    {
        onExitAnotherAreaEvent.Invoke();
    }
    public void SetIsFullyContainedByAnotherArea(bool value)
    {
        if (isFullyContainedByAnotherArea && !value)
        {
            OnExitAnotherArea();
        }
        else if (!isFullyContainedByAnotherArea && value)
        {
            OnFullyContainedByAnotherArea();
        }
        isFullyContainedByAnotherArea = value;
    }
}
