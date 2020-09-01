using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    #region  字段
    [HideInInspector]
    public string scenarioName;
    private string playerName1;
    private string playerName2;
    private string playerName3;
    
    private Sprite sceneImage;
    public Sprite actorImage1;
    public Sprite actorImage2;
    public Sprite actorImage3;
    public Sprite[] actorImage =new Sprite[3] { null, null, null };

    [HideInInspector]
    public Material outline;
    #endregion
    
    public  InputField inputField_scenarioName;
    public InputField inputField_player1;
    public InputField inputField_player2;
    public InputField inputField_player3;
    public Dropdown drop;
    private Transform imageWindow;

    public Transform sceneImageParent;
    public Transform actorImageParent;

    private Sprite defaultScene;
    private Sprite defaultActorImage1;
    private Sprite defaultActorImage2;

    /// <summary>
    /// 演员1
    /// </summary>
    Transform actor1;
    Transform actor2;
    Transform actor3;

    [HideInInspector]
    public bool isChoosed = false;
    private int inputActorNameCount = 0;
    [HideInInspector]
    public int chooseActorImageCount = 0;

    void Start()
    {     
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");
        EditInInputField();
        imageWindow = transform.Find("ImageWindow");
    }

    #region 方法

    /// <summary>
    /// 输入框输入
    /// </summary>
    public void EditInInputField()
    {
        inputField_scenarioName.onEndEdit.AddListener((string str) =>
        {
            scenarioName = str;
            GameManager.Instance.SetScenarioName(str);
            //Debug.LogError(GameManager.Instance.GetDramaName());
        });
        inputField_player1.onEndEdit.AddListener((str) =>
        {
            if (str != null)
            {
                playerName1 = str;
                GameManager.Instance.SetActorName(0, str);
                inputActorNameCount++;
            }
        });
        inputField_player2.onEndEdit.AddListener((str) =>
        {
            if (str != null)
            {
                playerName2 = str;
                GameManager.Instance.SetActorName(1, str);
                inputActorNameCount++;
            }
        });
        inputField_player3.onEndEdit.AddListener((str) =>
        {
            if (str != null)
            {
                playerName3 = str;
                GameManager.Instance.SetActorName(2, str);
                inputActorNameCount++;
            }            
        });
    }

    
    /// <summary>
    /// 上一个选择的场景图片
    /// </summary>
    private Transform lastSelectScene;
    /// <summary>
    /// 选择屏幕背景图
    /// </summary>
    /// <param name="trans"></param>  
    public void OnClickChooseSecene(Transform trans)
    {
        imageWindow.GetComponent<ImageWindow>().sceneImageIndex = trans.GetSiblingIndex();
        imageWindow.GetComponent<ImageWindow>().imageType = ImageType.SCENEIMAGE;
        imageWindow.gameObject.SetActive(true);
    }    

    /// <summary>
    /// 点击选择人物
    /// </summary>
    /// <param name="trans"></param>
    public void OnClickChooseCharacter(Transform trans)
    {
        imageWindow.GetComponent<ImageWindow>().actorImageIndex = trans.GetSiblingIndex();
        imageWindow.GetComponent<ImageWindow>().imageType = ImageType.ACTORIMAGE;
        imageWindow.gameObject.SetActive(true);
    }

    /// <summary>
    /// 检测默认组合是否改变
    /// </summary>
    public void DropDownListener()
    {
        if(sceneImage == defaultScene && defaultActorImage1 == actorImage1 &&defaultActorImage2 == actorImage2)
        {
            drop.captionText.text = drop.options[0].text;
        }
        else
        {
            drop.captionText.text = drop.options[2].text;
        }
    }

    /// <summary>
    ///检查剧本名称，场景，演员等是否已经选择
    /// </summary>
    /// <returns></returns>
    public bool CheakData()
    {
        for (int i = 0; i < sceneImageParent.childCount; i++)
        {
            if(sceneImageParent.GetChild(i).GetComponent<Image>().material != null)
            {
                sceneImage = sceneImageParent.GetChild(i).GetComponent<Image>().sprite;
                GameManager.Instance.SetSceneImage(sceneImage);
            }
        }

        if(chooseActorImageCount != inputActorNameCount)
        {
            return false;
        }
        else
        {
            
            for (int i = 0; i < actorImageParent.childCount; i++)
            {
                if(actorImageParent.GetChild(i).GetComponent<Image>().material == outline)
                {
                    actorImage[i] = actorImageParent.GetChild(i).GetComponent<Image>().sprite;
                    GameManager.Instance.SetActorImage(i, actorImageParent.GetChild(i).GetComponent<Image>().sprite);
                }
            }
        }
       
        if(scenarioName == null || sceneImage == null)
        {
            return false;
        }
        else
        {
            DropDownListener();
            return true;
        }
    }

    /// <summary>
    /// 无外框的点击显示外框，返回true，已有外框的点击禁用外框，返回false
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    bool ShowHideOutLine(Transform trans)
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

    #endregion

    public void ClearData()
    {
        scenarioName = "";
        playerName1 = "";
        playerName2 = "";
        playerName3 = "";
        sceneImage = null;
        actorImage1 = null;
        actorImage2 = null;
        actorImage3 = null;
    }


}
