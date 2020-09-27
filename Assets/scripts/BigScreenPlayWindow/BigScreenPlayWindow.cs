using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BigScreenPlayWindow : MonoBehaviour
{
    #region 字段
    public BigScreenShowPanel bigScreenShowPanel;
    
    public Transform buttons;
    public Text txt_nowTime;
    public Text txt_allTime;
    public GameObject slider_volume;
    public Slider slider_video;
    public VideoPlayer videoPlayer;
    private RawImage rawImage;
    public Text RecordDate;
    public Text ScenarioName;

    private bool showVolumeSlider = false;
    private int currentMinute, currentSecond;
    private int totalMinute, totalSecond;

    #endregion

    private void Start()
    {
        //_instance = this;
        foreach (Transform go in buttons)
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
                    case "btn_Share":
                        break;
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

    private void Update()
    {
        NowTime();       
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(VideoPlayerController._instance.videoPlayer.url))
        {
            RecordDate.text = PlayerPrefs.GetString(VideoPlayerController._instance.videoPlayer.url);
        }
        if (PlayerPrefs.HasKey(RecordDate.text))
        {
            //Debug.LogError(PlayerPrefs.GetString(RecordDate.text));
            ScenarioName.text = PlayerPrefs.GetString(RecordDate.text);
        }
        AllTime();
    }

    /// <summary>
    /// 初始化视频播放的声音
    /// </summary>
    public void InitVolume()
    {
        slider_volume.SetActive(false);
        slider_volume.GetComponent<Slider>().value = 0.5f;
        videoPlayer.SetDirectAudioVolume(0, slider_volume.GetComponent<Slider>().value);
        slider_volume.GetComponent<Slider>().onValueChanged.AddListener(value => {
            videoPlayer.SetDirectAudioVolume(0, value);
        });
    }

    /// <summary>
    /// 监听播放视频时播放进度条的变化
    /// </summary>
    public void VideoSliderListener()
    {
        slider_video.onValueChanged.AddListener((float value) => {
            if (slider_video.value == 1)
                return;
            videoPlayer.frame = long.Parse((value * videoPlayer.frameCount).ToString("0."));
        });
    }

    /// <summary>
    /// 视频正在播放的时长进度
    /// </summary>
    public void NowTime()
    {
        
        slider_video.value = float.Parse(videoPlayer.frame.ToString()) / float.Parse(videoPlayer.frameCount.ToString()); ;
        currentMinute = (int)videoPlayer.time / 60;
        currentSecond = (int)(videoPlayer.time - currentMinute * 60);
        txt_nowTime.text = string.Format("{0:D2}:{1:D2}", currentMinute, currentSecond);
        //slider_video.value = float.Parse( videoPlayer.time.ToString()) / float.Parse( videoPlayer.length.ToString());
    }

    /// <summary>
    /// 视频片段总时长显示
    /// </summary>
    public void AllTime()
    {
        float duration;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (val) =>
        {
            duration = videoPlayer.frameCount / videoPlayer.frameRate;
            totalMinute = (int)(videoPlayer.frameCount / videoPlayer.frameRate) / 60;
            totalSecond = ((int)(videoPlayer.frameCount / videoPlayer.frameRate)) % 60;           
            txt_allTime.text = string.Format("{0:D2}:{1:D2}", totalMinute, totalSecond);
        };        
        
    }

    public void OnClickSetFullScreen()
    {

        this.transform.Find("FullScreenInterface").gameObject.SetActive(true);
        rawImage = GameObject.Find("FullScreenInterface").GetComponent<RawImage>();
    }

    /// <summary>
    /// 隐藏当前面板
    /// </summary>
    public void Hide()
    {
        //Panel3_c._Instance.videoPlayer.Stop();
        this.gameObject.SetActive(false);
        VideoPlayerController._instance.videoPlayer.Stop();
    }

    public void DeleteVideo()
    {
        if (File.Exists(bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].FullName))
        {
            if (VideoPlayerController._instance.videoItemImage.ContainsKey(bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].FullName))
            {
                VideoPlayerController._instance.videoItemImage.Remove(bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].FullName);
            }
            PlayerPrefs.DeleteKey(bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].FullName);
            PlayerPrefs.DeleteKey(bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].CreationTime.ToString());
            File.Delete(Application.streamingAssetsPath + '/' + bigScreenShowPanel.fileInfos[bigScreenShowPanel.GetVideoItemIndex()].Name);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        VideoPlayerController._instance.videoItemList.RemoveAt(bigScreenShowPanel.GetVideoItemIndex());
        VideoPlayerController._instance.videoItemPathList.Remove(VideoPlayerController._instance.videoPlayer.url);
        GameObject.Destroy(bigScreenShowPanel.videoItemParent.GetChild(bigScreenShowPanel.GetVideoItemIndex()).gameObject);

        bigScreenShowPanel.CreateVideoItem();
        this.gameObject.SetActive(false);
        for (int i = 0; i < bigScreenShowPanel.videoItemParent.childCount; i++)
        {
            bigScreenShowPanel.videoItemParent.GetChild(i).GetComponent<Image>().material = null;
        }

    }
}
