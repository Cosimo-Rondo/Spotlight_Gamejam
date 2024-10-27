using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onSelectEvent;
    public UnityEvent onUnselectEvent;
    public UnityEvent onHighlightEvent;
    public UnityEvent onUnhighlightEvent;
    public UnityEvent onHoldEvent;
    public SpriteRenderer spriteRenderer;
    public UnityEngine.UI.Image image;
    [SerializeField] private TMP_Text textMeshPro;
    public Transform highlightTarget;
    public Collider2D mouseDetectArea;
    //public bool isActiveZoneForCanvas = false;

    public enum SelectType
    {
        MouseClick,
        MouseHold
    }
    public bool unhighlightOnSelect = true;

    public SelectType selectType;

    public enum HighlightType
    {
        None,
        Color,
        Sprite,
        Border,
        Scale
    }

    public HighlightType highlightType;
    public Color highlightColor = Color.white;
    private Color originalColor;
    public Sprite highlightSprite;
    public float borderWidth = 1f;
    public Vector3 highlightScale = new Vector3(1.1f, 1.1f, 1.1f);
    private Sprite originalSprite;
    private Vector3 originalPos;
    private Material originalMaterial;
    private Material outlineMaterial;
    protected bool isHighlighted = false;
    private Color originalTextColor;
    private Vector3 originalScale;
    private Vector2 originalSizeDelta;
    private RectTransform rectTransform;
    protected virtual void Awake()
    {
        if (highlightTarget == null)
        {
            highlightTarget = transform;
        }
        if (mouseDetectArea == null)
        {
            mouseDetectArea = GetComponent<Collider2D>();
        }
        originalPos = transform.position;
    }

    protected virtual void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = highlightTarget.GetComponentInChildren<SpriteRenderer>();
        }
        if (image == null)
        {
            image = highlightTarget.GetComponentInChildren<UnityEngine.UI.Image>();
        }
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
            originalMaterial = spriteRenderer.material;
            originalColor = spriteRenderer.color;
        }
        else if (image != null)
        {
            originalSprite = image.sprite;
            originalMaterial = image.material;
            originalColor = image.color;
        }

        textMeshPro = highlightTarget.GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null)
        {
            originalTextColor = textMeshPro.color;
        }

        if (highlightType == HighlightType.Border)
        {
            outlineMaterial = new Material(Shader.Find("Custom/SpriteOutline"));
            outlineMaterial.SetColor("_OutlineColor", highlightColor);
            outlineMaterial.SetFloat("_OutlineWidth", 0);
        }

        rectTransform = highlightTarget.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalSizeDelta = rectTransform.sizeDelta;
        }
        else
        {
            originalScale = highlightTarget.localScale;
        }

    }
    protected virtual void OnEnable()
    {
        Unhighlight();
    }
    bool isDragging = false;
    protected virtual void Update()
    {
        MouseHitCheck();
        if (selectType == SelectType.MouseClick)
        {
            if (isHighlighted && Input.GetMouseButtonDown(0))
            {
                OnSelect();
            }
            else if (!isHighlighted && Input.GetMouseButtonDown(0))
            {
                OnUnselect();
            }
        }
        else if (selectType == SelectType.MouseHold)
        {
            if (isHighlighted && Input.GetMouseButton(0))
            {
                onHoldEvent.Invoke();
            }
        }
    }
    void MouseHitCheck()
    {
        if (mouseDetectArea != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseDetectArea.OverlapPoint(mousePosition))
            {
                CustomizedOnMouseEnter();
            }
            else
            {
                CustomizedOnMouseExit();
            }
        }
    }

    protected virtual void OnSelect()
    {
        //if (!IsPageActive()) return;
        //Debug.Log("OnSelect");
        onSelectEvent.Invoke();
        if (unhighlightOnSelect)
        {
            Unhighlight();
        }
    }

    protected virtual void OnUnselect()
    {
        //if (!IsPageActive()) return;
        onUnselectEvent.Invoke();
    }

    public virtual void Highlight()
    {
        //Debug.Log("Highlight");
        if (isHighlighted) return;
        //if (!IsPageActive()) return;
        /*
        if (isActiveZoneForCanvas)
        {
            CursorManager.Instance.isCursorOverActiveZoneOfCanvas = true;
        }
        */
        isHighlighted = true;
        if (spriteRenderer != null || image != null)
        {
            switch (highlightType)
            {
                case HighlightType.Color:
                    if (spriteRenderer != null)
                        spriteRenderer.color = highlightColor;
                    else
                        image.color = highlightColor;
                    break;
                case HighlightType.Sprite:
                    if (highlightSprite != null)
                    {
                        if (spriteRenderer != null)
                            spriteRenderer.sprite = highlightSprite;
                        else
                            image.sprite = highlightSprite;
                    }
                    break;
                case HighlightType.Border:
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.material = outlineMaterial;
                        outlineMaterial.SetFloat("_OutlineWidth", borderWidth);
                        outlineMaterial.SetColor("_OutlineColor", highlightColor);
                    }
                    else
                    {
                        // Note: Border highlight for Image is not implemented here
                        Debug.LogWarning("Border highlight for Image is not implemented");
                    }
                    break;
            }
        }
        //Debug.Log("Highlight type: " + highlightType);
        if (highlightType == HighlightType.Scale)
        {
            //Debug.Log("Highlight scale");
            if (rectTransform != null)
            {
                Debug.Log("Highlight rect scale");
                rectTransform.sizeDelta = new Vector2(
                    originalSizeDelta.x * highlightScale.x,
                    originalSizeDelta.y * highlightScale.y
                );
            }
            else
            {
                highlightTarget.localScale = Vector3.Scale(originalScale, highlightScale);
            }
        }
        if (highlightType == HighlightType.Color && textMeshPro != null)
        {
            originalTextColor = textMeshPro.color;
            textMeshPro.color = highlightColor;
        }
        //highlightTarget.position = highlightTarget.position + new Vector3(0, 0, -0.1f);
        onHighlightEvent.Invoke();
    }

    public virtual void Unhighlight()
    {
        if (!isHighlighted) return;
        /*
        if (isActiveZoneForCanvas)
        {
            CursorManager.Instance.isCursorOverActiveZoneOfCanvas = false;
        }
        */
        isHighlighted = false;
        if (spriteRenderer != null || image != null)
        {
            switch (highlightType)
            {
                case HighlightType.Color:
                    if (spriteRenderer != null)
                        spriteRenderer.color = originalColor;
                    else
                        image.color = originalColor;
                    break;
                case HighlightType.Sprite:
                    if (spriteRenderer != null)
                        spriteRenderer.sprite = originalSprite;
                    else
                        image.sprite = originalSprite;
                    break;
                case HighlightType.Border:
                    if (spriteRenderer != null)
                        spriteRenderer.material = originalMaterial;
                    else
                        image.material = originalMaterial;
                    break;
            }
        }
        if (highlightType == HighlightType.Scale)
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = originalSizeDelta;
            }
            else
            {
                highlightTarget.localScale = originalScale;
            }
        }
        if (highlightType == HighlightType.Color && textMeshPro != null)
        {
            textMeshPro.color = originalTextColor;
        }
        //highlightTarget.position = highlightTarget.position + new Vector3(0, 0, 0.1f);
        onUnhighlightEvent.Invoke();
    }

    protected bool IsPageActive()
    {
        Page belongingPage = GetComponentInParent<Page>();
        return belongingPage == null || belongingPage.IsActive;
    }

    private void CustomizedOnMouseEnter()
    {
        if (selectType == SelectType.MouseClick || selectType == SelectType.MouseHold)
        {
            Highlight();
        }
    }

    private void CustomizedOnMouseExit()
    {
        if (selectType == SelectType.MouseClick || selectType == SelectType.MouseHold)
        {
            Unhighlight();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectType == SelectType.MouseClick || selectType == SelectType.MouseHold)
        {
            Highlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectType == SelectType.MouseClick || selectType == SelectType.MouseHold)
        {
            Unhighlight();
        }
    }
    public bool IsHighlighted()
    {
        return isHighlighted;
    }
}
