using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PaintingTimeline : MonoBehaviour
{
    public Color inActiveColor = Color.gray;
    public List<Frame> frames = new List<Frame>();
    public List<Image> icons = new List<Image>();
    public List<VisualElementAnimator> backgrounds = new List<VisualElementAnimator>();
    public List<bool> isFrameCompleted = new List<bool>();
    private int frameCount;
    private int currentIndex = -1;
    private Vector3 originalIconScale;
    public float switchInterval = 10f; // 切换间隔，单位为秒

    void Awake()
    {
        Init();
    }

    void Init()
    {        
        frameCount = frames.Count;
        for (int i = 0; i < frameCount; i++)
        {
            isFrameCompleted.Add(false);
            icons[i].color = inActiveColor;
        }
        if (icons.Count > 0)
        {
            originalIconScale = icons[0].transform.localScale;
        }
    }

    void Start()
    {
        backgrounds[0].transform.position += new Vector3(0, 0, 0.01f);
        backgrounds[0].Appear(0.1f);
        SetCurrentFrame(0);
        StartCoroutine(SwitchFrameCoroutine());
    }

    void Update()
    {
        
    }

    public void SetCurrentFrame(int index)
    {
        if (currentIndex == index) return;
        if (currentIndex >= 0 && currentIndex < icons.Count)
        {
            icons[currentIndex].transform.DOScale(originalIconScale, 0.3f);
            if (frames[currentIndex] != null) frames[currentIndex].GetComponent<VisualElementAnimator>().Disappear();
        }

        currentIndex = index;

        if (currentIndex >= 0 && currentIndex < icons.Count)
        {
            icons[currentIndex].transform.DOScale(originalIconScale * 1.2f, 0.3f);
            if (frames[currentIndex] != null) frames[currentIndex].GetComponent<VisualElementAnimator>().Appear();
        }
    }

    public int GetNextInactiveFrameIndex(int currentIndex)
    {
        int nextIndex = (currentIndex + 1) % frameCount;
        while (nextIndex != currentIndex)
        {
            if (!isFrameCompleted[nextIndex]) break;
            nextIndex = (nextIndex + 1) % frameCount;
        }
        return nextIndex;
    }

    public int GetNextFrameIndex(int currentIndex)
    {
        return (currentIndex + 1) % frameCount;
    }

    private IEnumerator SwitchFrameCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            int nextIndex = GetNextFrameIndex(currentIndex);
            if (nextIndex == currentIndex) {
                yield return null;
                continue;
            }
            backgrounds[currentIndex].transform.position -= new Vector3(0, 0, 0.01f);
            backgrounds[currentIndex].Disappear(switchInterval);
            backgrounds[nextIndex].transform.position += new Vector3(0, 0, 0.01f);
            backgrounds[nextIndex].Appear(switchInterval);
            yield return new WaitForSeconds(switchInterval * 0.8f);
            SetCurrentFrame(nextIndex);
            yield return new WaitForSeconds(switchInterval * 0.2f);
        }
    }
}
