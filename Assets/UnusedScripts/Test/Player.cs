using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum playMode
    {
        stright,
        circle,
        random
    }
    private playMode playM = playMode.stright;
    private GameObject pModeImage; //播放模式的图片切换

    public Text musicName;
    public Text nowTime;
    public Text allTime;
    public Slider slid;
    public Transform buttons; //按钮所在的集合体
    public AudioClip[] auc;
    public AudioSource aus;
    private int index;        //数组auc的索引

    private int currentHour, currentMinute, currentSecond;
    private int clipHour, clipMinute, clipSecond;

    public GameObject item; //要实例化的列表按钮对象，button预制体
    private GameObject it; //实例化出的列表音频按钮

    private GameObject voice_butt, voice_text, voice_slid;
    int clickNum = 0, value;

    private void Start()
    {
        aus.Stop();

        slid.onValueChanged.AddListener(delegate
        {
            if (slid.value == 1)
                return;          //加上之后，避免拖动进度条到最后不松手报错
            aus.time = slid.value * aus.clip.length;//给进度条添加事件监听，每当拖动进度条，歌曲从相应位置播放
        });

        foreach(Transform go in buttons)
        {
            go.GetComponent<Button>().onClick.AddListener(delegate //根据按钮名给按钮添加事件监听
            {
                switch (go.name)
                {
                    case "lastM":
                        LastMusic();
                        break;
                    case "pause":
                        Pause();
                        break;
                    case "play":
                        Play();
                        break;
                    case "nextM":
                        NextMusic();
                        break; 
                }
            });

        }
        InitPlayMode();
        InitItem();
        initVoice();
    }

    private void Update()
    {
        NowTime();
        AllTime();
        NowMusic();
    }

    public void initVoice1()
    {
        voice_butt = GameObject.Find("VoiceTeam/btn_voice");
        voice_text = GameObject.Find("VoiceTeam/Slider/txt_voice");
        voice_slid = GameObject.Find("VoiceTeam/Slider");

        voice_slid.GetComponent<Slider>().value = 20;
        //开始调用一次，设置初始音量大小
        voice();

        voice_slid.GetComponent<Slider>().onValueChanged.AddListener(delegate { voice(); });

        voice_butt.GetComponent<Button>().onClick.AddListener(delegate
        {
            clickNum++; //点击按钮的次数
            if (clickNum == 1)  //1为打开静音，0 为关闭静音
            {
                aus.mute = true;
                voice_butt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/voiceMute");
                //存下点击静音时进度条的值
                value = (int)voice_slid.GetComponent<Slider>().value;
                //进度条的value值为0
                voice_slid.GetComponent<Slider>().value = 0;
            }
            else
            {
                aus.mute = false;

                voice_butt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/voice");

                voice_slid.GetComponent<Slider>().value = value;

                clickNum = 0;
            }
        });
    }

    bool isShowVoiceSlider = false;
    public void initVoice()
    {
        voice_butt = GameObject.Find("VoiceTeam/btn_voice");
       // voice_text = GameObject.Find("VoiceTeam/Slider/txt_voice");
        voice_slid = GameObject.Find("VoiceTeam/Slider");
        voice_slid.SetActive(false);
        voice_slid.GetComponent<Slider>().value = 20;
        //开始调用一次，设置初始音量大小
        //voice();

        voice_slid.GetComponent<Slider>().onValueChanged.AddListener( a => {
            //voice(); 
            //aus.volume = voice_slid.GetComponent<Slider>().value * 0.01f;
            aus.volume = a * 0.01f;
            //Debug.LogError(a);
        });

        voice_butt.GetComponent<Button>().onClick.AddListener(() =>
        {
            isShowVoiceSlider = !isShowVoiceSlider;
            if (isShowVoiceSlider) 
            {
                voice_slid.SetActive(true);
                //aus.mute = true;
                //voice_butt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/voiceMute");
                //存下点击静音时进度条的值
                //value = (int)voice_slid.GetComponent<Slider>().value;
                //进度条的value值为0
                //voice_slid.GetComponent<Slider>().value = 0;
            }
            else
            {
                voice_slid.SetActive(false);
            }
   
        });
    }

    public void voice()
    {
        //把进度条最大值改为100，最小值为0
        //音频播放器的音量volume = 进度条的值* 0.01f；(因为volume的范围为0~1）
        aus.volume = voice_slid.GetComponent<Slider>().value * 0.01f;
        //音量的文本显示（为进度条的value值，value值时浮点型，这里显示成整型）
       // voice_text.GetComponent<Text>().text = ((int)voice_slid.GetComponent<Slider>().value ).ToString();

    }
    public void InitPlayMode()
    {
        //找到名为pModeImage的按钮
        pModeImage = GameObject.Find("pModeImage");
        //给它添加监听（调用Play Mode方法，来切换播放模式）
        pModeImage.GetComponent<Button>().onClick.AddListener(PlayMode);
    }

    public void InitItem()
    {
        List<GameObject> L = new List<GameObject>(); //列表L

        for (int i = 0; i < auc.Length; i++)
        {
            it = Instantiate(item); //实例化音频列表的各个按钮
            //设置父节点为SCrollview下的Content
            it.transform.SetParent(GameObject.Find("Content").transform, false);

            it.GetComponentInChildren<Text>().text = auc[i].name;
            L.Add(it);
        }
        GameObject[] g = new GameObject[L.Count]; //数组g，长度等于列表L的长度
        L.CopyTo(g);  //把列表L的内容复制到数组g

        foreach (GameObject gob in g)
        {
            gob.GetComponent<Button>().onClick.AddListener(delegate
            {
                //获取索引要从数组里获取，无法从list里获取，所以要把列表里的内容复制到数组g
                index = Array.IndexOf(g, gob);		//获取当前点击的按钮在数组g中的索引
                PlayMusic(index);                   //播放音频
            });
        }
    }

    public void NowTime()
    {
        currentHour = (int)aus.time / 3600;
        currentMinute = (int)(aus.time - currentHour * 3600) / 60;
        currentSecond = (int)(aus.time - currentHour * 3600 - currentMinute * 60);

        nowTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);

        //进度条变化
        slid.value = aus.time / aus.clip.length;
    }

    public void AllTime()
    {
        clipHour = (int)aus.clip.length / 3600;
        clipMinute = (int)(aus.clip.length - clipHour * 3600) /60;
        clipSecond = (int)(aus.clip.length - clipHour * 3600 - clipMinute * 60);

        allTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", clipHour, clipMinute, clipSecond);
    }

    public void LastMusic()
    {
        if(playM == playMode.random)
        {
            randomRange();
        }
        else
        {
            index--;
            //若索引等于-1 ，即当前播放的音频是auc数组列表的第一首，则上一首应为auc数组列表的最后一首
            if (index == -1)
            {
                index = auc.Length - 1;
            }
            PlayMusic(index);   
        } 
    }

    public void randomRange()
    {
        int r = UnityEngine.Random.Range(0, auc.Length);//随机数包含0 ，不包含auc.Length;

        if (index == r) //如果随机数等于当前播放音乐的索引（避免播放同一首）
            r++; //索引加1
        //如果索引等于auc音频数组的长度，即当前播放的音频是最后一首，则下一首应为aus数组列表的第一首
        if (r == auc.Length)
            r = 0;
        PlayMusic(r);// 播放音频
    }

    public void Pause()
    {
        aus.Pause();
    }

    public void Play()
    {
        aus.Play();
    }

    private void NextMusic()
    {
        if (playM == playMode.random)
            randomRange();
        else
        {
            index++;

            if (index == auc.Length)
            {
                index = 0;
            }
            PlayMusic(index);
        }
        //aus.clip = auc[index];
        //slid.value = 0;
        //aus.Play();
    }

    public void NowMusic()
    {
        AudioClip clip = aus.clip;
        string n = aus.clip.name;
        //string[] na = n.Split('-');//以"-"为分割点，把音频名分成若干部分

        //显示当前正在播放的歌曲名【歌曲名（默认字体，25号）+歌手名（默认字体，18号，红色）的形式】
        //musicName.text = string.Format("<size = 25>{0}</size>" + "\n<size=18><color=#FF0000FF>{1}</color></size>", na[0], na[1]);
        musicName.text = string.Format("<size=25>{0}</size>", n);

        //index = Array.IndexOf<AudioClip>(auc, clip);
        index = Array.IndexOf(auc, clip);//当前播放的音频在auc数组中的索引
        Slider();
    }

    //当进度条走到最后时，播放下一曲
    public void Slider()
    {
        if(currentHour == clipHour && currentMinute == clipMinute && currentSecond == clipSecond)
        {
           if(playM == playMode.circle)
            {
                PlayMusic(index);
            }
            else
            {
                NextMusic();
            }
        }
    }

    public void PlayMusic(int index)
    {
        aus.clip = auc[index]; //音频播放器的音频为aus数组中索引对应的音乐

        slid.value = 0;
        aus.Play();

    }

    int t = 0;
    public void PlayMode()
    {
        t++;
        if (t == 1)
        {
            //播放模式为顺序播放
            playM = playMode.stright;
            //加载顺序播放对应的图片
            pModeImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/stright");
        }
        if (t == 2)
        {
            playM = playMode.circle;
            pModeImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/circle");
        }
        if (t == 3)
        {
            playM = playMode.random;
            pModeImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/random");
            //初始化t
            t = 0;
        }

    }

}
