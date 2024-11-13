using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RESOLUTION : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;

    private void Start()
    {
        // Set the videoPlayer RenderTexture to match screen resolution
        AdjustVideoResolution();
    }

    private void AdjustVideoResolution()
    {
        // Get the current screen resolution
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        // Adjust the RawImage to match screen resolution
        rawImage.rectTransform.sizeDelta = new Vector2(screenWidth, screenHeight);

        // Optional: Adjust the Render Texture resolution if necessary
        RenderTexture renderTexture = videoPlayer.targetTexture;
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(screenWidth, screenHeight, 16);
            videoPlayer.targetTexture = renderTexture;
        }
        else
        {
            renderTexture.Release(); // Release if already created
            renderTexture.width = screenWidth;
            renderTexture.height = screenHeight;
            renderTexture.Create();
        }
    }
}
