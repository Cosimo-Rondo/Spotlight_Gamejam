using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DrawingCompleteAudio : MonoBehaviour
{
    public UnityEvent playAudio;
    public CompletePaint completePaint;
    public bool isPlayed=false;

    void Start(){
        completePaint=GetComponent<CompletePaint>();
        isPlayed=false;
    }
    public void PlayFinishAudio(){
        if(completePaint.isCompleted&&!isPlayed){
            playAudio.Invoke();
            isPlayed=true;
        }
    }
}
