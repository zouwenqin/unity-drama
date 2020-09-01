using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoShowWindow : MonoBehaviour
{
   
    #region 字段
    public Transform buttons;
    public Text txt_nowTime;
    public Text txt_allTime;
    public GameObject slider_volume;
    public Slider slider_video;
    public VideoPlayer videoPlayer;
    private RawImage rawImage;
    public Text RecordDate;
    public Text ScenarioName;
    public GameObject bigScreenShowPanel;

    private bool showVolumeSlider = false;
    private int currentMinute, currentSecond;
    private int totalMinute, totalSecond;
    #endregion

    private void Start()
    {       
        //Debug.LogError(GameManager.Instance.GetScenarioName());
       
        //Debug.LogError(RecordDate.text);
        //RecordDate.text = VideoPlayerController._instance.videoItemDate[Panel3_c._Instance.GetVideoItemIndex()].ToString();
        //_instance = this;
        foreach(Transform go in buttons)
        {
            go.GetComponent<Button>().onClick.AddListener(delegate 
            {
                switch (go.name)
                {
                    case "btn_Play":
                        VideoPlayerController._instance.videoPlayer.Play();
                        break;
                    //case "btn_Previous":
                    //    break;
                    //case "btn_Next":
                    //    break;
                    case "btn_Stop":
                        VideoPlayerController._instance.videoPlayer.Pause();
                        break;
                    //case "btn_Share":
                    //    break;
                    case "btn_Delete":
                        DeleteVideo();
                        break;
                    case "btn_Volume":
                        showVolumeSlider = !showVolumeSlider;
                        if (showVolumeSlider)
                        {
                            slider_volume.SetActive(true);
                        }
                        else
                        {
                            slider_volume.SetActive(false);
                        }
                        break;
                    case "btn_SetFullScreen":
                        OnClickSetFullScreen();
                        break;
                }
            });
        }
        VideoSliderListener();
        InitVolume();
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(VideoPlayerController._instance.videoPlayer.url))
        {
            RecordDate.text = PlayerPrefs.GetString(VideoPlayerController._instance.videoPlayer.url);
        }
        if (PlayerPrefs.HasKey(RecordDate.text))
        {
            ScenarioName.text = PlayerPrefs.GetString(RecordDate.text);
        }
        AllTime();
    }

    private void Update()
    {
        NowTime();
        AllTime();
    }

    public void DeleteVideo()
    {
        //if (File.Exists(VideoPlayerController._instance.videoPlayer.url))
        //{
        //    File.Delete(VideoPlayerController._instance.videoPlayer.url);
        //    File.Delete(VideoPlayerController._instance.videoPlayer.url + ".meta");
        //}
        if (File.Exists(RecordPanel._instance.fileInfos[RecordPanel._instance.GetVideoItemIndex()].FullName))
        {
            PlayerPrefs.DeleteKey(RecordPanel._instance.fileInfos[RecordPanel._instance.GetVideoItemIndex()].FullName);
            PlayerPrefs.DeleteKey(RecordPanel._instance.fileInfos[RecordPanel._instance.GetVideoItemIndex()].CreationTime.ToString());
            File.Delete(Application.streamingAssetsPath +'/'+ RecordPanel._instance.fileInfos[RecordPanel._instance.GetVideoItemIndex()].Name);
        }
        //PlayerPrefs.DeleteKey(VideoPlayerController._instance.videoPlayer.url);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        VideoPlayerController._instance.videoItemList.RemoveAt(RecordPanel._instance.GetVideoItemIndex());
        VideoPlayerController._instance.videoItemPathList.Remove(VideoPlayerController._instance.videoPlayer.url);
        GameObject.Destroy(RecordPanel._instance.videoItemParent.GetChild(RecordPanel._instance.GetVideoItemIndex()).gameObject);
       
        RecordPanel._instance.CreateVideoItem();
        this.gameObject.SetActive(false);
        for (int i = 0; i < RecordPanel._instance.videoItemParent.childCount; i++)
        {
            RecordPanel._instance.videoItemParent.GetChild(i).GetComponent<Image>().material = null;
        }
        
    }

    public void InitVolume()
    {
        slider_volume.SetActive(false);
        slider_volume.GetComponent<Slider>().value = 0.5f;
        videoPlayer.SetDirectAudioVolume(0, slider_volume.GetComponent<Slider>().value);
        slider_volume.GetComponent<Slider>().onValueChanged.AddListener(value => {
            videoPlayer.SetDirectAudioVolume(0, value);
        });
    }

    public void VideoSliderListener()
    {
        slider_video.onValueChanged.AddListener((float value) => {
            if (slider_video.value == 1)
                return;
            videoPlayer.frame = long.Parse((value * videoPlayer.frameCount).ToString("0."));
        });
    }

    public void NowTime()
    {
        slider_video.value = float.Parse(videoPlayer.frame.ToString()) / float.Parse(videoPlayer.frameCount.ToString()); ;
        currentMinute = (int)videoPlayer.time / 60;
        currentSecond = (int)(videoPlayer.time - currentMinute * 60);
        txt_nowTime.text = string.Format("{0:D2}:{1:D2}", currentMinute, currentSecond);
        //slider_video.value = float.Parse( videoPlayer.time.ToString()) / float.Parse( videoPlayer.length.ToString());
    }

    public void AllTime()
    {
        totalMinute = (int)(videoPlayer.frameCount / videoPlayer.frameRate / 60);
        totalSecond = (int)(videoPlayer.frameCount / videoPlayer.frameRate % 60);
        txt_allTime.text = string.Format("{0:D2}:{1:D2}", totalMinute, totalSecond);
    }

    public void OnClickSetFullScreen()
    {
        
        this.transform.Find("FullScreenInterface").gameObject.SetActive(true);
        rawImage = GameObject.Find("FullScreenInterface").GetComponent<RawImage>();
    }
    
    public void Hide()
    {
        //Panel3_c._Instance.videoPlayer.Stop();
        this.gameObject.SetActive(false);
        VideoPlayerController._instance.videoPlayer.Stop();
    }
}
