using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePaint : MonoBehaviour
{
    public int numberOfPieces;
    public List<GameObject> pieces;
    public bool isCompleted;
    public GameObject homeIcon;
    
    // Start is called before the first frame update
    void Start()
    {
        isCompleted=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCompleted)
            homeIcon.GetComponent<VisualElementAnimator>().Appear();
    }
    public void CompleteCheck(){
        for(int i=0;i<pieces.Capacity;i++){
            if(pieces[i].activeSelf==false)
                return;
        }
        isCompleted=true;
    }
}
