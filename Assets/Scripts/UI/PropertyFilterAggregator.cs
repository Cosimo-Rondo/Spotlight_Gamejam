using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyFilterAggregator: MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Graphic uiElement;
    public Light lightSprite;
    public PolygonMaskGenerator hiddenArea;
    public float alphaFilter = 1f;
    public float originalAlpha;
    public bool saved = false;
    private List<PropertyFilter> filters = new List<PropertyFilter>();
    void Awake()
    {
        GetComponents();
    }
    void GetComponents()
    {
        if (this == null) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiElement = GetComponent<Graphic>();
        lightSprite = GetComponent<Light>();
        hiddenArea = GetComponent<PolygonMaskGenerator>();
    }
    void Update()
    {
        UpdateAlpha();
    }
    public void AddFilter(PropertyFilter filter)
    {
        filters.Add(filter);
    }
    public void UpdateAlpha()
    {
        if (this == null) return;
        GetComponents();
        alphaFilter = 1f;
        
        foreach (PropertyFilter filter in filters)
        {
            alphaFilter *= filter.alphaFilter;
        }
        //Debug.Log(gameObject.name + " alphaFilter: " + alphaFilter);
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alphaFilter * originalAlpha;
            spriteRenderer.color = color;
        }
        if (uiElement != null)
        {
            Color color = uiElement.color;
            color.a = alphaFilter * originalAlpha;
            uiElement.color = color;
        }
        if (lightSprite != null)
        {
            lightSprite.color = new Color(lightSprite.color.r, lightSprite.color.g, lightSprite.color.b, alphaFilter * originalAlpha);
        }
        if (hiddenArea != null)
        {
            hiddenArea.alpha = alphaFilter * originalAlpha;
        }
    }
    public void Save()
    {
        if (saved) return;
        SaveOriginalAlpha();
        saved = true;
    }
    private void SaveOriginalAlpha()
    {
        GetComponents();
        if (spriteRenderer != null)
        {
            originalAlpha = spriteRenderer.color.a;
        }
        if (uiElement != null)
        {
            originalAlpha = uiElement.color.a;
        }
        if (lightSprite != null)
        {
            originalAlpha = lightSprite.color.a;
        }
        if (hiddenArea != null)
        {
            originalAlpha = hiddenArea.alpha;
        }
    }
}
