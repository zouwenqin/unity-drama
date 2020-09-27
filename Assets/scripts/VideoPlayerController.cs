using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController :MonoBehaviour
{
    public static VideoPlayerController _instance;

    public VideoPlayer videoPlayer;

    public List<GameObject> videoItemList = new List<GameObject>();
    public List<string> videoItemPathList = new List<string>();
    public Dictionary<string, Sprite> videoItemImage = new Dictionary<string, Sprite>();
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;   
    }   
}
