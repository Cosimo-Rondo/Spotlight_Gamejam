using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public PuzzleItem currentPuzzleItem;
    public PuzzleLock currentItemTargetArea;
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

    public bool isCursorOverUI = false;
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
    public void SetCurrentItemTargetArea(PuzzleLock puzzleLock)
    {
        currentItemTargetArea = puzzleLock;
    }
}
