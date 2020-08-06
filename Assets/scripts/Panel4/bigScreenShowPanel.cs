using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class bigScreenShowPanel : MonoBehaviour
{
    public Transform videoItemContent;
    public GameObject videoItemPrefab;
    
    public GameObject videoPlayPanel;
    public GameObject bigScreenPlay;
    

    //private GameObject videoItem;
    private Material outline;
    private List<GameObject> videoItemPool = new List<GameObject>();

    private void Start()
    {
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");
    }
    private void OnEnable()
    {
        InitVideoItem();
        //CreateVideoItem();
    }

    private void OnDisable()
    {
        HideVideoItem();
    }

    public void InitVideoItem()
    {
       
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.mp4");
            //FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {


                GameObject videoItem = GetVideoItem();
                //GameObject videoItem = GameObject.Instantiate<GameObject>(videoItemPrefab);
                if (videoItem.transform.parent != videoItemContent)
                {
                    videoItem.transform.SetParent(videoItemContent);
                    videoItem.transform.localScale = Vector3.one;
                    videoItem.transform.localPosition = new Vector3(videoItem.transform.localPosition.x, videoItem.transform.localPosition.y, 0);
                    videoItem.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItem.transform.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnClickChooseVideo(videoItem.transform);
                        VideoPlayerController._instance.videoPlayer.url = videoItem.transform.GetComponent<VideoItem>().videoPath;
                    });
                }
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

    public void CreateVideoItem()
    {

        for (int i = 0; i < videoItemContent.childCount; i++)
        {
            GameObject.Destroy(videoItemContent.GetChild(i).gameObject);

        }
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".meta")) continue;
                if (fileInfos[i].Name.EndsWith(".mp4"))
                {
                    GameObject videoItemObj = GameObject.Instantiate<GameObject>(videoItemPrefab);
                    videoItemObj.transform.SetParent(videoItemContent);
                    videoItemObj.transform.localScale = Vector3.one;
                    videoItemObj.transform.localPosition = new Vector3(videoItemObj.transform.localPosition.x, videoItemObj.transform.localPosition.y, 0);
                    videoItemObj.transform.GetComponent<VideoItem>().videoPath = fileInfos[i].FullName;
                    videoItemObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnClickChooseVideo(videoItemObj.transform);

                        VideoPlayerController._instance.videoPlayer.url = videoItemObj.transform.GetComponent<VideoItem>().videoPath;

                    });
                    //Debug.Log( "FullName:" + fileInfos[i].FullName );  
                    //Debug.Log( "DirectoryName:" + fileInfos[i].DirectoryName );  
                }
            }
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
    
}
