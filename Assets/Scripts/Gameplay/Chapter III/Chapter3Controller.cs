using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public CameraPoint lastCamPointOutside = null;
    public CameraPoint CamPoint4, CamPoint5, CamPoint6;
    public VisualElementAnimator Frame2_1, Frame2_2;
    void Awake()
    {
        lastCamPointOutside = CamPoint4;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RecordLastCamPointOutside()
    {
        lastCamPointOutside = Camera.main.GetComponent<CameraController>().currentCameraPoint;
    }
    public void SwitchToLastCamPointOutside()
    {
        lastCamPointOutside.SwitchToThis(0);
    }
    public void ShowFrameOutside()
    {
        if (lastCamPointOutside == CamPoint4){
            Frame2_1.Appear();
        }
        else if (lastCamPointOutside == CamPoint5)
        {
            Frame2_2.Appear();
        }
    }
    public void NextChapter()
    {
        SceneSwitcher.Instance.SwitchScene("Chapter IV - II - opening");
    }
    public void Umbrella()
    {
        
    }
}
