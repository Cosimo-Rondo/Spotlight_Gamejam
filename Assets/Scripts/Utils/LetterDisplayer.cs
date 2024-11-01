using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LetterDisplayer : MonoBehaviour
{
    public LetterData letterData;
    public TextMeshProUGUI textDisplay;
    public float fadeInDuration = 1f;
    public float delayBetweenSentences = 1f;

    private List<string> sentences;
    private int currentSentenceIndex = 0;
    public UnityEvent onLetterDone;

    void Start()
    {
        Init();
    }
    void Init()
    {
        if (letterData != null)
        {
            sentences = letterData.Sentences;
        }
        else
        {
            Debug.LogError("LetterData is not assigned!");
        }

        textDisplay.text = "";
    }
    public void PlayLetter()
    {
        StartCoroutine(DisplaySentences());
    }
    public void PlayLetterOfName(string name)
    {
        letterData = Resources.Load<LetterData>("Letter/" + name);
        Init();
        PlayLetter();
    }

    IEnumerator DisplaySentences()
    {
        Init();
        foreach (string sentence in sentences)
        {
            yield return StartCoroutine(FadeInSentence(sentence));
            if (delayBetweenSentences > 0)
            {
                yield return new WaitForSeconds(delayBetweenSentences);
            }
            else
            {
                yield return new WaitUntil(() => Input.anyKeyDown);
                yield return new WaitForSeconds(0.07f);
            }
        }
        onLetterDone.Invoke();
    }

    IEnumerator FadeInSentence(string sentence)
    {
        int startIndex = textDisplay.text.Length;
        textDisplay.text += sentence.Replace("\\n", "\n") + "\n";
        textDisplay.ForceMeshUpdate();

        TMP_TextInfo textInfo = textDisplay.textInfo;
        Color32[] newVertexColors = textInfo.meshInfo[0].colors32;

        float fadeProgress = 0f;
        while (fadeProgress < fadeInDuration)
        {
            byte alpha = (byte)Mathf.Lerp(0, 255, fadeProgress / fadeInDuration);

            for (int i = startIndex; i < textDisplay.text.Length - 1; i++)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                if (vertexIndex != -1)
                {
                    newVertexColors[vertexIndex + 0].a = alpha;
                    newVertexColors[vertexIndex + 1].a = alpha;
                    newVertexColors[vertexIndex + 2].a = alpha;
                    newVertexColors[vertexIndex + 3].a = alpha;
                }
            }

            textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            fadeProgress += Time.deltaTime;
            yield return null;
        }
    }
}
