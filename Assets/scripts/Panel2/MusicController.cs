using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayMode
{
    STRIGHT,
    CIRCLE,
    RANDOM
}

public enum MusicStyle
{
    LYRIC,
    CHEERFUL,
    DYNAMIC,
    CLASSIC
}

public class MusicController : MonoBehaviour
{
    public AudioClip[] lyricAudioClips;
    public AudioClip[] cheerfulAudioClips;
    public AudioSource audioSource;
    public GameObject musicItemPrefabs;

    /// <summary>
    /// 当前歌曲播放时间
    /// </summary>
    public Text nowTime;
    /// <summary>
    /// 歌曲总时间
    /// </summary>
    public Text allTime;
    /// <summary>
    /// 音乐进度条slider
    /// </summary>
    public Slider slider;
    public Button button_play_stop;
    public Button btn_volume;
    public GameObject slider_volume;
    public Button btn_playMode;
    private PlayMode playMode = PlayMode.STRIGHT;
    private MusicStyle musicStyle = MusicStyle.LYRIC;

    private GameObject musicItemsParent;
    private bool isPlayAudio;
    private bool isShowVolumeSlider;
    private int currentMusicIndex;
    private int t; //播放模式选择索引
    

    /// <summary>
    /// 存放实例化的音乐列表项
    /// </summary>
    private GameObject item;
    /// <summary>
    /// 存放音乐列表预制体的对象池
    /// </summary>
    private List<GameObject> musicItemsPool = new List<GameObject>();
    /// <summary>
    /// 临时存放初始化音乐列表的数据的列表
    /// </summary>
    private List<GameObject> musicItems = new List<GameObject>() ;

    private void Start()
    {
        InitVolume();
        //btn_playMode.onClick.AddListener(OnClickChangePlayMode);
        MusicSliderListener();
        musicItemsParent = GameObject.Find("TwoStyleAndMusicPanel/MusicController/MusicOptionsPanel");
    }

    private void Update()
    {
        NowTime();
    }

    #region 音乐列表初始化相关函数
    public void InitLyricMusicStyle()
    {
        musicStyle = MusicStyle.LYRIC;
        musicItems.Clear();
        HideAllMusicItems();
        for (int i = 0; i < lyricAudioClips.Length; i++)
        {
            item = GetMusicItems();
            if (i == 0)
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
            }
            else
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
            }
            item.transform.Find("Toggle").GetComponent<Toggle>().group = GameObject.Find("TwoStyleAndMusicPanel/MusicController/MusicOptionsPanel").GetComponent<ToggleGroup>();
            
            item.transform.SetParent(GameObject.Find("TwoStyleAndMusicPanel/MusicController/MusicOptionsPanel").transform, false);
            item.transform.Find("txt_Number").GetComponent<Text>().text = string.Format("0{0}", i + 1);
            item.transform.Find("txt_Name").GetComponent<Text>().text = lyricAudioClips[i].name;
            item.transform.Find("txt_TotalTime").GetComponent<Text>().text = GetTotalMusicTime(lyricAudioClips[i]);
            
            musicItems.Add(item);
        }
        GameObject[] obj = new GameObject[musicItems.Count];
        musicItems.CopyTo(obj);
        foreach (GameObject gameObject in obj)
        {
            gameObject.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((bool v) =>
            {
                if (v)
                {
                    int index = Array.IndexOf(obj, gameObject);
                    PlayMusic(lyricAudioClips, index);
                }
            });
        }
        

    }

    
    public void InitCheerfulMusicStyle()
    {
        musicStyle = MusicStyle.CHEERFUL;
        musicItems.Clear();
        HideAllMusicItems();
        for (int i = 0; i < cheerfulAudioClips.Length; i++)
        {
            item = GetMusicItems();
            if (i == 0)
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
            }
            else
            {
                item.transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
            }
            item.transform.Find("Toggle").GetComponent<Toggle>().group = GameObject.Find("TwoStyleAndMusicPanel/MusicController/MusicOptionsPanel").GetComponent<ToggleGroup>();           
            item.transform.SetParent(GameObject.Find("TwoStyleAndMusicPanel/MusicController/MusicOptionsPanel").transform, false);
            item.transform.Find("txt_Number").GetComponent<Text>().text = string.Format("0{0}", i + 1);
            item.transform.Find("txt_Name").GetComponent<Text>().text = cheerfulAudioClips[i].name;
            item.transform.Find("txt_TotalTime").GetComponent<Text>().text = GetTotalMusicTime(cheerfulAudioClips[i]);
            musicItems.Add(item);
        }
        
        GameObject[] obj = new GameObject[musicItems.Count];
        musicItems.CopyTo(obj);
       
        foreach (GameObject gameObject in obj)
        {
            gameObject.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((bool v) =>
            {
                if (v)
                {
                    int index = Array.IndexOf(obj, gameObject);
                    //int index = musicItems.IndexOf(gameObject);
                    PlayMusic(cheerfulAudioClips, index);
                }
            });
        }
    }

    

    /// <summary>
    /// 利用对象池原理实例化音乐预制体
    /// </summary>
    /// <returns></returns>
    public GameObject GetMusicItems()
    {
        for (int i = 0; i < musicItemsPool.Count; i++)
           
        {
            
            if (musicItemsPool[i].activeSelf == false)
            {
                musicItemsPool[i].SetActive(true);
                return musicItemsPool[i];
            }
        }
       
            GameObject obj =  GameObject.Instantiate(musicItemPrefabs);
            musicItemsPool.Add(obj);
            return obj;
        
    }

    public void HideAllMusicItems()
    {
        for (int i = 0; i < musicItemsPool.Count; i++)
        {
            musicItemsPool[i].SetActive(false);
        }
    }

    public void OnLyricToggleValueChanged(bool v)
    {
        if (v)
        {
            InitLyricMusicStyle();
        }
    }

    #endregion

    public void OnClickPlayOrStop()
    {
        //TODO 在resource文件夹下建立存放播放暂停图品，播放模式的图片
        if (isPlayAudio == false)
        {
            isPlayAudio = true;
            //audioSource.clip = panelTwo_M.currentAudioClip;
            audioSource.Play();
            //button_play_stop.image.sprite = play;
        }
        else
        {
            isPlayAudio = false;
            audioSource.Pause();
            //button_play_stop.image.sprite = stop;
        }
    }

    public void InitVolume()
    {
        slider_volume.SetActive(false);
        slider_volume.GetComponent<Slider>().value = 20;
        slider_volume.GetComponent<Slider>().onValueChanged.AddListener(value =>
        {
            audioSource.volume = value * 0.01f;
        });
        btn_volume.onClick.AddListener(() => {
            isShowVolumeSlider = !isShowVolumeSlider;
            if (isShowVolumeSlider)
            {
                slider_volume.SetActive(true);
            }
            else
            {
                slider_volume.SetActive(false);
            }
        });
    }

    public void OnClickChangePlayMode()
    {
        Debug.LogError(playMode);
        
        t++;
        if(t ==1)
        {
            playMode = PlayMode.STRIGHT;
        }
        if(t == 2)
        {
            playMode = PlayMode.RANDOM;
        }
        if(t == 3)
        {
            playMode = PlayMode.CIRCLE;
            t = 0;
        }
    }

    public void MusicSliderListener()
    {
        slider.onValueChanged.AddListener(delegate
        {
            //拖动进度条，歌曲从相应位置播放
            audioSource.time = slider.value * audioSource.clip.length;
        });
    }

    public void PlayMusic(AudioClip[] audioClip,int index)
    {
        audioSource.Stop();
        //Debug.LogError(index);
        audioSource.clip = audioClip[index] ;
        allTime.text =  GetTotalMusicTime(audioClip[index]);
        slider.value = 0;
        musicItemsParent.transform.GetChild(index).GetComponentInChildren<Toggle>().isOn = true;
        //if (audioSource.clip.name == musicItemsParent.transform.GetChild(index).GetChild(1).GetComponent<Text>().text)
        //{
        //    musicItemsParent.transform.GetChild(index).GetComponentInChildren<Toggle>().isOn = true;
        //    Debug.LogError(audioSource.clip.name);
        //}
        audioSource.Play();
    }

    public void NowTime()
    {
        nowTime.text = string.Format("{0:D2}:{1:D2}", (int)audioSource.time / 60, (int)audioSource.time % 60);
        slider.value = audioSource.time / audioSource.clip.length;
    }

    public string GetTotalMusicTime(AudioClip audioClip)
    {
        string str = string.Format("{0:D2}:{1:D2}", (int)audioClip.length / 60, (int)audioClip.length % 60);
        return str;
    }

    public void OnClickPreviousMusic()
    {
        if(musicStyle == MusicStyle.LYRIC)
        {
            ChoosePreviousMusic(lyricAudioClips);
        }
        if(musicStyle == MusicStyle.CHEERFUL)
        {
            ChoosePreviousMusic(cheerfulAudioClips);
        }
        
    }

    public void OnClickNextMusic()
    {
        if (musicStyle == MusicStyle.LYRIC)
        {
            ChoosePreviousMusic(lyricAudioClips);
        }
        if (musicStyle == MusicStyle.CHEERFUL)
        {
            ChoosePreviousMusic(cheerfulAudioClips);
        }
    }

    public void ChoosePreviousMusic(AudioClip[] audioClips)
    {
        if (playMode == PlayMode.CIRCLE)
        {
            slider.value = 0;
            
        }
        if (playMode == PlayMode.RANDOM)
        {
            currentMusicIndex = UnityEngine.Random.Range(0, audioClips.Length);
            PlayMusic(audioClips, currentMusicIndex);
        }
        if (playMode == PlayMode.STRIGHT)
        {
            currentMusicIndex--;
            if (currentMusicIndex == -1)
            {
                currentMusicIndex =audioClips.Length - 1;
            }
            PlayMusic(audioClips, currentMusicIndex);
        }
    }

    public void ChooseNextMusic(AudioClip[] audioClips)
    {
        if (playMode == PlayMode.CIRCLE)
        {
            slider.value = 0;
            
        }
        if (playMode == PlayMode.RANDOM)
        {
            currentMusicIndex = UnityEngine.Random.Range(0, audioClips.Length);
            PlayMusic(audioClips, currentMusicIndex);
        }
        if (playMode == PlayMode.STRIGHT)
        {
            currentMusicIndex++;
            if (currentMusicIndex == audioClips.Length)
            {
                currentMusicIndex = 0;
            }
            PlayMusic(audioClips, currentMusicIndex);
        }
    }


}
