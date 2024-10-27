using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyFilter : MonoBehaviour
{

    public float alphaFilter = 1f;
    public void SetAlphaFilter(float newAlpha)
    {
        alphaFilter = Mathf.Clamp01(newAlpha);
    }

    public void UpdateAlpha()
    {
        if (this == null) return;
        PropertyFilterAggregator aggregator = GetComponent<PropertyFilterAggregator>();
        if (aggregator != null)
        {
            aggregator.UpdateAlpha();
        }
    }
    public void Save()
    {
        if (this == null) return;
        PropertyFilterAggregator aggregator = GetComponent<PropertyFilterAggregator>();
        if (aggregator != null)
        {
            aggregator.Save();
        }
    }
}
