using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public List<Light> lights;
    public Painting border;
    public Page page;
    public Collider2D rackCollider;
    void Awake()
    {
        if (border == null)
        {
            border = transform.Find("Border").GetComponent<Painting>();
        }
        page = GetComponent<Page>();
        if (rackCollider == null)
        {
            Transform rack = transform.Find("Rack");
            if (rack != null)
            {
                rackCollider = rack.GetComponent<Collider2D>();
            }
        }
    }
    bool isActiveLastFrame = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !BubbleCursor.IsInActiveArea)
        {
            if (page != null && isActiveLastFrame)
            {
                page.Disappear();
                StartCoroutine(ActiveRackCollider());
            }
        }
        isActiveLastFrame = page.isActive;
    }
    IEnumerator ActiveRackCollider()
    {
        if (rackCollider == null) yield break;
        yield return new WaitForSeconds(0.5f);
        rackCollider.enabled = true;
    }
    public void PutbackLights()
    {
        foreach (Light light in lights)
        {
            light.PutBack(true);
        }
    }
}
