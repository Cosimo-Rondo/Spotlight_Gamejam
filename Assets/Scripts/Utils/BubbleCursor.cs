using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BubbleCursor : MonoBehaviour
{
    private static BubbleCursor _instance;
    public static BubbleCursor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BubbleCursor>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("BubbleCursor");
                    _instance = go.AddComponent<BubbleCursor>();
                }
            }
            return _instance;
        }
    }

    [System.Serializable]
    class CursorInfo{
        public Sprite sprite;
        public Vector2 offset;
        public Vector2 size;
    }
    [SerializeField] private CursorInfo defaultCursor;
    [SerializeField] private CursorInfo exitDoorCursor;
    [SerializeField] private CursorInfo rotateCursor;
    [SerializeField] private CursorInfo dragCursor;
    [SerializeField] private CursorInfo clickCursor;
    private GameObject cursorObject;
    private Image cursorImage;
    private Canvas cursorCanvas;
    CursorInfo currentCursorInfo;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Initialize()
    {
        InitializeCursorSprite();
        SetCursor(defaultCursor);
    }

    private void InitializeCursorSprite()
    {
        // 创建 Canvas
        GameObject canvasObj = new GameObject("CursorCanvas");
        cursorCanvas = canvasObj.AddComponent<Canvas>();
        cursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        cursorCanvas.sortingOrder = 2;
        
        // 添加 CanvasScaler
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // 创建光标
        cursorObject = new GameObject("CursorSprite");
        cursorObject.transform.SetParent(canvasObj.transform, false);
        cursorImage = cursorObject.AddComponent<Image>();
        cursorImage.raycastTarget = false;
        
        Cursor.visible = false;
        DontDestroyOnLoad(canvasObj);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 确保在场景加载后光标对象仍然存在
        if (cursorObject == null || cursorImage == null)
        {
            InitializeCursorSprite();
            SetCursor(defaultCursor);
        }
    }

    private void SetCursor(CursorInfo cursorInfo)
    {
        cursorImage.sprite = cursorInfo.sprite;
        currentCursorInfo = cursorInfo;
        UpdateCursorSize();
    }

    private void UpdateCursorSize()
    {
        if (cursorObject == null) return;
        RectTransform rectTransform = cursorObject.GetComponent<RectTransform>();
        if (rectTransform == null) return;
        rectTransform.sizeDelta = currentCursorInfo.size;
    }

    private void UpdateCursorAppearance()
    {
        if (cursorObject == null) return;
        RectTransform rectTransform = cursorObject.GetComponent<RectTransform>();
        if (rectTransform == null) return;
        CursorInfo cursorInfo;
        if (isCursorReadyToDrag) cursorInfo = dragCursor;
        else if (isCursorReadyToRotate) cursorInfo = rotateCursor;
        else cursorInfo = defaultCursor;
        SetCursor(cursorInfo);
    }

    public Selectable currentHighlightArea = null;
    [SerializeField] private bool isCursorOverUI = false;
    static public bool IsOverUI => Instance.isCursorOverUI;
    [SerializeField] private bool isCursorInActiveArea = false;
    static public bool IsInActiveArea => Instance.isCursorInActiveArea;

    [SerializeField] private bool isCursorInDragArea = false;
    static public bool IsInDragArea => Instance.isCursorInDragArea;

    [SerializeField] private bool isCursorInRotateArea = false;
    static public bool IsInRotateArea => Instance.isCursorInRotateArea;
    [SerializeField] private bool isCursorReadyToDrag = false;
    static public bool IsReadyToDrag => Instance.isCursorReadyToDrag;
    [SerializeField] private bool isCursorReadyToRotate = false;
    static public bool IsReadyToRotate => Instance.isCursorReadyToRotate;
    private List<GameObject> itemToDrag = new List<GameObject>();
    static public void AddItemToDrag(GameObject item)
    {
        if (Instance.itemToDrag.Contains(item)) return;
        Instance.itemToDrag.Add(item);
    }
    static public void RemoveItemToDrag(GameObject item)
    {
        Instance.itemToDrag.Remove(item);
    }
    private List<GameObject> itemToRotate = new List<GameObject>();
    static public void AddItemToRotate(GameObject item)
    {
        if (Instance.itemToRotate.Contains(item)) return;
        Instance.itemToRotate.Add(item);
    }
    static public void RemoveItemToRotate(GameObject item)
    {
        Instance.itemToRotate.Remove(item);
    }

    List<Selectable> selectables = new List<Selectable>();

    private void Update()
    {
        UpdateCursorState();
        UpdateCursorAppearance();
    }

    // 新增：更新光标大小的方法
    static public void AddSelectable(Selectable selectable)
    {
        if (Instance.selectables.Contains(selectable)) return;
        Instance.selectables.Add(selectable);
    }
    static public void RemoveSelectable(Selectable selectable)
    {
        if (!Instance.selectables.Contains(selectable)) return;
        Instance.selectables.Remove(selectable);
    }
    void UpdateCursorState()
    {
        isCursorOverUI = false;
        isCursorInActiveArea = false;
        isCursorInRotateArea = false;
        isCursorInDragArea = false;
        isCursorReadyToDrag = false;
        isCursorReadyToRotate = false;
        List<Selectable> cleanedSelectables = new List<Selectable>();
        foreach(var selectable in selectables)
        {
            if(selectable != null)
            {
                cleanedSelectables.Add(selectable);
            }
        }
        selectables = cleanedSelectables;
        foreach(var selectable in selectables)
        {
            if (!selectable.enabled) continue;
            if(selectable.isUIElement && selectable.IsHighlighted()) isCursorOverUI = true;
            if(selectable.isActiveArea && selectable.IsHighlighted()) isCursorInActiveArea = true;
            if(selectable.isRotateArea && selectable.IsHighlighted()) isCursorInRotateArea = true;
            if(selectable.isDragArea && selectable.IsHighlighted()) isCursorInDragArea = true;
            if (selectable.isDraggable && selectable.IsHighlighted()) isCursorReadyToDrag = true;
        }
        List<GameObject> cleanedItemToDrag = new List<GameObject>();
        foreach(var item in itemToDrag)
        {
            if (item != null) cleanedItemToDrag.Add(item);
        }
        List<GameObject> cleanedItemToRotate = new List<GameObject>();
        foreach(var item in itemToRotate)
        {
            if (item != null) cleanedItemToRotate.Add(item);
        }
        itemToDrag = cleanedItemToDrag;
        itemToRotate = cleanedItemToRotate;
        if (itemToDrag.Count > 0) isCursorReadyToDrag = true;
        if (itemToRotate.Count > 0) isCursorReadyToRotate = true;
        cursorObject.transform.position = Input.mousePosition + (Vector3)currentCursorInfo.offset;
    }

    public PuzzleItem currentPuzzleItem;
    public List<PuzzleLock> currentItemTargetAreas = new List<PuzzleLock>();

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
}
