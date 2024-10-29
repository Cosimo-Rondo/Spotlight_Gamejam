using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PaintingTimeline : MonoBehaviour
{
    public Color inActiveColor = Color.gray;
    public Color highlightColor = Color.yellow; // 添加高亮颜色
    public List<Color> iconColors = new List<Color>();
    public List<Frame> frames = new List<Frame>();
    public List<Image> icons = new List<Image>();
    public List<VisualElementAnimator> backgrounds = new List<VisualElementAnimator>();
    public List<bool> isFrameCompleted = new List<bool>();
    private int frameCount;
    private int currentIndex = -1;
    private Vector3 originalIconScale;
    public float switchInterval = 10f;
    public float iconColorFadeDuration = 0.2f; // 添加颜色渐变持续时间

    public Light lastFrameLightA, lastFrameLightB, lastFrameLightC;
    public ListTrigger lastFrameCompleteTrigger;
    public VisualElementAnimator switchSceneMask;

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
        if (!lastFrameLightA.IsLightOn() && !lastFrameLightB.IsLightOn() && !lastFrameLightC.IsLightOn())
        {
            lastFrameCompleteTrigger.TriggerEvents();
        }
    }

    public void SetCurrentFrame(int index)
    {
        if (currentIndex == index) return;
        if (currentIndex >= 0 && currentIndex < icons.Count)
        {
            icons[currentIndex].transform.DOScale(originalIconScale, 0.3f);
            if (frames[currentIndex] != null) frames[currentIndex].GetComponent<VisualElementAnimator>().Disappear();
            // 如果前一个frame未完成，恢复为非激活颜色
            if (!isFrameCompleted[currentIndex])
            {
                icons[currentIndex].DOColor(inActiveColor, iconColorFadeDuration);
            }
        }

        currentIndex = index;

        if (currentIndex >= 0 && currentIndex < icons.Count)
        {
            icons[currentIndex].transform.DOScale(originalIconScale * 1.4f, 0.3f);
            if (frames[currentIndex] != null) frames[currentIndex].GetComponent<VisualElementAnimator>().Appear();
            // 如果当前frame未完成，设置为高亮颜色
            if (!isFrameCompleted[currentIndex])
            {
                icons[currentIndex].DOColor(highlightColor, iconColorFadeDuration);
            }
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
    int activeFrameCount = 0;
    public void SetFrameActive(int index)
    {
        isFrameCompleted[index] = true;
        icons[index].DOColor(iconColors[index], iconColorFadeDuration);
        activeFrameCount++;
        if (activeFrameCount == 6) NextScene();
    }
    public void NextScene()
    {
        switchSceneMask.Appear(2);
        SceneSwitcher.Instance.SwitchScene("Chapter V - I");
    }
}
