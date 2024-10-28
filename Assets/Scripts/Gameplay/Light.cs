using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour
{
    [Range(0f, 180f)]
    public float angle = 45f; // ���߼нǣ���������������slider��������Χ0~90
    [SerializeField] private float distance = 100000f; // ���߾���
    [Range(-90f, 90f)]
    public float rotation = 0f; // ������ת���򣬿�����slider��������Χ-90~90
    public float minRotation = -90f;
    public float maxRotation = 90f;
    float originalRotation = 0f;
    public float minAngle = 0f;
    public float maxAngle = 175f;
    
    private float targetRotation;
    private float targetAngle;
    public float rotationSmoothTime = 0.1f;
    public float angleSmoothTime = 0.1f;
    private float rotationVelocity;
    private float angleVelocity;
    
    private GameObject meshObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    public Polygon shape;
    public Color color = new Color(1f, 1f, 1f, 0.2f);
    private Painting border; // ���������ڴ洢Border��shape
    [SerializeField] private bool isLightOn = true;
    public RailSlider railSlider;
    public enum OperationType{
        Rotate,
        Move
    }
    public OperationType operationType = OperationType.Rotate;
    Frame frame;
    Transform brushTransform;

    public VisualElementAnimator visualElementAnimator;
    public VisualElementAnimator lightIcon;

    void Awake()
    {
        // ����һ����Ϊ"mesh"��������
        meshObject = new GameObject("mesh");
        meshObject.transform.SetParent(transform, false);

        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        
        // ����URP Unlit���ʲ�����Ϊ��͸����ɫ
        Material material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        material.color = color;
        
        // ���ò���Ϊ͸��ģʽ
        material.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
        material.SetFloat("_Blend", 0); // 0 = Alpha, 1 = Premultiply, 2 = Additive, 3 = Multiply
        material.SetFloat("_AlphaClip", 0);
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
        
        meshRenderer.material = material;

        shape = new Polygon();

        distance = 100000f;
        minDistanceToActiveRotate = 0.2f;

        railSlider = GetComponentInParent<RailSlider>();
        
        targetRotation = rotation;
        targetAngle = angle;

        if (brushTransform == null)
        {
            brushTransform = transform.Find("Brush");
        }
        if (visualElementAnimator == null)
        {
            visualElementAnimator = GetComponentInParent<VisualElementAnimator>();
        }
        originalRotation = transform.rotation.eulerAngles.z;
    }
    void Start()
    {
        frame = GetComponentInParent<Frame>();
        border = frame.border;
    }

    void Update()
    {
        UpdateSpotlight();
        CheckPlayerInteraction();
        
        // ƽ������rotation��angle
        rotation = Mathf.SmoothDamp(rotation, targetRotation, ref rotationVelocity, rotationSmoothTime);
        angle = Mathf.SmoothDamp(angle, targetAngle, ref angleVelocity, angleSmoothTime);
    }
    private float lastClickTime = 0;
    private bool isMouseHolding = false;
    private bool isHovering = false;
    private Vector2 lastMousePosition;
    private float minDistanceToActiveRotate = 0.1f;
    private bool isMousePosValidLastFrame = false;
    public bool useLegacyInteraction = false;
    void CheckPlayerInteraction()
    {
        if (useLegacyInteraction)
        {
            Interaction_Legacy();
        }
        else
        {
            Interaction_Legacy();//Interaction_Modern();
        }
    }
    public void Interaction_Modern()
    {
        if (isHovering)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("time elapsed: " + (Time.time - lastClickTime));
                if (Time.time - lastClickTime < 0.4f)
                {
                    isLightOn = !isLightOn;
                    lastClickTime = -1;
                }
                isMouseHolding = true;
                lastClickTime = Time.time;
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonDown(1))
            {
                PutBack();
            }
        }
        if (isHovering || isMouseHolding)
        {
            CheckAngleOperation();
            CheckRotationOperation();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseHolding = false;
            isMousePosValidLastFrame = false;
        }
        if (isMouseHolding)
        {
            if (!isMousePosValidLastFrame) isMousePosValidLastFrame = true;
            else CheckMoveOperation();
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    public void CheckRotationOperation()
    {
        float inputDelta = Input.GetAxis("Horizontal") 
        - (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? 1 : 0)
        - (Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? 1 : 0);
        targetRotation += Input.mouseScrollDelta.y * 750f * Time.deltaTime;
        targetRotation += inputDelta * 100f * Time.deltaTime;
        targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation); 
    }
    public void CheckAngleOperation()
    {
        float verticalInput = Input.GetAxis("Vertical") + (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        if (verticalInput != 0)
        {
            targetAngle += verticalInput * 25f * Time.deltaTime;
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
        }
    }
    public void CheckMoveOperation()
    {
        if (railSlider == null) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        railSlider.MoveAlongRail(mousePosition - lastMousePosition);
    }
    public void Interaction_Legacy()
    {
        if (isHovering)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("time elapsed: " + (Time.time - lastClickTime));
                if (Time.time - lastClickTime < 0.4f)
                {
                    isLightOn = !isLightOn;
                    lastClickTime = -1;
                }
                operationType = OperationType.Move;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (CursorManager.Instance.isCursorOverRotationZoneOfCanvas)
                {
                    operationType = OperationType.Rotate;
                }
                else operationType = OperationType.Move;
            }
            if (Input.GetMouseButton(0))
            {
                isMouseHolding = true;
                CursorManager.Instance.isMovingLight = true;
                lastClickTime = Time.time;
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                targetAngle += Input.mouseScrollDelta.y * 1.5f;
                targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
            }
        } 
        if (Input.GetMouseButtonUp(0))
        {
            isMouseHolding = false;
            isMousePosValidLastFrame = false;
            CursorManager.Instance.isMovingLight = false;
        }
        if (isMouseHolding)
        {
            if (operationType == OperationType.Rotate)
            {
                CheckRotateOperation_Legacy();
            }
            else if (operationType == OperationType.Move)
            {
                if (!isMousePosValidLastFrame)
                {
                    isMousePosValidLastFrame = true;
                }
                else CheckMoveOperation();
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
    public bool IsLightOn()
    {
        if (visualElementAnimator != null)
        {
            if (!visualElementAnimator.isActive)
            {
                return false;
            }
        }
        return isLightOn;
    }
    public void PutBack(bool forever = false)
    {
        visualElementAnimator.Disappear();
        if (!forever)
        {
            if (lightIcon != null)
            {
                lightIcon.Appear();
            }
        }
    }
    public void CheckRotateOperation_Legacy()
    {
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(currentMousePosition, transform.position) < minDistanceToActiveRotate) {
            isMousePosValidLastFrame = false;
            return;
        }
        if (!isMousePosValidLastFrame)
        {
            isMousePosValidLastFrame = true;
        }
        else
        {
            Vector2 lightPosition = transform.position;

            Vector2 lastDirection = lastMousePosition - lightPosition;
            Vector2 currentDirection = currentMousePosition - lightPosition;
            float dist = currentDirection.magnitude;

            float angleChange = Vector2.SignedAngle(lastDirection, currentDirection);
            float angleChangeNormalized = angleChange * dist / 10f;
            targetRotation += angleChangeNormalized;
            targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);            
        }
        lastMousePosition = currentMousePosition;
    }
    public void OnHighlight()
    {
        //Debug.Log("On Light Highlight");
        isHovering = true;
    }
    public void OnUnhighlight()
    {
        //Debug.Log("On Light Unhighlight");
        isHovering = false;
    }
    void UpdateSpotlight()
    {
        Mesh mesh = meshFilter.mesh;
        if (!isLightOn) {
            //mesh.Clear();
            //mesh.RecalculateNormals();
            //return;
        }
        float currentAngle = isLightOn ? angle : 0;
        if (brushTransform != null)
        {
            
        }
        Debug.Log(gameObject.name + " rotation: " + originalRotation + " + " + rotation);
        transform.rotation = Quaternion.Euler(0, 0, originalRotation + rotation);
        // �����������ߵķ��򣬿���rotation
        Vector2 direction = Quaternion.Euler(0, 0, 0) * Vector2.right;
        Vector2 leftRay = Quaternion.Euler(0, 0, currentAngle / 2) * direction;
        Vector2 rightRay = Quaternion.Euler(0, 0, -currentAngle / 2) * direction;
        // ���ö���
        shape.vertices.Clear();
        /*
        shape.vertices.Add(new Vector2(-1,1));
        shape.vertices.Add(new Vector2(-1,0));
        shape.vertices.Add(new Vector2(0,0));
        shape.vertices.Add(new Vector2(0,1));
        */
        shape.vertices.Add(transform.TransformPoint(Vector2.zero));
        shape.vertices.Add(transform.TransformPoint(leftRay * distance));
        shape.vertices.Add(transform.TransformPoint(rightRay * distance));
        Polygon lightShape = new Polygon(shape.vertices);

        shape.vertices = Polygon.CalculateIntersection(
            new List<Polygon>{border.borderShape, lightShape}
        );

        // ����������
        List<Vector2> localVertices = new List<Vector2>();
        foreach(Vector2 vertex in shape.vertices)
            localVertices.Add(transform.InverseTransformPoint(vertex));
        int[] triangles = Polygon.Triangulate(localVertices);
        
        mesh.Clear();
        mesh.vertices = Polygon.ConvertToVector3Array(localVertices);
        mesh.triangles = triangles;

        // ���¼��㷨��
        mesh.RecalculateNormals();

        //meshFilter.mesh = mesh;
        meshRenderer.material.color = color;
    }
}
