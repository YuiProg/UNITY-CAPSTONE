using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    public void changeScene(string scenename)
    {
        Time.timeScale = 1;
        LevelManager.instance.loadscene(scenename);
    }

    public void saveStats()
    {
        Save.instance.saveStats();
    }

}
