using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter5Controller : MonoBehaviour
{
    public Transform todoList;
    public GameObject wateringPrefab;
    private GameObject lastWateringPrefab = null;
    //public bool listDone=false;
    private int done=0;
    public ListTrigger initList;
    public List<string> LetterNames = new List<string>();
    int currentDay;
    public LetterDisplayer letterDisplayer;
    public PuzzleLock toDoListLock;

    [Header("Camera")]
    public CameraPoint outsideCameraPoint;

    // Start is called before the first frame update
    void Awake()
    {
        currentDay = -1;
    }
    void Start()
    {
        InitTodoList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //0-Todo-list
    public void InitTodoList(){
        done=0;
        toDoListLock.Lock();
        //listDone=false;
        currentDay += 1;
        if (initList != null)
        {
            initList.TriggerEvents();
        }
        if (lastWateringPrefab != null)
        {
            Destroy(lastWateringPrefab);
        }
        lastWateringPrefab = Instantiate(wateringPrefab);
        lastWateringPrefab.GetComponent<Watering>().gameController = this;
        foreach(Transform child in todoList){
            child.gameObject.SetActive(false);
        }

    }
    public void SetTodoList(int i){
        int number=1;
        foreach(Transform child in todoList){
            if(number==i){
                child.gameObject.SetActive(true);
                done+=1;
            }
            number++;
        }
        if(done==3)
        {
            toDoListLock.TriggerUnlock();
        }
            //listDone=true;
    }

    public void PlayLetter()
    {
        if (letterDisplayer != null)
        {
            int index = currentDay >= LetterNames.Count ? LetterNames.Count - 1 : currentDay;
            letterDisplayer.PlayLetterOfName(LetterNames[index]);
        }
    }
    public void LoadLevel(string levelName)
    {
        // 使用SceneManager来加载关卡
        SceneManager.LoadScene(levelName);
    }
}
