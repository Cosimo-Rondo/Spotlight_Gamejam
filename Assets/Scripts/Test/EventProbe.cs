using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventProbe : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent events;

    public void Trigger()
    {
        events.Invoke();
    }
}
