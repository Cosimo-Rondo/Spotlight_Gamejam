using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public PuzzleItem currentPuzzleItem;
    public List<PuzzleLock> currentItemTargetAreas = new List<PuzzleLock>();
    public static CursorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CursorManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CursorManager");
                    _instance = go.AddComponent<CursorManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private Sprite defaultCursorSprite;
    [SerializeField] private Sprite uiCursorSprite;
    [SerializeField] private Sprite activeZoneCursorSprite;
    [SerializeField] private Sprite rotationZoneCursorSprite;
    public bool isMovingLight = false;
    public Light currentMovingLight = null;
    [SerializeField] private Vector3 cursorOffset = Vector3.zero;

    private GameObject cursorObject;
    private SpriteRenderer cursorSpriteRenderer;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        InitializeCursorSprite();
        SetDefaultCursor();
    }

    private void InitializeCursorSprite()
    {
        cursorObject = new GameObject("CursorSprite");
        cursorSpriteRenderer = cursorObject.AddComponent<SpriteRenderer>();
        cursorSpriteRenderer.sortingOrder = 9999; // Ensure it's rendered on top
        Cursor.visible = false;
        cursorObject.transform.localScale = Vector3.one * 0.7f;
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorObject.transform.position = (Vector3)mousePosition + cursorOffset;
    }

    public bool isCursorOverUI { get; private set; } = false;
    private int cursorOverUICount = 0;
    public bool isCursorOverActiveZoneOfCanvas = false;
    public bool isCursorOverRotationZoneOfCanvas = false;

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void SetCursorOverActiveZoneOfCanvas(bool value)
    {
        isCursorOverActiveZoneOfCanvas = value;
        UpdateCursorAppearance();
    }

    public void SetCursorOverRotationZoneOfCanvas(bool value)
    {
        isCursorOverRotationZoneOfCanvas = value;
        UpdateCursorAppearance();
    }

    public void SetCurrentPuzzleItem(PuzzleItem puzzleItem)
    {
        currentPuzzleItem = puzzleItem;
    }

    public void AddCurrentItemTargetArea(PuzzleLock puzzleLock)
    {
        if (currentItemTargetAreas.Contains(puzzleLock)) return;
        currentItemTargetAreas.Add(puzzleLock);
    }

    public void RemoveCurrentItemTargetArea(PuzzleLock puzzleLock)
    {
        currentItemTargetAreas.Remove(puzzleLock);
    }

    public void EnterUI()
    {
        cursorOverUICount++;
        if (cursorOverUICount > 0)
        {
            isCursorOverUI = true;
            UpdateCursorAppearance();
        }
    }

    public void LeaveUI()
    {
        cursorOverUICount--;
        if (cursorOverUICount <= 0)
        {
            isCursorOverUI = false;
            UpdateCursorAppearance();
        }
    }

    private void UpdateCursorAppearance()
    {
        if (isMovingLight) return;
        if (isCursorOverRotationZoneOfCanvas)
        {
            cursorSpriteRenderer.sprite = rotationZoneCursorSprite;
        }
        else
        {
            SetDefaultCursor();
        }
    }

    private void SetDefaultCursor()
    {
        cursorSpriteRenderer.sprite = defaultCursorSprite;
    }
}
