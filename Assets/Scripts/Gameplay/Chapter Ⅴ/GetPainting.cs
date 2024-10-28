using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPainting : MonoBehaviour
{
    public bool canGet;
    private CompletePaint completePaint;
    public GameObject hidenPainting;
    public GameObject finishedPainting;
    // Start is called before the first frame update
    void OnEnable()
    {
        canGet=completePaint.isCompleted;
        completePaint=GetComponent<CompletePaint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Combine(){
        canGet=completePaint.isCompleted;
        if(canGet){
            hidenPainting.SetActive(false);
            finishedPainting.SetActive(true);
        }
    }

}
