using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonMaskGenerator : MonoBehaviour
{
    public Painting painting;
    private GameObject maskArea;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Polygon shape;
    public List<Light> lights = new List<Light>();
    public List<PolygonCollider> interestedColliders = new List<PolygonCollider>();
    private Frame frame;
    public float alpha = 1;
    void Awake()
    {
        frame = GetComponentInParent<Frame>();
    }
    void Start()
    {
        GenerateMask();
    }
    void GenerateMask()
    {
        maskArea = new GameObject("MaskArea");
        maskArea.transform.SetParent(transform);
        maskArea.transform.localScale = Vector3.one;
        maskArea.transform.localPosition = Vector3.zero;
        maskArea.transform.position += new Vector3(0, 0, -0.01f * lights.Count);

        meshFilter = maskArea.AddComponent<MeshFilter>();
        meshRenderer = maskArea.AddComponent<MeshRenderer>();
        UpdateMesh(meshFilter.mesh);

        Material maskMaterial = new Material(Shader.Find("CustomRenderTexture/Mask"));
        maskMaterial.SetTexture("_MainTex", painting.texture);
        maskMaterial.SetVector("_CanvasSize", new Vector4(painting.canvasSize.x, painting.canvasSize.y, 0, 0));
        maskMaterial.SetVector("_CanvasPivot", new Vector4(painting.canvasCenter.x, painting.canvasCenter.y, 0, 0));
        meshRenderer.material = maskMaterial;
    }
    void Update()
    {
        UpdateMaskShape();
        UpdateShaderSettings();
    }
    void UpdateMaskShape()
    {
        UpdateMesh(meshFilter.mesh);
    }
    void UpdateShaderSettings()
    {
        Material maskMaterial = meshRenderer.material;
        //maskMaterial.SetTexture("_MainTex", painting.texture);
        maskMaterial.SetVector("_CanvasSize", new Vector4(painting.canvasSize.x, painting.canvasSize.y, 0, 0));
        maskMaterial.SetVector("_CanvasPivot", new Vector4(painting.canvasCenter.x / Screen.width, painting.canvasCenter.y / Screen.height, 0, 0));
        maskMaterial.SetFloat("_Alpha", alpha);
    }
    void UpdateMesh(Mesh mesh)
    {
        mesh.Clear();
        List<Polygon> polygons = new List<Polygon>();
        foreach (Light light in lights)
        {
            if (!light.IsLightOn() || light.shape == null) return;
            polygons.Add(light.shape);
        }
        polygons.Add(painting.borderShape);
        List<Vector2> intersectionVertices = Polygon.CalculateIntersection(polygons);
        shape = new Polygon(intersectionVertices);

        List<Vector2> intersectionVerticesLocal = new List<Vector2>();
        foreach (Vector2 vertex in intersectionVertices)
        {
            Vector3 localPos = transform.InverseTransformPoint(vertex);
            intersectionVerticesLocal.Add(new Vector2(localPos.x, localPos.y));
        
            //intersectionVerticesLocal.Add(new Vector2(localPos.x * transform.lossyScale.x, localPos.y * transform.lossyScale.y));
        }
        Vector3[] vertices = Polygon.ConvertToVector3Array(intersectionVerticesLocal);
        int[] triangles = Polygon.Triangulate(intersectionVerticesLocal);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        DiscoverShapesInside();
    }
    void DiscoverShapesInside()
    {
        foreach (PolygonCollider collider in interestedColliders)
        {
            Debug.Log("Discover shape");
            if (FullyContainShape(collider.shape))
            {
                Debug.Log("Fully contain shape");
                bool intersectWithIrrelevantLights = false;
                if (!collider.allowOverlappedByIrrelevantAreas)
                {
                    foreach (Light light in frame.lights)
                    if (!lights.Contains(light) && light.IsLightOn())
                    {
                        if (light.shape == null) continue;
                        if (collider.shape.IntersectWithPolygon(light.shape))
                        {
                            intersectWithIrrelevantLights = true;
                            break;
                        }
                    }
                }
                collider.SetIsFullyContainedByAnotherArea(!intersectWithIrrelevantLights);
            }
            else
            {
                collider.SetIsFullyContainedByAnotherArea(false);
            }
        }
    }

    public bool FullyContainShape(Polygon polygon)
    {
        return shape.FullyContainPolygon(polygon);
    }

}
