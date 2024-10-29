using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5_1Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextCharacter()
    {
        SceneSwitcher.Instance.SwitchScene("Chapter V-2-1");
    }
}
