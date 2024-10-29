using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter4Clock : MonoBehaviour
{
    private const float MINUTES_PER_DAY = 1440f;
    private const float DEGREES_PER_CIRCLE = 360f;
    public Transform minuteHand;
    public Transform hourHand;
    public float minuteTime;
    public float hour;
    public float minute;
    public TimeOfDay timeOfDay;
    public float baseSpeed;
    public Selectable selectable;
    public List<Sprite> windowView; // 存储不同季节和时间的风景Sprite
    public SpriteRenderer windowBlackMask;
    public List<SpriteRenderer> windowRenderers; // 用于显示风景的SpriteRenderer

    [System.Serializable]
    public class InspirationCondition{
        public Season season;
        public float startTime, endTime;
    }
    public List<InspirationCondition> inspirationConditions = new List<InspirationCondition>();
    public InspirationCondition currentInspirationCondition;

    public enum Season {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum TimeOfDay {
        Morning,
        Noon,
        Evening
    }

    public Season currentSeason = Season.Spring; // 当前季节
    public Selectable door;

    // 计算分针角度
    // 分针每分钟转6度 (360/60)
    public float GetMinuteHandAngle(float minutes)
    {
        float normalizedMinutes = minutes % 60f;
        return 90 - normalizedMinutes * 6f;
    }

    // 计算时针角度
    // 时针每小时转30度 (360/12)，每分钟转0.5度 (30/60)
    public float GetHourHandAngle(float minutes) 
    {
        float totalHours = (minutes / 60f) % 12f;
        return 90 - totalHours * 30f;
    }
    public TimeOfDay GetTimeOfDay(float minutes)
    {
        float hours = (minutes / 60f) % 24f;
        if (hours < 9f)
        {
            return TimeOfDay.Morning;
        }
        else if (hours >= 9f && hours < 14f)
        {
            return TimeOfDay.Noon;
        }
        else if (hours >= 14f)
        {
            return TimeOfDay.Evening;
        }
        return TimeOfDay.Evening;
    }

    // 给定分钟数，返回时针和分针的角度
    void Awake()
    {
        baseSpeed = 1f;
        if (selectable == null)
        {
            selectable = GetComponent<Selectable>();
        }
        SetCurrentInspirationCondition(0);
    }
    void SetCurrentInspirationCondition(int index)
    {
        currentInspirationCondition = inspirationConditions[index];
    }
    public void Update()
    {
        //minuteTime = minute + hour * 60f;
        minute = minuteTime % 60f;
        hour = minuteTime / 60f % 24f;
        if (minuteHand != null) minuteHand.transform.rotation = Quaternion.Euler(0, 0, GetMinuteHandAngle(minuteTime));
        if (hourHand != null) hourHand.transform.rotation = Quaternion.Euler(0, 0, GetHourHandAngle(minuteTime));
        timeOfDay = GetTimeOfDay(minuteTime);
        minuteTime += Time.deltaTime * Speed();
        UpdateWindowView();
        UpdateDoorStatus();
    }
    void UpdateDoorStatus()
    {
        if (door != null)
        {
            if (currentInspirationCondition.season == currentSeason && 
                hour >= currentInspirationCondition.startTime && hour <= currentInspirationCondition.endTime)
                door.enabled = true;
            else door.enabled = false;
        }
    }
    float Speed()
    {
        return baseSpeed * SpeedMultiplier();
    }
    float SpeedMultiplier() 
    {
        if (selectable == null) return 1f;
        return selectable.IsHighlighted() ? 100f : 1f;
    }
    public void SetHourTime(float hour)
    {
        minuteTime = hour * 60;
    }
    public void SetSeason(Season season)
    {
        currentSeason = season;
    }
    void UpdateWindowView()
    {
        
        for(int i = 0; i < windowRenderers.Count; i++)
        {
            if (windowRenderers[i] != null)
            {
                windowRenderers[i].sprite = windowView[ViewIndex(currentSeason, (TimeOfDay)i)];
                windowRenderers[i].color = new Color(1f, 1f, 1f, GetAlpha(minuteTime, (TimeOfDay)i));
            }
        }
        windowBlackMask.color = new Color(0f, 0f, 0f, GetBlackMaskAlpha(minuteTime));
    }
    int ViewIndex(Season season, TimeOfDay time)
    {
        return (int)season * 3 + (int)time;

    }
    float GetBlackMaskAlpha(float currentMinuteTime)
    {
        float currentHour = currentMinuteTime / 60f % 24f;
        if (currentHour <= 3f || currentHour >= 21f) return 1f;
        if (currentHour >= 7f && currentHour <= 16f) return 0f;
        if (currentHour >3f && currentHour < 7f) return (7f - currentHour) / 4f;
        if (currentHour > 17f && currentHour < 22f) return (currentHour - 17f) / 5f;
        return 0f;
    }
    float GetAlpha(float currentMinuteTime, TimeOfDay timeOfDay)
    {
        float currentHour = currentMinuteTime / 60f % 24f;
        if (timeOfDay == TimeOfDay.Morning)
        {
            if (currentHour < 10f)
            {
                return 1f;
            }
            else if (currentHour >= 11f)
            {
                return 0f;
            }
            else return 11f - currentHour;
        }
        else if (timeOfDay == TimeOfDay.Noon)
        {
            if (currentHour < 10f || currentHour >= 17.5f)
            {
                return 0f;
            }
            else if (currentHour >= 10f && currentHour <= 11f)
            {
                return currentHour - 10f;
            }
            else if (currentHour >= 17f && currentHour <= 17.5f)
            {
                return (17.5f - currentHour) / 0.5f;
            }
            else if (currentHour >= 11f && currentHour <= 17f)
            {
                return 1f;
            }
        }   
        else if (timeOfDay == TimeOfDay.Evening)
        {
            if (currentHour >= 17.5f)
            {
                return 1f;
            }
            else if (currentHour >= 17f && currentHour <= 17.5f)
            {
                return (currentHour - 17f) / 0.5f;
            }
            else return 0f;
        }
        return 0f;
    }
    public void NextScene()
    {
        SceneSwitcher.Instance.SwitchScene("Chapter V - I");
    }
}
