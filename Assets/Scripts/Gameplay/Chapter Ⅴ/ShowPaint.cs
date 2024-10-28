using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPaint : MonoBehaviour
{
    public bool canShow=false;
    public GameObject line;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(){
        if(canShow)
            gameObject.SetActive(true);
    }
    public void CanShow(){
        if(line.activeSelf)
            canShow=true;
        else
            canShow=false;
    }
}
