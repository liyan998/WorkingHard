using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextFilter : MonoBehaviour
{

    [SerializeField]
    TextAsset
        forbiddenWordsFile;
    List<string> forbiddenWords = new List<string>();

    void Start()
    {
        forbiddenWords.AddRange(forbiddenWordsFile.text.Split('\n'));
    }

    public bool IsLegal(string inputText)
    {
        foreach (string word in forbiddenWords)
        {
            if (inputText.Contains(word))
            {
                Debug.Log("This content contained the word #" + word + "# is illegal!");
                return false;
            }
        }
        return true;
    }
}
