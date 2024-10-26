using UnityEngine;
using System.Collections.Generic;

public class Page : VisualElementAnimator
{
    [SerializeField] private bool isRoot = false;

    public bool IsActive => isActive;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        if (isRoot)
        {
            PageManager.Open(this, true);
        }
    }

    public override void Appear()
    {
        base.Appear();
    }

    public override void Disappear()
    {
        base.Disappear();
    }
    public Transform GetContentTransform()
    {
        return contentsRoot;
    }
}
