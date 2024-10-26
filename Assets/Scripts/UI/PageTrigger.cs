using UnityEngine;

public class PageTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Open,
        Close
    }

    public TriggerType triggerType = TriggerType.Open;

    public bool isStacked = false;

    public Page targetPage;

    public void Trigger()
    {
        switch (triggerType)
        {
            case TriggerType.Open:
                PageManager.Open(targetPage, isStacked);
                break;
            case TriggerType.Close:
                PageManager.Close();
                break;
        }
    }
}
