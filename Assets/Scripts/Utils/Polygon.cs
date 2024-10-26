using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<Vector2> vertices = new List<Vector2>();
    public Polygon()
    {
    }
    public Polygon(List<Vector2> vertices)
    {
        this.vertices = vertices;
    }
    public static List<Vector2> CalculateIntersection(List<Polygon> polygons)
    {
        if (polygons.Count == 0)
            return new List<Vector2>();

        List<Vector2> intersection = new List<Vector2>(polygons[0].vertices);

        for (int i = 1; i < polygons.Count; i++)
        {
            intersection = GetPolygonIntersection(intersection, polygons[i].vertices);
        }

        return intersection;
    }

    public static List<Vector2> GetPolygonIntersection(List<Vector2> polygon1, List<Vector2> polygon2)
    {
        List<Vector2> intersection = new List<Vector2>();
        int n1 = polygon1.Count;
        int n2 = polygon2.Count;

        for (int i = 0; i < n1; i++)
        {
            Vector2 edge1Start = polygon1[i];
            Vector2 edge1End = polygon1[(i + 1) % n1];

            for (int j = 0; j < n2; j++)
            {
                Vector2 edge2Start = polygon2[j];
                Vector2 edge2End = polygon2[(j + 1) % n2];

                Vector2? intersectionPoint = GetLineIntersection(edge1Start, edge1End, edge2Start, edge2End);
                if (intersectionPoint.HasValue)
                {
                    intersection.Add(intersectionPoint.Value);
                }
            }
        }

        foreach (Vector2 point in polygon1)
        {
            if (IsPointInPolygon(point, polygon2))
            {
                intersection.Add(point);
            }
        }

        foreach (Vector2 point in polygon2)
        {
            if (IsPointInPolygon(point, polygon1))
            {
                intersection.Add(point);
            }
        }

        SortPointsClockwise(intersection);

        return intersection;
    }
    public bool IntersectWithPolygon(Polygon polygon)
    {
        return GetPolygonIntersection(this.vertices, polygon.vertices).Count > 0;
    }
    public bool FullyContainPolygon(Polygon polygon)
    {
        foreach (Vector2 point in polygon.vertices)
        {
            if (!IsPointInPolygon(point, this.vertices))
            {
                return false;
            }
        }
        return true;
    }
    public static Vector2? GetLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float det = (a2.x - a1.x) * (b2.y - b1.y) - (b2.x - b1.x) * (a2.y - a1.y);
        if (Mathf.Abs(det) < float.Epsilon) return null;

        float t = ((b1.x - a1.x) * (b2.y - b1.y) - (b2.x - b1.x) * (b1.y - a1.y)) / det;
        float u = ((b1.x - a1.x) * (a2.y - a1.y) - (a2.x - a1.x) * (b1.y - a1.y)) / det;

        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        {
            return new Vector2(a1.x + t * (a2.x - a1.x), a1.y + t * (a2.y - a1.y));
        }

        return null;
    }

    public static bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool inside = false;
        int n = polygon.Count;

        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    public static void SortPointsClockwise(List<Vector2> points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Count;

        points.Sort((a, b) =>
        {
            float angleA = Mathf.Atan2(a.y - center.y, a.x - center.x);
            float angleB = Mathf.Atan2(b.y - center.y, b.x - center.x);
            return angleB.CompareTo(angleA);
        });
    }

    public static Vector3[] ConvertToVector3Array(List<Vector2> vector2List)
    {
        Vector3[] vector3Array = new Vector3[vector2List.Count];
        for (int i = 0; i < vector2List.Count; i++)
        {
            vector3Array[i] = new Vector3(vector2List[i].x, vector2List[i].y, 0);
        }
        return vector3Array;
    }

    public static int[] Triangulate(List<Vector2> vertices)
    {
        List<int> indices = new List<int>();

        // 多边形必须至少有3个点
        if (vertices.Count < 3)
            return indices.ToArray();

        // 使用第一个点作为中心点
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            indices.Add(0); // 中心点
            indices.Add(i);
            indices.Add(i + 1);
        }

        return indices.ToArray();
    }

}
