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
    public AVProMovieCaptureBase _movieCapture;
    public Text RecordingTime;
    public Text RecordingTimeOnScreen;
    public Image borderStyleImage;
    public Image recordBG;
    public GameObject panel1;
    public Camera recorderCamera;


    private int currentHour, currentMinute, currentSecond;
    private float totalRecordTime;
    bool startRecordingTimer = false;    

    DirectoryInfo directoryInfo;
    public FileInfo[] fileInfos;

    private void Awake()
    {      
        _movieCapture._downScale = AVProMovieCaptureBase.DownScale.Original;//原画画质
        _movieCapture._frameRate = AVProMovieCaptureBase.FrameRate.Thirty;//帧数
        _movieCapture._outputFolderPath = Application.streamingAssetsPath;//保存视频的路径
        _movieCapture._useMediaFoundationH264 = true;
        _movieCapture._autoFilenameExtension = "mp4";//格式
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
    }

    private void Update()
    {
        if (startRecordingTimer)
        {
            //totalRecordTime = _movieCapture._frameTotal / _movieCapture._fps;
            totalRecordTime += Time.deltaTime;
            RecordTime();
        }
    }

    private void OnEnable()
    {
        recordBG.sprite = GameManager.Instance.GetSceneImage();
    }

    public void RecordTime()
    {
        currentHour = (int)totalRecordTime / 3600;
        currentMinute = (int)(totalRecordTime - 3600 * currentHour) / 60;
        currentSecond = (int)(totalRecordTime - 60 * currentMinute - 3600 * currentHour);
        RecordingTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
        RecordingTimeOnScreen.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
        //RecordingTimeOnScreen.text = Time.time.ToString("00:00:00");
    }

    #region 录制相关
    public void StartCapture()
    {
        _movieCapture.StartCapture();
        totalRecordTime = 0;
        startRecordingTimer = true;
        borderStyleImage.gameObject.SetActive(true);
        borderStyleImage.sprite = GameManager.Instance.GetborderStyle();
        SetCameraFilter();       
    }

    private void SetCameraFilter()
    {
       if(GameManager.Instance.GetFilterStyle() == FilterStyle.vintage)
        {
            recorderCamera.GetComponent<CameraFilterPack_TV_Vintage>().enabled = true;
            recorderCamera.GetComponent<CameraFilterPack_Blend2Camera_SoftLight>().enabled = false;
            recorderCamera.GetComponent<CameraFilterPack_Film_ColorPerfection>().enabled = false;
        }
       if(GameManager.Instance.GetFilterStyle() == FilterStyle.soft)
        {
            recorderCamera.GetComponent<CameraFilterPack_TV_Vintage>().enabled = false;
            recorderCamera.GetComponent<CameraFilterPack_Blend2Camera_SoftLight>().enabled = true;
            recorderCamera.GetComponent<CameraFilterPack_Film_ColorPerfection>().enabled = false;
        }
       if(GameManager.Instance.GetFilterStyle() == FilterStyle.bright)
        {
            recorderCamera.GetComponent<CameraFilterPack_TV_Vintage>().enabled = false;
            recorderCamera.GetComponent<CameraFilterPack_Blend2Camera_SoftLight>().enabled = false;
            recorderCamera.GetComponent<CameraFilterPack_Film_ColorPerfection>().enabled = true;
        }
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
        totalRecordTime = 0;
        startRecordingTimer = false;
        borderStyleImage.gameObject.SetActive(false);
        CancelCameraFilter();
        SaveVideoItemInfo();
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

    /// <summary>
    /// 录制完成后保存录制的信息
    /// </summary>
    public void SaveVideoItemInfo()
    {
          
        if (Directory.Exists(Application.streamingAssetsPath))
        {
               
            fileInfos = directoryInfo.GetFiles("*.mp4", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".mp4"))
                {                                      
                    if (!VideoPlayerController._instance.videoItemPathList.Contains(fileInfos[i].FullName))
                    {

                        VideoPlayerController._instance.videoItemPathList.Add(fileInfos[i].FullName);
                        //VideoPlayerController._instance.videoItemInfo.Add(fileInfos[i].FullName, fileInfos[i].CreationTime);
                        if (!PlayerPrefs.HasKey(fileInfos[i].FullName))
                        {
                            PlayerPrefs.SetString(fileInfos[i].FullName, fileInfos[i].CreationTime.ToString());  //将录制视频的时间存储到本地
                            PlayerPrefs.SetString(fileInfos[i].CreationTime.ToString(), GameManager.Instance.GetScenarioName());
                            //PlayerPrefs.SetString(GameManager.Instance.GetScenarioName(), DateTime.Now.ToString());
                            VideoPlayerController._instance.videoItemImage.Add(fileInfos[i].FullName, GameManager.Instance.GetSceneImage());
                        }
                    }
                }
            }
            
        }
    }

    /// <summary>
    /// 录制完成后退出滤镜模式
    /// </summary>
    private void CancelCameraFilter()
    {
        recorderCamera.GetComponent<CameraFilterPack_TV_Vintage>().enabled = false;
        recorderCamera.GetComponent<CameraFilterPack_Blend2Camera_SoftLight>().enabled = false;
        recorderCamera.GetComponent<CameraFilterPack_Film_ColorPerfection>().enabled = false;
    }

}
