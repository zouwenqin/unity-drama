using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Panel3_c : MonoBehaviour
{
    public static Panel3_c _instance; 
    public Transform videoItemParent;
    public GameObject videoItemPrefab;
    public AVProMovieCaptureBase _movieCapture;
    public Image[] ImagePerson;
    public Text RecordingTime;
    public GameObject videoPlayPanel;

    public GameObject panel1;
    public GameObject videoShowWindow;
    private Material outline;
    public List<GameObject> videoItemPool = new List<GameObject>();

    private int currentHour, currentMinute, currentSecond;
    private float totalRecordTime;
    bool startRecordingTimer = false;

    [HideInInspector]
    public int videoIndex;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        
        _movieCapture._downScale = AVProMovieCaptureBase.DownScale.Original;//原画画质
        _movieCapture._frameRate = AVProMovieCaptureBase.FrameRate.Thirty;//帧数
        _movieCapture._outputFolderPath = Application.streamingAssetsPath;//保存视频的路径
        //_movieCapture._codecName = "Media Foundation H.264(MP4)";//设置视频格式
        _movieCapture._useMediaFoundationH264 = true;
        _movieCapture._autoFilenameExtension = "mp4";//格式
        //_movieCapture._autoFilenamePrefix = null;
        //_movieCapture._autoGenerateFilename = false;
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

    private void OnEnable()
    {
        CreateVideoItem();
    }

    public void NowTime()
    {
        currentHour = (int)totalRecordTime / 3600;
        currentMinute = (int)(totalRecordTime - 3600 * currentHour) / 60;
        currentSecond = (int)(totalRecordTime - 60 * currentMinute - 3600 * currentHour);
        RecordingTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
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
        
        //Debug.LogError(_movieCapture._outputFolderPath);
         
        
        CreateVideoItem();
        //PlayerPrefs.SetString(GameManager.Instance.GetScenarioName(), DateTime.Now.ToString());
        //Debug.LogError(GameManager.Instance.GetScenarioName());
        //Debug.LogError(PlayerPrefs.GetString(GameManager.Instance.GetScenarioName()));
        totalRecordTime = 0;
        startRecordingTimer = false;
    }

    public void ReStartCapture()
    {
        _movieCapture.StopCapture();
        this.gameObject.SetActive(false);
        //SceneManager.LoadSceneAsync("mainScene");
        panel1.GetComponent<PanelOne_C>().ClearData();
        panel1.GetComponent<PanelOne_C>().inputField_scenarioName.text = null;
        panel1.GetComponent<PanelOne_C>().scenarioName = null;
        panel1.SetActive(true);
        PanelManager._instance.currentState = CurrentPanel.panel1;

    }

    
    public void CreateVideoItem()
    {
        VideoPlayerController._instance.videoItemList.Clear();
        for (int i = videoItemParent.childCount - 1; i >= 0; i--)
        {
            Destroy(videoItemParent.GetChild(i).gameObject);
        }
       // HideVideoItem();
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            
            //Debug.LogError(Application.dataPath);
            Debug.LogError(Application.streamingAssetsPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".meta")) continue;
                if (fileInfos[i].Name.EndsWith(".mp4"))
                {
                    GameObject videoItemObj = GameObject.Instantiate<GameObject>(videoItemPrefab,videoItemParent,false);
                    if (!VideoPlayerController._instance.videoItemPathList.Contains(fileInfos[i].FullName))
                    {
                        
                        VideoPlayerController._instance.videoItemPathList.Add(fileInfos[i].FullName);
                        if (!PlayerPrefs.HasKey(fileInfos[i].FullName))
                        {
                            PlayerPrefs.SetString(fileInfos[i].FullName, GameManager.Instance.GetScenarioName());  //将录制视频的时间存储到本地
                            PlayerPrefs.SetString(GameManager.Instance.GetScenarioName(), DateTime.Now.ToString());
                         
                        }
                    }
                    VideoPlayerController._instance.videoItemList.Add(videoItemObj);
                    //videoItemObj.transform.SetParent(VideoItemParent);
                    videoItemObj.transform.localScale = Vector3.one;
                    //videoItemObj.transform.localPosition = new Vector3(videoItemObj.transform.localPosition.x, videoItemObj.transform.localPosition.y, 0);
                    videoItemObj.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItemObj.transform.GetComponent<VideoItem>().videoItemScenarioName = GameManager.Instance.GetScenarioName();
                    videoItemObj.transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    videoItemObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //Debug.LogError("beforeParentChildCount " + videoItemParent.childCount);
                        OnClickChooseVideo(videoItemObj.transform);
                        if (videoItemObj.GetComponent<Image>().material != null)
                        {
                            VideoPlayerController._instance.videoPlayer.url = videoItemObj.transform.GetComponent<VideoItem>().videoPath;
                        }
                        
                    });
                    //Debug.Log( "FullName:" + fileInfos[i].FullName );  
                    //Debug.Log( "DirectoryName:" + fileInfos[i].DirectoryName );  
                }
            }
            
        }
    }

    Transform lastSelectVideo;
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
                videoShowWindow.SetActive(true);
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
       
        int childCount = videoItemParent.childCount - 1;
        for (int i = childCount - 1 ; i >= 0; i--)
        {
            if(videoItemParent.GetChild(i).GetComponent<Image>().material != null)
            {
                return i;
            }
        }
        return 0;
    }

    public GameObject getVideoItemInPool()
    {
        int childCount = videoItemParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            if (videoItemParent.GetChild(i).GetComponent<Image>().material != null)
            {
                return videoItemParent.GetChild(i).gameObject;
            }
        }
        return null;
    }
    public void HideVideoItem()
    {
        for (int i = 0; i < videoItemPool.Count; i++)
        {
            videoItemPool[i].SetActive(false);
        }
    }
    public GameObject GetVideoItem()
    {
        for (int i = 0; i < videoItemPool.Count; i++)
        {
            if (videoItemPool[i].activeSelf == false)
            {
                videoItemPool[i].SetActive(true);
                return videoItemPool[i];
            }
        }
        GameObject obj = GameObject.Instantiate(videoItemPrefab);
        videoItemPool.Add(obj);
        return obj;
    }

    public string GetRecordDate()
    {
        
        return null;

    }
}
