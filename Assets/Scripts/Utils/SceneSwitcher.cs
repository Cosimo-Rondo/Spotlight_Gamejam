using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{   
    private static SceneSwitcher _instance;
    public static SceneSwitcher Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneSwitcher>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SceneSwitcher");
                    _instance = go.AddComponent<SceneSwitcher>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    public void SwitchScene(string sceneName)
    {
        StartCoroutine(SwitchSceneCoroutine(sceneName));
    }
    IEnumerator SwitchSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }
}
