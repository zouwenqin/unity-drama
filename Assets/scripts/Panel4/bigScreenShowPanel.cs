using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BigScreenShowPanel : MonoBehaviour
{
    public Transform videoItemParent;
    public GameObject videoItemPrefab;
    
    public GameObject videoPlayPanel;
    public GameObject bigScreenPlayWindow;

    DirectoryInfo directoryInfo;
    public FileInfo[] fileInfos;

    public InputField startSearchTimeBar;
    public InputField endSearchTimeBar;
    public InputField searchBar;
    public Button SearchButton;
    private string searchBarInput;
    //private GameObject videoItem;
    private Material outline;

    private bool findedRecord = false;

    private void Awake()
    {
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");
        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
    }

    private void Start()
    {
        
        searchBar.onEndEdit.AddListener((string str) =>
        {
            searchBarInput = str;
        });
        SearchButton.onClick.AddListener(OnClickSearchButton);
        startSearchTimeBar.onEndEdit.AddListener(str =>
        {
            
            //startTime = startSearchTimeBar.text;
        });
        endSearchTimeBar.onEndEdit.AddListener(str =>
        {
           
            // endTime = endSearchTimeBar.text;
        });
    }

    private void OnEnable()
    {
        Invoke("CreateVideoItem", 0.1f);
        //CreateVideoItem();
        SearchDateShow();
    }
   
    public void CreateVideoItem()
    {
        if (VideoPlayerController._instance.videoItemList != null)
        {
            VideoPlayerController._instance.videoItemList.Clear();
        }
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
                    GameObject videoItemObj = GameObject.Instantiate<GameObject>(videoItemPrefab, videoItemParent, false);
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
                    videoItemObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                    videoItemObj.transform.localScale = Vector3.one;
                    if (VideoPlayerController._instance.videoItemImage.ContainsKey(fileInfos[i].FullName))
                    {
                        videoItemObj.GetComponent<Image>().sprite = VideoPlayerController._instance.videoItemImage[fileInfos[i].FullName];
                    }
                    videoItemObj.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItemObj.transform.GetComponent<VideoItem>().recordScenarioName = PlayerPrefs.GetString(fileInfos[i].CreationTime.ToString());
                    videoItemObj.transform.GetComponent<VideoItem>().recordTime = fileInfos[i].CreationTime;
                    videoItemObj.transform.GetComponent<VideoItem>().convertDatetime = fileInfos[i].CreationTime.ToString("yyyy-MM-dd");
                    VideoPlayerController._instance.videoItemList.Add(videoItemObj);
                    videoItemObj.transform.GetComponent<Button>().onClick.RemoveAllListeners();
                    int index = i;
                    videoItemObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //ZoomInVideoItem(index);
                        OnClickChooseVideo(index);
                        if (videoItemObj.GetComponent<Image>().material == outline)
                        {
                            VideoPlayerController._instance.videoPlayer.url = videoItemObj.transform.GetComponent<VideoItem>().videoPath;
                        }                       
                    });
                }
            }

        }
    }

    //private void ZoomInVideoItem(int index)
    //{
    //    videoItemParent.GetChild(index).GetComponent<LayoutElement>().ignoreLayout = true;
    //    videoItemParent.GetChild(index).gameObject.SetActive(false);
       
    //    zoomInVideoItem.transform.SetParent(videoItemParent, false);
    //    zoomInVideoItem.transform.localScale = Vector3.one;
    //    zoomInVideoItem.transform.SetSiblingIndex(index);
        
    //}

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

    public void ShowBigScreen()
    {
        
        
    }

    /// <summary>
    /// 点击选择历史录制的视频
    /// </summary>
    /// <param name="index"></param>
    public void OnClickChooseVideo(int index)
    {
        for (int i = 0; i < videoItemParent.childCount; i++)
        {
            if (i == index)
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = outline;
                videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    bigScreenPlayWindow.SetActive(true);
                });
            }
            else
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = null;
            }
        }
    }

    public int GetVideoItemIndex()
    {

        //int childCount = videoItemParent.childCount - 1;  //TODO 改了不要紧?
        int childCount = videoItemParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            if (videoItemParent.GetChild(i).GetComponent<Image>().material == outline)
            {
                return i;
            }
        }
        return 0;
    }

    string startTime;
    string endTime;
    /// <summary>
    /// 历史搜索栏的默认信息显示
    /// </summary>
    public void SearchDateShow()
    {
        endSearchTimeBar.text = string.Format("{0}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth((int)DateTime.Now.Year, (int)DateTime.Now.Month));
        startSearchTimeBar.text = string.Format("{0}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, "01");
        searchBar.placeholder.GetComponent<Text>().text = "搜索剧名";
    }

    public void OnClickSearchButton()
    {
        findedRecord = false;
        startTime = startSearchTimeBar.text;
        endTime = endSearchTimeBar.text;
        for (int i = 0; i < videoItemParent.childCount; i++)
        {         
            if (videoItemParent.GetChild(i).GetComponent<VideoItem>().recordScenarioName == searchBarInput
                && DateTime.Compare(videoItemParent.GetChild(i).GetComponent<VideoItem>().recordTime, Convert.ToDateTime(startTime)) >= 0
                && DateTime.Compare(videoItemParent.GetChild(i).GetComponent<VideoItem>().recordTime, Convert.ToDateTime(endTime)) <= 0
                )
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = outline;
                videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    bigScreenPlayWindow.SetActive(true);
                });
                findedRecord = true;
                break;
            }
            else
            {
                videoItemParent.GetChild(i).GetComponent<Image>().material = null;
            }
        }
        
        if(findedRecord == false)
        {
            TipManager.Instance.TipShow("在相应的时间范围内没有找到对应的记录");
        }
    }
}
