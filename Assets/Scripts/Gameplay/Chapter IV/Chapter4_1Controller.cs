using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4_1Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> bgs = new List<Sprite>();
    public List<Sprite> objs = new List<Sprite>();
    public SpriteRenderer bg;
    public SpriteRenderer obj;
    int index;
    public Chapter4Clock clock;
    void Start()
    {
        index = 0;
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextScene()
    {
        index++;
        UpdateSprite();
        if (index == 1)
        {
            clock.SetSeason(Chapter4Clock.Season.Winter);
            clock.SetHourTime(5.5f);
        }
        else if (index == 2)
        {
            clock.SetSeason(Chapter4Clock.Season.Autumn);
            clock.SetHourTime(5.5f);           
        }
        else if (index == 3)
        {
            clock.SetSeason(Chapter4Clock.Season.Winter);
            clock.SetHourTime(12);                      
        }
    }
    void UpdateSprite()
    {
        if (index > 3) return;
        bg.sprite = bgs[index];
        obj.sprite = objs[index];
        if (index != 0)
        {
            obj.transform.localPosition = new Vector3(0, 0, obj.transform.localPosition.z);
        }
        
    }
}
