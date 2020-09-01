using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class RecordPanel : MonoBehaviour
{
    public static RecordPanel _instance; 
    public Transform videoItemParent;
    public GameObject videoItemPrefab;
    public AVProMovieCaptureBase _movieCapture;
    public Text RecordingTime;
    public Text RecordingTimeOnScreen;
    public GameObject videoPlayPanel;

    public GameObject panel1;
    public GameObject videoShowWindow;
    private Material outline;

    public InputField startSearchTimeBar;
    public InputField endSearchTimeBar;
    public InputField searchBar;
    public Button SearchButton;
    private int currentHour, currentMinute, currentSecond;
    private float totalRecordTime;
    bool startRecordingTimer = false;
    
    private string searchBarInput;
         
    [HideInInspector]
    public int videoIndex;

    DirectoryInfo directoryInfo;
    public FileInfo[] fileInfos;
    private void Awake()
    {
        _instance = this;
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
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
        searchBar.onEndEdit.AddListener((string str) =>
        {
            searchBarInput = str;
        });
        SearchButton.onClick.AddListener(OnClickSearchButton);
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
        SearchDateShow();
    }

    #region 录制相关函数
    public void NowTime()
    {
        currentHour = (int)totalRecordTime / 3600;
        currentMinute = (int)(totalRecordTime - 3600 * currentHour) / 60;
        currentSecond = (int)(totalRecordTime - 60 * currentMinute - 3600 * currentHour);
        RecordingTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
        RecordingTimeOnScreen.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
    }
    public void StartCapture()
    {
        _movieCapture.StartCapture();
        totalRecordTime = 0;
        startRecordingTimer = true;
        
    }

    public void PauseCapture()
    {
        startRecordingTimer = false;
        _movieCapture.PauseCapture();
    }

    public void ContinuePause()
    {
        startRecordingTimer = true;
        _movieCapture.ResumeCapture();

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
        this.gameObject.SetActive(false);
        panel1.GetComponent<SelectPanel>().ClearData();
        panel1.SetActive(true);
        PanelManager._instance.currentState = CurrentPanel.panel1;

    }
#endregion

    public void CreateVideoItem()
    {
        VideoPlayerController._instance.videoItemList.Clear();
        for (int i = videoItemParent.childCount - 1; i >= 0; i--)
        {
            Destroy(videoItemParent.GetChild(i).gameObject);
        }      
        if (Directory.Exists(Application.streamingAssetsPath))
        {
               
            fileInfos = directoryInfo.GetFiles("*.mp4", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".mp4"))
                {
                    //Debug.LogError(fileInfos[i].Name);
                    //Debug.LogError(fileInfos[i].CreationTime);
                    GameObject videoItemObj = GameObject.Instantiate<GameObject>(videoItemPrefab,videoItemParent,false);
                    if (!VideoPlayerController._instance.videoItemPathList.Contains(fileInfos[i].FullName))
                    {

                        VideoPlayerController._instance.videoItemPathList.Add(fileInfos[i].FullName);
                        //VideoPlayerController._instance.videoItemInfo.Add(fileInfos[i].FullName, fileInfos[i].CreationTime);
                        if (!PlayerPrefs.HasKey(fileInfos[i].FullName))
                        {
                            PlayerPrefs.SetString(fileInfos[i].FullName, fileInfos[i].CreationTime.ToString());  //将录制视频的时间存储到本地
                            PlayerPrefs.SetString(fileInfos[i].CreationTime.ToString(), GameManager.Instance.GetScenarioName());
                            //PlayerPrefs.SetString(GameManager.Instance.GetScenarioName(), DateTime.Now.ToString());

                        }
                    }
                   
                    videoItemObj.transform.localScale = Vector3.one;
                    videoItemObj.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItemObj.transform.GetComponent<VideoItem>().recordScenarioName = PlayerPrefs.GetString(fileInfos[i].CreationTime.ToString());
                    videoItemObj.transform.GetComponent<VideoItem>().recordTime = fileInfos[i].CreationTime;
                    VideoPlayerController._instance.videoItemList.Add(videoItemObj);
                    videoItemObj.transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    int index = i;
                    videoItemObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //Debug.LogError("beforeParentChildCount " + videoItemParent.childCount);
                        //OnClickChooseVideo(videoItemObj.transform);
                       
                        OnClickChooseVideo(index);
                        if (videoItemObj.GetComponent<Image>().material == outline)
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

    public void OnClickChooseVideo(int index)
    {
        for (int i = 0; i < videoItemParent.childCount; i++)
        {
            if(i == index)
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = outline;
                videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    videoShowWindow.SetActive(true);
                });
            }
            else
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = null;
            }
        }
    }

    /// <summary>
    /// 获取视频项目索引,可能有问题
    /// </summary>
    /// <returns></returns>
    public int  GetVideoItemIndex()
    {

        //int childCount = videoItemParent.childCount - 1;  //TODO 改了不要紧?
        int childCount = videoItemParent.childCount;
        for (int i = childCount - 1 ; i >= 0; i--)
        {
            if(videoItemParent.GetChild(i).GetComponent<Image>().material == outline)
            {
                return i;
            }
        }
        return 0;
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
    
    public void  SearchDateShow()
    {
        endSearchTimeBar.text = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth((int)DateTime.Now.Year, (int)DateTime.Now.Month));
        startSearchTimeBar.text = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, "01");
        searchBar.placeholder.GetComponent<Text>().text = "搜索剧名";
    }
    public void OnClickSearchButton()
    {
        for (int i = 0; i < videoItemParent.childCount; i++)
        {
            if(videoItemParent.GetChild(i).GetComponent<VideoItem>().recordScenarioName == searchBarInput)
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = outline;
                //videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
                //{
                //    videoShowWindow.SetActive(true);
                //});
            }
            else
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = null;
            }
        }
    }
}
