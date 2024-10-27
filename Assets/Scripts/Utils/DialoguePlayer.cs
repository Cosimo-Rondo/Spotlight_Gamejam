using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialoguePlayer : MonoBehaviour
{
    public List<VisualElementAnimator> dialogues = new List<VisualElementAnimator>();
    public bool hideLastDialogue = true;
    public bool hideWhenDone = true;
    private int currentDialogueIndex = -1;
    public float waitTimeBetweenDialogues = 0.5f;
    public UnityEvent onDialogueDone;
    void Start()
    {
    }
    public void Play()
    {
        StartCoroutine(PlayDialogueCoroutine());
    }

    IEnumerator PlayDialogueCoroutine()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (hideLastDialogue && currentDialogueIndex >= 0)
            {
                dialogues[currentDialogueIndex].Disappear();
            }
            if (i > 0) yield return new WaitForSeconds(waitTimeBetweenDialogues);
            currentDialogueIndex = i;
            dialogues[i].Appear();
            if (i == dialogues.Count - 1 && !hideWhenDone) onDialogueDone.Invoke();
            yield return new WaitForSeconds(0.1f); // Prevent key bounce

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        // Hide the last dialogue if needed
        if (hideWhenDone) onDialogueDone.Invoke();
        if (hideWhenDone && currentDialogueIndex >= 0)
        {
            dialogues[currentDialogueIndex].Disappear();
        }
    }
}
