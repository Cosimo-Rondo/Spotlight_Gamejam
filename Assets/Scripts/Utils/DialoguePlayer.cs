using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public List<VisualElementAnimator> dialogues = new List<VisualElementAnimator>();
    public bool hideLastDialogue = true;
    public bool hideWhenDone = true;
    private int currentDialogueIndex = -1;
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

            currentDialogueIndex = i;
            dialogues[i].Appear();

            yield return new WaitForSeconds(0.1f); // Prevent key bounce

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        // Hide the last dialogue if needed
        if (hideWhenDone && currentDialogueIndex >= 0)
        {
            dialogues[currentDialogueIndex].Disappear();
        }
    }
}
