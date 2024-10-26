using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleLock : MonoBehaviour
{
    public enum LockType
    {
        Item,
        AndGate,
        OrGate,
        Dumb
    }
    public bool reusable = false;
    public LockType lockType;

    // For Item lock
    public PuzzleItem requiredItem;
    public bool useItemCode = false;
    public string requiredItemCode;

    // For AndGate and OrGate locks
    public List<PuzzleLock> preconditionLocks;

    // Event to notify when the lock status changes
    public UnityEvent<bool> onStatusChanged;
    public UnityEvent onUnlocked;

    [SerializeField]
    private bool isUnlocked = false;
    private Selectable detectArea;

    void Awake()
    {
        detectArea = GetComponent<Selectable>();
    }
    void Start()
    {
        isUnlocked = false;
        if (IsGate())
        {
            SubscribeToPreconditionLocks();
        }
    }
    bool isHighlightedLastFrame = false;
    public void Update()
    {
        if (lockType == LockType.Item)
        {
            if (detectArea != null)
            {
                if (detectArea.IsHighlighted())
                {
                    CursorManager.Instance.SetCurrentItemTargetArea(this);
                }
                else if (!detectArea.IsHighlighted() && isHighlightedLastFrame)
                {
                    CursorManager.Instance.SetCurrentItemTargetArea(null);
                }
                isHighlightedLastFrame = detectArea.IsHighlighted();
            }
        }
    }

    // Method to unlock the puzzle lock
    private bool Unlock()
    {
        if (!reusable && isUnlocked) return false;
        isUnlocked = true;
        onStatusChanged.Invoke(true);
        onUnlocked.Invoke();
        return true;
    }
    public void TriggerUnlock()
    {
        Unlock();
    }
    public void Lock()
    {
        if (!reusable) return;
        isUnlocked = false;
        onStatusChanged.Invoke(false);
    }

    bool IsGate()
    {
        return lockType == LockType.AndGate || lockType == LockType.OrGate;
    }

    void SubscribeToPreconditionLocks()
    {
        foreach (var preconditionLock in preconditionLocks)
        {
            preconditionLock.onStatusChanged.AddListener(CheckAndUpdateStatus);
        }
    }

    void CheckAndUpdateStatus(bool status)
    {
        if (lockType == LockType.AndGate)
        {
            if (preconditionLocks.TrueForAll(l => l.isUnlocked))
            {
                Unlock();
            }
            else
            {
                if (reusable) Lock();
            }
        }
        else if (lockType == LockType.OrGate)
        {
            if (preconditionLocks.Exists(l => l.isUnlocked))
            {
                Unlock();
            }
            else
            {
                if (reusable) Lock();
            }
        }
    }
}
