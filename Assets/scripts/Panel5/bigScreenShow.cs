using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class bigScreenShow : MonoBehaviour
{
    public Transform videoItemContent;
    public GameObject videoItemPrefab;
    public VideoPlayer videoPlayer;
    public GameObject videoPlayPanel;
    public GameObject panel4;

    private GameObject videoItem;
    private Material outline;
    private List<GameObject> videoItemPool = new List<GameObject>();

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
        HideVideoItem();
    }
    public void InitVideoItem()
    {
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.mp4");
            //FileInfo[] fileInfos1 = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                videoItem = GetVideoItem();
                videoItem.transform.SetParent(videoItemContent);
                videoItem.transform.localScale = Vector3.one;
                videoItem.transform.localPosition = new Vector3(videoItem.transform.localPosition.x, videoItem.transform.localPosition.y, 0);
                videoItem.GetComponent<VideoPath>().videoPath = fileInfos[i].FullName;
                videoItem.GetComponent<Button>().onClick.AddListener(() => {
                    OnClickChooseVideo(videoItem.transform);
                    videoPlayer.url = videoItem.GetComponent<VideoPath>().videoPath;
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
                panel4.SetActive(true);
            });
        }
    }

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
}
