using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BigScreenPlayWindow : MonoBehaviour
{
   

    //public static Panel4_c _instance;
    #region 字段
    public Transform buttons;
    public Text txt_nowTime;
    public Text txt_allTime;
    public GameObject slider_volume;
    public Slider slider_video;
    public VideoPlayer videoPlayer;
    private RawImage rawImage;

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
                    case "btn_Confirm":
                        Hide();
                        break;
                    case "btn_Return":
                        Hide();
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
        AllTime();
    }

    private void OnEnable()
    {

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

    /// <summary>
    /// 隐藏当前面板
    /// </summary>
    public void Hide()
    {
        //Panel3_c._Instance.videoPlayer.Stop();
        this.gameObject.SetActive(false);
        VideoPlayerController._instance.videoPlayer.Stop();
    }

}
