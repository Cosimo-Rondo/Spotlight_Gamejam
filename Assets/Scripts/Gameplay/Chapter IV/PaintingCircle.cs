using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingCircle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int currentFrameIndex = 0;
    public List<bool> isComplete = new List<bool>();
    public List<VisualElementAnimator> frames = new List<VisualElementAnimator>();
    
    
    void Awake()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            isComplete.Add(false);
        }
        currentFrameIndex = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetComplete(int index)
    {
        isComplete[index] = true;
        
    }
}
