using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonProxy : MonoBehaviour
{
    public void PlayBGM(string name)
    {
        AudioManager.PlayBGM(name);
    }
    public void PlaySFXNoWait(string name)
    {
        AudioManager.PlaySFXNoWait(name);
    }
    public void PlaySFXWait(string name)
    {
        AudioManager.PlaySFXWait(name);
    }
    public void SwitchScene(string name)
    {
        SceneSwitcher.Instance.SwitchScene(name);
    }
}
