using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOne_C : MonoBehaviour
{
    #region  字段
    
    private string scenarioName;
    private string playerName1;
    private string playerName2;
    /// <summary>
    /// 演员名字三
    /// </summary>
    private string playerName3;
    /// <summary>
    /// 背景图片
    /// </summary>
    private Sprite sceneImage;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite actorImage1;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite actorImage2;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite actorImage3;

    private Material outline;
    #endregion
    public static PanelOne_C Instance;
    public InputField inputField_scenarioName;
    public InputField inputField_player1;
    public InputField inputField_player2;
    public InputField inputField_player3;
    public Dropdown drop;
    private Vector2 currentSize;

    public Transform defaultScene;
    public Transform defaultActor1;
    public Transform defaultActor2;
    void Start()
    {
        Instance = this;
        // panelOne_M = new PanelOne_M();
        outline = Resources.Load<Material>("Shaders/Custom_ImageOutline");
        EditInInputField();
        //drop = this.GetComponent<Dropdown>();
        //currentSize =  new Vector2(GameObject.Find("ScrollViewScenes/Viewport/Content").GetComponent<GridLayoutGroup>().cellSize.x,
        //    GameObject.Find("ScrollViewScenes/Viewport/Content").GetComponent<GridLayoutGroup>().cellSize.y);
        
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
            }
        });
        inputField_player2.onEndEdit.AddListener((str) =>
        {
            if (str != null)
            {
                playerName2 = str;
            }
        });
        inputField_player3.onEndEdit.AddListener((str) =>
        {
            if (str != null)
            {
                playerName3 = str;
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
        if (lastSelectScene == null)
        {
            lastSelectScene = trans;
        }
        if (lastSelectScene != null && lastSelectScene != trans)
        {
            if(lastSelectScene.GetComponent<Image>().material != null)
            {
                lastSelectScene.GetComponent<Image>().material = null;
            }
            lastSelectScene = trans;
        }
        bool hasShow = ShowHideOutLine(trans);
        if (hasShow)
        {
            sceneImage = trans.GetComponent<Image>().sprite;
        }
        else
        {
            sceneImage = null;
        }
    }

    /// <summary>
    /// 演员1
    /// </summary>
    Transform actor1;
    Transform actor2;
    Transform actor3;
    int actorImageIndex = 0;

    /// <summary>
    /// 点击选择人物
    /// </summary>
    /// <param name="trans"></param>
    public void OnClickChooseCharacter(Transform trans)
    {
        bool show = ShowHideOutLine(trans);
        if (show) //点击了无外框的图片
        {
            if (actor1 == null)  //第一个角色为空
            {
                actor1 = trans;
                actorImage1 = trans.GetComponent<Image>().sprite;
                return;
            }
            if (actor2 == null)
            {
                actor2 = trans;               
                actorImage2 = trans.GetComponent<Image>().sprite;
                return;
            }
            if (actor3 == null)
            {
                actor3 = trans;                
                actorImage3 = trans.GetComponent<Image>().sprite;
                return;

            }
        }
        else  //点击已有外框的图片
        {
            if (actor1 == trans)  
            {
                if (actor2 != null && actor3 != null)
                {
                    actor1 = actor2;
                    actorImage1 = actorImage2;
                    actor2 = actor3;
                    actorImage2 = actorImage3;
                    actor3 = null;
                    actorImage3 = null;
                }
                else if(actor2 != null && actor3 == null)
                {
                    actor1 = actor2;
                    actorImage1 = actorImage2;
                    actor2 = null;
                    actorImage2 = null;
                }
                else
                {
                    actor1 = actor3;
                    actorImage1 = actorImage3;
                    actor3 = null;
                    actorImage3 = null;
                }
            }
            else
            if (actor2 == trans)
            {
                if(actor3 != null)
                {
                    actor2 = actor3;
                    actorImage2 = actorImage3;
                    actor3 = null;
                    actorImage3 = null;
                }
                else
                {
                    actor2 = null;
                    actorImage2 = null;
                }
            }
            else
            if (actor3 == trans)
            {
                actor3 = null;
                actorImage3 = null;
            }
        }
    }

    /// <summary>
    /// 检测默认组合是否改变
    /// </summary>
    public void DropDownListener()
    {
        if(lastSelectScene == defaultScene && defaultActor1 == actor1 &&defaultActor2 == actor2)
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
        if (scenarioName == null ||
        sceneImage == null || (actorImage1 == null && actorImage2 == null && actorImage3 == null)
        //图片和名字不一致
        || ((playerName1 == null && actorImage1 != null) || (playerName1 != null && actorImage1 == null))
        || ((playerName2 == null && actorImage2 != null) || (playerName2 != null && actorImage2 == null))
        || ((playerName3 == null && actorImage3 != null) || (playerName3 != null && actorImage3 == null))
        )
        {
            return false;
        }
        else
        {
            DropDownListener();
            GameManager.Instance.SetSceneImage(sceneImage);
            if (actorImage1 != null) GameManager.Instance.SetActorImage(0, actorImage1);
            if (actorImage2 != null) GameManager.Instance.SetActorImage(1, actorImage2);
            if (actorImage3 != null) GameManager.Instance.SetActorImage(2, actorImage3);
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

    public void UpdateSize(Transform trans)
    {
        trans.localScale = new Vector3(currentSize.x * 1.3f,currentSize.y*1.3f, 1);
    }

}
