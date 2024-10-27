using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLetterData", menuName = "ScriptableObjects/LetterData", order = 1)]
public class LetterData : ScriptableObject
{
    [SerializeField]
    private List<string> sentences = new List<string>();

    public List<string> Sentences
    {
        get { return sentences; }
    }
}
