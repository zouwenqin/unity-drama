using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BigScreenShowPanel : MonoBehaviour
{
    public Transform videoItemContent;
    public GameObject videoItemPrefab;
    
    public GameObject videoPlayPanel;
    public GameObject bigScreenPlay;
    

    //private GameObject videoItem;
    private Material outline;
    [HideInInspector]
    public  List<GameObject> videoItemPool = new List<GameObject>();

    private void Start()
    {
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");
    }
    private void OnEnable()
    {
        InitVideoItem();
        
    }

    private void OnDisable()
    {
        for (int i = videoItemContent.childCount - 1; i >= 0; i--)
        {
            Destroy(videoItemContent.GetChild(i).gameObject);
        }
    }

    public void InitVideoItem()
    {
        //Debug.LogError(VideoPlayerController._instance.videoItemPathList.Count);
        for (int i = 0; i < VideoPlayerController._instance.videoItemList.Count; i++)
        {
            //GameObject videoItem = VideoPlayerController._instance.videoItemList[i];
            GameObject videoItem = VideoPlayerController._instance.videoItemList[i];
            videoItem.transform.SetParent(videoItemContent, false);
            videoItem.transform.localScale = Vector3.one;
            //videoItem.transform.GetComponent<VideoItem>().videoPath = VideoPlayerController._instance.videoItemList[i].GetComponent<VideoItem>().videoPath;
            videoItem.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            videoItem.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                
                OnClickChooseVideo(videoItem.transform);
                if (videoItem.GetComponent<Image>().material != null)
                {
                    VideoPlayerController._instance.videoPlayer.url = videoItem.transform.GetComponent<VideoItem>().videoPath;
                }
            });
        }
    }

    public void InitVideoItem1()
    {
        //HideVideoItem();
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.mp4");
            //FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {


                //GameObject videoItem = GetVideoItem();
                GameObject videoItem = GameObject.Instantiate<GameObject>(videoItemPrefab,videoItemContent,false);
               
                    
                    videoItem.transform.localScale = Vector3.one;
                    //videoItem.transform.localPosition = new Vector3(videoItem.transform.localPosition.x, videoItem.transform.localPosition.y, 0);
                    videoItem.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItem.transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //Debug.LogError("fullScreen" + videoItemPool.Count);
                        OnClickChooseVideo(videoItem.transform);
                        if (videoItem.GetComponent<Image>().material != null)
                        {
                            VideoPlayerController._instance.videoPlayer.url = videoItem.transform.GetComponent<VideoItem>().videoPath;
                        }
                    });
                }
            
        }
    }

    Transform previousSelectVideo;
    public void OnClickChooseVideo(Transform trans)
    {
        if(previousSelectVideo == null)
        {
            previousSelectVideo = trans;
        }
        if (previousSelectVideo != null && previousSelectVideo != trans)
        {
            if (previousSelectVideo.GetComponent<Image>().material != null)
            {
                previousSelectVideo.GetComponent<Image>().material = null;
            }
            previousSelectVideo = trans;
        }
        bool hasShow = ShowOutline(trans);
        if (hasShow)
        {
            videoPlayPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                bigScreenPlay.SetActive(true);
            });
        }
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

    /// <summary>
    /// 从对象池中拿对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetVideoItem()
    {
        for (int i = 0; i < videoItemPool.Count; i++)
        {
            if(videoItemPool[i].activeSelf == false)
            {
                videoItemPool[i].SetActive(true);
                return videoItemPool[i];
            }
        }
        GameObject obj = GameObject.Instantiate(videoItemPrefab);
        videoItemPool.Add(obj);
        return obj;
    }

    public void HideVideoItem()
    {
        for (int i = 0; i < videoItemPool.Count; i++)
        {
            videoItemPool[i].SetActive(false);
        }
    }

    public void ShowBigScreen()
    {
        
        
    }
    
}
