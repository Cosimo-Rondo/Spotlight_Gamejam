using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPaint : MonoBehaviour
{
    public bool canShow=false;
    private bool is_show = false;
    public GameObject line;
    private float duration = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(){
        if (canShow && !is_show)
        {
            gameObject.SetActive(true);
            is_show = true;
            StartCoroutine(SmoothShowPieces());
        }
    }
    public void CanShow(){
        if(line.activeSelf)
            canShow=true;
        else
            canShow=false;
    }

    IEnumerator SmoothShowPieces()
    {
        Material piece_mat = transform.GetComponent<SpriteRenderer>().material;
        piece_mat.color = new Color(1, 1, 1, 0);
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp(timer / duration, 0, 1);
            piece_mat.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t);
            yield return null;
        }
    }
}
