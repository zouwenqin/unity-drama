using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Panel3_c : MonoBehaviour
{
    public static Panel3_c _Instance;
    public GameObject videoObj;
    
    public Transform VideoItemParent;
    public GameObject videoItem;
    public AVProMovieCaptureBase _movieCapture;
    public Image[] ImagePerson;
    public Text RecordingTime;
    public GameObject videoPlayPanel;

    public GameObject panel4;
    private Material outline;


    private int currentHour, currentMinute, currentSecond;
    private float totalRecordTime;
    bool startRecordingTimer = false;

    [HideInInspector]
    public int videoIndex;

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        Debug.LogError(DateTime.Now.ToString()) ;
        //videoPlayPanel.GetComponent<AVProMovieCaptureFromScene>().
        _movieCapture._downScale = AVProMovieCaptureBase.DownScale.Original;//原画画质
        _movieCapture._frameRate = AVProMovieCaptureBase.FrameRate.Thirty;//帧数
        _movieCapture._outputFolderPath = Application.streamingAssetsPath;//保存视频的路径
        //_movieCapture._codecName = "Media Foundation H.264(MP4)";//设置视频格式
        _movieCapture._useMediaFoundationH264 = true;
        _movieCapture._autoFilenameExtension = "mp4";//格式
        //_movieCapture._codecName = "x264";
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");

    }

    private void Update()
    {
        if (startRecordingTimer)
        {
            //totalRecordTime = _movieCapture._frameTotal / _movieCapture._fps;
            totalRecordTime += Time.deltaTime;
            NowTime();
        }
    }

    public void NowTime()
    {
        currentHour = (int)totalRecordTime / 3600;
        currentMinute = (int)(totalRecordTime - 3600 * currentHour) / 60;
        currentSecond = (int)(totalRecordTime - 60 * currentMinute - 3600 * currentHour);
        RecordingTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
    }

    private void OnEnable()
    {
        if (PanelOne_C.Instance == null)
        {
            return;
        }
        //if (PanelOne_C.Instance.panelOneM.CharacterTex1 != null)
        if (PanelOne_C.Instance.actorImage1 != null)
        {
            //ImagePerson[0].sprite = PanelOne_C.Instance.panelOneM.CharacterTex1;
            ImagePerson[0].sprite = PanelOne_C.Instance.actorImage1;
        }
        //if (PanelOne_C.Instance.panelOneM.CharacterTex2 != null)
        if (PanelOne_C.Instance.actorImage2 != null)
        {
            //ImagePerson[1].sprite = PanelOne_C.Instance.panelOneM.CharacterTex2;
            ImagePerson[1].sprite = PanelOne_C.Instance.actorImage2;
        }
        //if (PanelOne_C.Instance.panelOneM.CharacterTex3 != null)
        if (PanelOne_C.Instance.actorImage3 != null)
        {
            //ImagePerson[2].sprite = PanelOne_C.Instance.panelOneM.CharacterTex3;
            ImagePerson[2].sprite = PanelOne_C.Instance.actorImage3;
        }
        CreateVideoItem();
    }

    public void StartCapture()
    {
        _movieCapture.StartCapture();
        totalRecordTime = 0;
        startRecordingTimer = true;
    }

    public void PauseCapture()
    {
        _movieCapture.PauseCapture();
    }

    public void ContinuePause()
    {
        _movieCapture._startPaused = true;
    }

    public void StopCapture()
    {
        _movieCapture.StopCapture();
        CreateVideoItem();
        totalRecordTime = 0;
        startRecordingTimer = false;
    }

    public void ReStartCapture()
    {
        _movieCapture.StopCapture();
        _movieCapture.StartCapture();
       
    }

    Transform lastSelectVideo;
    public void CreateVideoItem()
    {

        for (int i = 0; i < VideoItemParent.childCount; i++)
        {
            GameObject.Destroy(VideoItemParent.GetChild(i).gameObject);

        }
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".meta")) continue;
                if (fileInfos[i].Name.EndsWith(".mp4"))
                {
                    GameObject videoItemObj = GameObject.Instantiate<GameObject>(videoItem);
                    videoItemObj.transform.SetParent(VideoItemParent);
                    videoItemObj.transform.localScale = Vector3.one;
                    videoItemObj.transform.localPosition = new Vector3(videoItemObj.transform.localPosition.x, videoItemObj.transform.localPosition.y, 0);
                    videoItemObj.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItemObj.transform.GetComponent<VideoItem>().videoItemScenarioName = GameManager.Instance.GetScenarioName();
                    videoItemObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnClickChooseVideo(videoItemObj.transform);

                        VideoPlayerController._instance.videoPlayer.url = videoItemObj.transform.GetComponent<VideoItem>().videoPath;
                        
                    });
                    //Debug.Log( "FullName:" + fileInfos[i].FullName );  
                    //Debug.Log( "DirectoryName:" + fileInfos[i].DirectoryName );  
                }
            }
        }
    }

    /// <summary>
    /// 点击选择要播放的视频
    /// </summary>
    /// <param name="trans"></param>
    public void OnClickChooseVideo(Transform trans)
    {
        if (lastSelectVideo == null)
        {
            lastSelectVideo = trans;
        }
        if (lastSelectVideo != null && lastSelectVideo != trans)
        {
            //if (LastSelectTransScene.GetComponent<Outline>().enabled)
            //{
            //    LastSelectTransScene.GetComponent<Outline>().enabled = false;
            //}
            if (lastSelectVideo.GetComponent<Image>().material != null)
            {
                lastSelectVideo.GetComponent<Image>().material = null;
            }
            lastSelectVideo = trans;
        }
        bool hasShow = ShowOutline(trans);
        if (hasShow)
        {
            videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                panel4.SetActive(true);
            });
        }

    }

    public void StopPlay()
    {
        VideoPlayerController._instance.videoPlayer.Stop();
    }

    public bool ShowOutline(Transform trans)
    {
        if (trans.GetComponent<Image>().material != outline)
        {

            trans.GetComponent<Image>().material = outline;
            return true;
        }
        else
        {

            trans.GetComponent<Image>().material = null;
            return false;
        }
    }

    public int  GetVideoItemIndex()
    {
      
        for (int i = 0; i < VideoItemParent.childCount; i++)
        {
            if(VideoItemParent.GetChild(i).GetComponent<Image>().material != null)
            {
                return i;
            }
        }
        return 0;
    }

    public string GetRecordDate()
    {
        
        return null;

    }
}
