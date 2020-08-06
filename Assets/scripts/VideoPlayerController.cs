using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController :MonoBehaviour
{
    public static VideoPlayerController _instance;

    public VideoPlayer videoPlayer;

   

    private void Awake()
    {
            _instance = this;   
    }


    
}
