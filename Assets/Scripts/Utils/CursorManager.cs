/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Light currentActiveLight = null;
    [SerializeField] private Vector3 cursorOffset = Vector3.zero;
    [SerializeField] private Vector2 cursorSize = new Vector2(32, 32); // ����������Inspector�е����Ĺ���С

    private GameObject cursorObject;
    private Image cursorImage;
    private Canvas cursorCanvas;

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
        // ���� Canvas
        GameObject canvasObj = new GameObject("CursorCanvas");
        cursorCanvas = canvasObj.AddComponent<Canvas>();
        cursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        cursorCanvas.sortingOrder = 9999;
        
        // ��� CanvasScaler
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        // �������
        cursorObject = new GameObject("CursorSprite");
        cursorObject.transform.SetParent(canvasObj.transform, false);
        cursorImage = cursorObject.AddComponent<Image>();
        cursorImage.raycastTarget = false;
        
        Cursor.visible = false;
        UpdateCursorSize(); // ���������¹���С
    }

    private void Update()
    {
        // ʹ����Ļ����
        if (Input.GetMouseButtonUp(0))
        {
            isMovingLight = false;
        }
        cursorObject.transform.position = Input.mousePosition + (Vector3)cursorOffset;
    }

    // ���������¹���С�ķ���
    private void UpdateCursorSize()
    {
        if (cursorObject != null)
        {
            RectTransform rectTransform = cursorObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = cursorSize;
            }
        }
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
            cursorImage.sprite = rotationZoneCursorSprite;
        }
        else
        {
            SetDefaultCursor();
        }
        UpdateCursorSize(); // �������ڸ��¹�����ʱͬʱ���´�С
    }

    private void SetDefaultCursor()
    {
        cursorImage.sprite = defaultCursorSprite;
    }
}
*/