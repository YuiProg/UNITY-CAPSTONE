using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;        

public class TextShuffler : MonoBehaviour
{
    public Text uiText; 

    public float shuffleInterval = 2f; 
    private List<string> stringsToShuffle = new List<string>
    {
        "You can skip the side quest but it will be a difficult journey if you dare.",
        "After finishing a chapter you unlock a new chapter in your journal so check it out!",
        "Parry is the best key to win in any fight you know why it give bonus damage so utilize it.",
        "Always check the surroundings because it's possible that there's an amber box.",
        "TAPUSIN NINYO YUNG SOUNDTRACK MAANGAS YAN LEGIT PROMISE."
    };

    private void Start()
    {
        if (uiText == null)
        {
            uiText = GetComponent<Text>(); 

        }

        if (uiText != null)
        {
            StartCoroutine(ShuffleStrings());
        }
    }

    private IEnumerator ShuffleStrings()
    {

        ShuffleList(stringsToShuffle);

        int index = 0;

        while (true)
        {

            uiText.text = stringsToShuffle[index];

            index = (index + 1) % stringsToShuffle.Count;

            yield return new WaitForSeconds(shuffleInterval);

            if (index == 0)
            {
                ShuffleList(stringsToShuffle);
            }
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
