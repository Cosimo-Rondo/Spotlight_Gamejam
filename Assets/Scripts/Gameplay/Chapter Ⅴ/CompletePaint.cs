using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletePaint : MonoBehaviour
{
    [Header("Jigsaw Handler")]
    public List<GameObject> pieces;
    public bool isCompleted;
    public GameObject homeIcon;

    [Header("OnClickDarken")]
    public bool isEnd;
    public Image blackImage;
    public Image endText; 
    public int clickTime=8;
    public float duration = 5;
    private Color targetColor = new Color(0f, 0f, 0f, 0f); // 初始透明黑色
    private float darkenRate = 0.125f; // 每次点击变暗的程度
    public ListTrigger listTrigger;
    private Coroutine endCouroutine;

    // Start is called before the first frame update
    void Start()
    {
        isCompleted=false;
        
        if(blackImage!=null)
            blackImage.color = targetColor;

        darkenRate=1.0f/clickTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCompleted&&!isEnd) //完成拼图且不是最后一关
            homeIcon.GetComponent<VisualElementAnimator>().Appear();
        if(isCompleted&&isEnd){ //完成拼图且是最后一关
            blackImage.color=new Color(0f,0f,0f,1f);
            endCouroutine=StartCoroutine(SetEndText());
        }
    }

    //每次点击拼图块时调用
    public void CompleteCheck(){
        for(int i=0;i<pieces.Capacity;i++){
            if(pieces[i].activeSelf==false)
                return;
        }
        isCompleted=true;
    }

    public void DarkenScreen()
    {
        Debug.Log(targetColor);
        Debug.Log("前"+blackImage.color);
        Debug.Log(isEnd);
        if(isEnd==true){
            // 每次点击时，颜色变得更暗
            targetColor.a+=darkenRate;
            if (targetColor.a > 1)
            {
                targetColor.a = 1; // 确保不会超过纯黑色
            }

            // 更新Image的颜色
            blackImage.color = Color.Lerp(blackImage.color,targetColor,0.3f);
        }
        Debug.Log("后"+blackImage.color);
    }

    IEnumerator SetEndText(){
        // 渐变持续的时间
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            endText.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1.0f,1.0f,1.0f,1.0f), t);
            

            if(endText.color.a>0.95f){
                listTrigger.TriggerEvents();
                StopCoroutine(endCouroutine);
            }

            yield return null;
        }
    }
 
    
}
