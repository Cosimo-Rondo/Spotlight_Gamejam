using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    private static PageManager _instance;
    public static PageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PageManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PageManager");
                    _instance = go.AddComponent<PageManager>();
                }
            }
            return _instance;
        }
    }

    private Stack<Page> pageStack = new Stack<Page>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void Open(Page newPage, bool isStacked)
    {
        if (Instance.pageStack.Count > 0)
        {
            Page currentPage = Instance.pageStack.Peek();
            if (newPage == currentPage) return;
            if (!isStacked) Instance.pageStack.Pop();
            currentPage.Disappear();
        }

        Instance.pageStack.Push(newPage);
        newPage.Appear();
    }

    public static void Close()
    {
        if (Instance.pageStack.Count > 0)
        {
            Page currentPage = Instance.pageStack.Pop();
            currentPage.Disappear();

            if (Instance.pageStack.Count > 0)
            {
                Instance.pageStack.Peek().Appear();
            }
        }
    }
}
