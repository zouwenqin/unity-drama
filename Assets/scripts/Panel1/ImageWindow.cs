using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ImageType
{
    ACTORIMAGE,
    SCENEIMAGE
}

public class ImageWindow : MonoBehaviour
{
    private GameObject selectPanel;
    public  Sprite[] actorSprites;
    public Sprite[] sceneSprites;
    public Image image;
    public Button chooseButton;
    public Button closeButton;
    public Button previousButton;
    public Button nextButton;
    public Button leftRotateButton;
    public Button rightRoateButton;

    [HideInInspector]
    public ImageType imageType;
    [HideInInspector]
    public int actorImageIndex;
    [HideInInspector]
    public int sceneImageIndex;
    [HideInInspector]
    public int  isChoose = 0;
    [HideInInspector]
    public bool finishChoose = false;
    float angle ;

    
    private void OnEnable ()
    {
        image.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
        angle = 0;
        isChoose = 0;
        finishChoose = false;
        
        if (imageType == ImageType.SCENEIMAGE)
        {
            image.sprite = sceneSprites[sceneImageIndex];

        }
        if (imageType == ImageType.ACTORIMAGE)
        {
            image.sprite = actorSprites[actorImageIndex];

        }
    }
 
    private void Start()
    {
        
        selectPanel = GameObject.Find("OneSelectPanel");
    }

    private void OnDisable()
    {
        if(isChoose == 1)
        {
            selectPanel.GetComponent<SelectPanel>().isChoosed = true;
        }
        else
        {
            selectPanel.GetComponent<SelectPanel>().isChoosed = false;
        }
    }

    /// <summary>
    /// 点击取消选择按钮
    /// </summary>
    public void OnClickCancelButton()
    {       
        isChoose = 2;
        
        finishChoose = true;
        if (imageType == ImageType.SCENEIMAGE)
        {
            selectPanel.GetComponent<SelectPanel>().sceneImageParent.GetChild(sceneImageIndex).GetComponent<Image>().material = null;
        }
        if(imageType == ImageType.ACTORIMAGE)
        {
            if (selectPanel.GetComponent<SelectPanel>().actorImageParent.GetChild(actorImageIndex).GetComponent<Image>().material != null)
            {
                selectPanel.GetComponent<SelectPanel>().actorImageParent.GetChild(actorImageIndex).GetComponent<Image>().material = null;
                selectPanel.GetComponent<SelectPanel>().chooseActorImageCount--;
            }
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击确认选择按钮
    /// </summary>
    public void OnClickConfirmButton()
    {   
        isChoose = 1;
        
        finishChoose = true;
        if (imageType == ImageType.SCENEIMAGE)
        {
            for (int i = 0; i < selectPanel.GetComponent<SelectPanel>().sceneImageParent.childCount; i++)
            {
                if (i == sceneImageIndex)
                {
                    selectPanel.GetComponent<SelectPanel>().sceneImageParent.GetChild(i).GetComponent<Image>().material = selectPanel.GetComponent<SelectPanel>().outline;
                    selectPanel.GetComponent<SelectPanel>().BgScene.GetComponent<Image>().sprite = selectPanel.GetComponent<SelectPanel>().sceneImageParent.GetChild(i).GetComponent<Image>().sprite;
                }
                else
                {
                    selectPanel.GetComponent<SelectPanel>().sceneImageParent.GetChild(i).GetComponent<Image>().material = null;
                }
            }
        }
        if(imageType == ImageType.ACTORIMAGE)
        {
            for (int i = 0; i < selectPanel.GetComponent<SelectPanel>().actorImageParent.childCount; i++)
            {
                if (i == actorImageIndex)
                {
                    selectPanel.GetComponent<SelectPanel>().actorImageParent.GetChild(i).GetComponent<Image>().material = selectPanel.GetComponent<SelectPanel>().outline;
                    selectPanel.GetComponent<SelectPanel>().chooseActorImageCount++;
                }         
            }
        }
        this.gameObject.SetActive(false);
    }

    public void OnClickPreviousButton()
    {
        if(imageType == ImageType.SCENEIMAGE)
        {
            sceneImageIndex--;
            if(sceneImageIndex == -1)
            {
                sceneImageIndex = sceneSprites.Length - 1;
            }
            image.sprite = sceneSprites[sceneImageIndex];
        }
        else if(imageType == ImageType.ACTORIMAGE)
        {
            actorImageIndex--;
            if(actorImageIndex == -1)
            {
                actorImageIndex = actorSprites.Length - 1;
            }
            image.sprite = actorSprites[actorImageIndex];
        }
    }

    public void OnClickNextButton()
    {
        if (imageType == ImageType.SCENEIMAGE)
        {
            sceneImageIndex++;
            if (sceneImageIndex == sceneSprites.Length)
            {
                sceneImageIndex = 0;
            }
            image.sprite = sceneSprites[sceneImageIndex];
        }
        else if (imageType == ImageType.ACTORIMAGE)
        {
            actorImageIndex++;
            if (actorImageIndex == actorSprites.Length)
            {
                actorImageIndex = 0;
            }
            image.sprite = actorSprites[actorImageIndex];
        }
    }

    public void OnClickLeftRotateButton()
    {

        angle += 30f;
        image.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, angle, 0);
    }

    public void OnClickRightRotateButton()
    {
        angle -= 30f;
        image.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, angle, 0);
    }


}
