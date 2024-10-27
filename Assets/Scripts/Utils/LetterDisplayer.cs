using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterDisplayer : MonoBehaviour
{
    public LetterData letterData;
    public TextMeshProUGUI textDisplay;
    public float fadeInDuration = 1f;
    public float delayBetweenSentences = 1f;

    private List<string> sentences;
    private int currentSentenceIndex = 0;

    void Start()
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

    IEnumerator DisplaySentences()
    {
        foreach (string sentence in sentences)
        {
            yield return StartCoroutine(FadeInSentence(sentence));
            yield return new WaitForSeconds(delayBetweenSentences);
        }
    }

    IEnumerator FadeInSentence(string sentence)
    {
        int startIndex = textDisplay.text.Length;
        textDisplay.text += sentence + "\n";
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
