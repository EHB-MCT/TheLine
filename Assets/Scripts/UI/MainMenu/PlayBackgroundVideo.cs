using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayBackgroundVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // Referentie naar de Video Player
    public RawImage rawImage;            // Referentie naar de Raw Image voor de video

    void Start()
    {
        // Zorg ervoor dat de video player correct is ingesteld
        if (videoPlayer != null && rawImage != null)
        {
            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0); // Stel de render texture in
            rawImage.texture = videoPlayer.targetTexture; // Koppel de render texture aan de Raw Image

            videoPlayer.Play(); // Speel de video af
        }
        else
        {
            Debug.LogError("VideoPlayer of RawImage is niet ingesteld.");
        }
    }
}

