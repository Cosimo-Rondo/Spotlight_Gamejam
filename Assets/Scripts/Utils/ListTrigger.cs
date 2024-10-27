using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventTimePair
{
    public UnityEvent eventToTrigger;
    public float delayInSeconds;
}

public class ListTrigger : MonoBehaviour
{
    [SerializeField]
    private List<EventTimePair> eventTimePairs = new List<EventTimePair>();

    public void TriggerEvents()
    {
        StartCoroutine(TriggerEventsCoroutine());
    }

    private IEnumerator TriggerEventsCoroutine()
    {
        foreach (EventTimePair pair in eventTimePairs)
        {
            pair.eventToTrigger.Invoke();
            if (pair.delayInSeconds >= 0)
            {
                yield return new WaitForSeconds(pair.delayInSeconds);
            }
            else
            {
                yield return new WaitUntil(() => Input.anyKeyDown);
            }
        }
    }
}
