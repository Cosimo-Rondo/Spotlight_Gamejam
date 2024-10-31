using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItem : MonoBehaviour
{

    [SerializeField] private bool useItemCode = false;
    [SerializeField] private string itemCode = "default";

    private SpriteRenderer spriteRenderer;
    private Vector3 initialLocalPosition;
    private Vector3 originalScale;
    private float originalAlpha;
    private Selectable selectable;
    private Vector3 originalLocalPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        if (spriteRenderer != null)
        {
            originalAlpha = spriteRenderer.color.a;
        }
        selectable = GetComponent<Selectable>();
        if (selectable == null)
        {
            selectable = gameObject.AddComponent<Selectable>();
            selectable.highlightType = Selectable.HighlightType.Scale;
            selectable.isDraggable = true;
            selectable.SetTransparent(false);
        }
        originalLocalPos = transform.localPosition;
    }
    bool isDragging = false;
    void Update()
    {
        bool isHighlighted = selectable.IsHighlighted();
        if (isHighlighted && Input.GetMouseButton(0))
        {
            if (!isDragging && BubbleCursor.Instance.currentPuzzleItem == null)
            {
                isDragging = true;
                BubbleCursor.Instance.SetCurrentPuzzleItem(GetComponent<PuzzleItem>());
            }
        }
        else if (isDragging && !Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                OnRelease();

            }
        }
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }
    void OnRelease()
    {
        List<PuzzleLock> targetAreas = BubbleCursor.Instance.currentItemTargetAreas;
        bool accepted = false;
        foreach (PuzzleLock targetArea in targetAreas)
        {
            if (!targetArea.useItemCode)
            {
                if (targetArea.requiredItem == this) accepted = true;
            }
            else
            {
                if (targetArea.requiredItemCode == itemCode) accepted = true;
            }
            if (accepted)
            {
                targetArea.TriggerUnlock();
                Accepted();
                break;
            }
        }
        transform.localPosition = originalLocalPos;
        isDragging = false;
        BubbleCursor.Instance.SetCurrentPuzzleItem(null);
    }
    public Vector3 GetInitialLocalPosition()
    {
        return initialLocalPosition;
    }
    public string GetItemCode()
    {
        if (useItemCode)
        {
            return itemCode;
        }
        return null;
    }

    public void Accepted()
    {
        Destroy(gameObject);
    }

    public void RecoverAppearance()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = originalAlpha;
            spriteRenderer.color = color;

            transform.localScale = originalScale;
        }
        else
        {
            Debug.LogWarning("Cannot recover appearance: SpriteRenderer is null.");
        }
    }
}
