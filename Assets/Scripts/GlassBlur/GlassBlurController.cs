using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBlurController : MonoBehaviour
{
    public GameObject grass;
    public Texture2D openwindow;
    public Texture2D outside;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = grass.transform.GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenWindow()
    {
        mat.SetTexture("_ClearTex", openwindow);
    }
    public void GoOutside()
    {
        mat.SetTexture("_ClearTex", outside);
        mat.SetVector("_SampleOffset", new Vector4(0, 25, 80f, 0.5f));
    }
}
