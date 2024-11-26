using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker instance;
    public bool hasQuest = false;
    [SerializeField] GameObject Questbar;
    [SerializeField] Text questText;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        

    }

    private void Start()
    {
        Questbar.SetActive(hasQuest);

        if (PlayerPrefs.HasKey("Quest") == true)
        {
            hasQuest = true;
            questText.text = PlayerPrefs.GetString("Quest");
        }
        else
        {
            hasQuest = false;
        }
    }

    private void Update()
    {
            Questbar.SetActive(PlayerPrefs.HasKey("Quest"));
            questText.text = PlayerPrefs.GetString("Quest");
    }
}
