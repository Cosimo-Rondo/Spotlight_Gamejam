using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CountdownTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public float countdownTime = 0.2f;
    public UnityEvent onCountdownEnd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(countdownTime);
        onCountdownEnd.Invoke();
    }
}
