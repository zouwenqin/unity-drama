using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOne_C : MonoBehaviour
{
    #region  字段
    /// <summary>
    /// 剧本名称
    /// </summary>
    public string scenarioName;
    /// <summary>
    /// 演员名字一
    /// </summary>
    public string playerName1;
    /// <summary>
    /// 演员名字二
    /// </summary>
    public string playerName2;
    /// <summary>
    /// 演员名字三
    /// </summary>
    public string playerName3;
    /// <summary>
    /// 背景图片
    /// </summary>
    public Sprite BgTex;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite CharacterTex1;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite CharacterTex2;
    /// <summary>
    /// 演员图片
    /// </summary>
    public Sprite CharacterTex3;

    private Material outline;
    #endregion
    public static PanelOne_C Instance;
   //  PanelOne_M panelOne_M;
   // public PanelOne_M panelOneM => panelOne_M;
    /// <summary>
    /// 剧本名称
    /// </summary>
    public InputField inputField_scenarioName;
    public InputField inputField_player1;
    public InputField inputField_player2;
    public InputField inputField_player3;
    public Dropdown drop;

    private Vector2 currentSize;
    //private Vector2 targetSize;
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

    //输入框输入数据

    #region 方法
    public void EditInInputField()
    {
        inputField_scenarioName.onEndEdit.AddListener((string str) =>
        {
            //panelOne_M.scenarioName = str;
            scenarioName = str;
        });
        inputField_player1.onEndEdit.AddListener((str) =>
        {
            //panelOne_M.playerName1 = str;
            playerName1 = str;
        });
        inputField_player2.onEndEdit.AddListener((str) =>
        {
            // panelOne_M.playerName2 = str;
            playerName2 = str;
        });
        inputField_player3.onEndEdit.AddListener((str) =>
        {
            //panelOne_M.playerName3 = str;
            playerName3 = str;
        });
    }

    /// <summary>
    /// 上一个选择的场景图片
    /// </summary>
    Transform LastSelectTransScene;
    /// <summary>
    /// 选择屏幕背景图
    /// </summary>
    /// <param name="trans"></param>
    
    //点击选择场景图片
    public void OnClickChooseSecene(Transform trans)
    {
        if (LastSelectTransScene == null)
        {
            LastSelectTransScene = trans;
        }
        if (LastSelectTransScene != null && LastSelectTransScene != trans)
        {
            //if (LastSelectTransScene.GetComponent<Outline>().enabled)
            //{
            //    LastSelectTransScene.GetComponent<Outline>().enabled = false;
            //}
            if(LastSelectTransScene.GetComponent<Image>().material != null)
            {
                LastSelectTransScene.GetComponent<Image>().material = null;
            }
            LastSelectTransScene = trans;
        }
        bool hasShow = ShowHideOutLine(trans);
        if (hasShow)
        {
            //panelOne_M.BgTex = trans.GetComponent<Image>().sprite;
            BgTex = trans.GetComponent<Image>().sprite;
        }
        else
        {
            // panelOne_M.BgTex = null;
            BgTex = null;
        }
    }

    /// <summary>
    /// 角色1
    /// </summary>
    Transform character1;
    /// <summary>
    /// 角色2
    /// </summary>
    Transform character2;
    /// <summary>
    /// 角色3
    /// </summary>
    Transform character3;

    //点击选择人物
    public void OnClickChooseCharacter(Transform trans)
    {
        //UpdateSize(trans);
        bool showhide = ShowHideOutLine(trans);
        if (showhide)
        {
            if (character1 == null)
            {
                character1 = trans;
                //panelOne_M.CharacterTex1 = trans.GetComponent<Image>().sprite;
                CharacterTex1 = trans.GetComponent<Image>().sprite;
                return;
            }
            if (character2 == null)
            {
                character2 = trans;
                //panelOne_M.CharacterTex2 = trans.GetComponent<Image>().sprite;
                CharacterTex2 = trans.GetComponent<Image>().sprite;

                return;
            }
            if (character3 == null)
            {
                character3 = trans;
                //panelOne_M.CharacterTex3 = trans.GetComponent<Image>().sprite;
                CharacterTex3 = trans.GetComponent<Image>().sprite;
                return;

            }
            //如果三个都被选中了
            if (character1 != null && character2 != null && character3 != null)
            {
                // character1.GetComponent<Outline>().enabled = false;
                character1.GetComponent<Image>().material = null;
                character1 = trans;
                //panelOne_M.CharacterTex1 = trans.GetComponent<Image>().sprite;
                CharacterTex1 = trans.GetComponent<Image>().sprite;

            }
        }
        else
        {
            if (character1 == trans)
            {
                character1 = null;
            }
            else
            if (character2 == trans)
            {
                character2 = null;
            }
            else
            if (character3 == trans)
            {
                character3 = null;
            }
        }
    }

    //显示图片外框线
    bool ShowHideOutLine(Transform trans)
    {
        //if (trans.GetComponent<Outline>().enabled == false)
        //{
        //    trans.GetComponent<Outline>().enabled = true;
        //    return true;
        //}
        //else
        //{
        //    trans.GetComponent<Outline>().enabled = false;
        //    return false;
        //}
        
        if (trans.GetComponent<Image>().material != outline )
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

    public void ClearData()
    {
        scenarioName = "";
        playerName1 = "";
        playerName2 = "";
        playerName3 = "";
        BgTex = null;
        CharacterTex1 = null;
        CharacterTex2 = null;
        CharacterTex3 = null;
    }

    public bool CheakData()
    {
        if (character1 != null && character2 != null && character3 == null)
        {
            //PanelOne_C.Instance.drop.captionText.text  = PanelOne_C.Instance.drop.options[0].text;
            drop.captionText.text = drop.options[0].text;
        }
        else
        {
            drop.captionText.text = drop.options[2].text;
        }

        if (scenarioName == "" ||
        //playerName1 == "" ||
        //playerName2 == "" ||
        //playerName3 == "" ||
        BgTex == null || (CharacterTex1 == null && CharacterTex2 == null && CharacterTex3 == null)
        //图片和名字不一致
        || ((playerName1 == "" && CharacterTex1 != null) || (playerName1 != "" && CharacterTex1 == null))
        || ((playerName2 == "" && CharacterTex2 != null) || (playerName2 != "" && CharacterTex2 == null))
        || ((playerName3 == "" && CharacterTex3 != null) || (playerName3 != "" && CharacterTex3 == null))
        )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    
    public void UpdateSize(Transform trans)
    {
        trans.localScale = new Vector3(currentSize.x * 1.3f,currentSize.y*1.3f, 1);
    }

}
