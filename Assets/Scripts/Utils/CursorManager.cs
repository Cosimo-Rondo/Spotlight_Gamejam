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
    }

    public bool isCursorOverUI { get; private set; } = false;
    private int cursorOverUICount = 0;
    public bool isCursorOverActiveZoneOfCanvas = false;

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
        }
    }
    public void LeaveUI()
    {
        cursorOverUICount--;
        if (cursorOverUICount <= 0)
        {
            isCursorOverUI = false;
        }
    }
}
