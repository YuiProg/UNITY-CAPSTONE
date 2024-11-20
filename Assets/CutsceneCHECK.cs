using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneCHECK : MonoBehaviour
{

    public string scenename;
    public VideoPlayer videoPlayer;
    public Text skip;
    private bool showSkipText = false;
    private bool pressedSpaceToSkip = false;

    void Start()
    {
        skip.text = "";
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!showSkipText)
            {
                showSkipText = true;
                skip.text = "Press Space to Skip";
            }
            else if (!pressedSpaceToSkip)
            {
                pressedSpaceToSkip = true;
                LevelManager.instance.loadscene("Cave_1");
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!pressedSpaceToSkip)
        {
            LevelManager.instance.loadscene(scenename);
        }
    }
}


