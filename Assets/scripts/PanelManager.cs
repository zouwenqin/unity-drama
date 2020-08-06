using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentPanel
{
    /// <summary>
    /// 开始场景
    /// </summary>
    startscene,
    /// <summary>
    /// 第一个panel
    /// </summary>
    panel1,
    /// <summary>
    /// 第二个panel
    /// </summary>
    panel2,
    /// <summary>
    /// 第三个panel
    /// </summary>
    panel3,
    /// <summary>
    /// 大屏播放界面
    /// </summary>
    panel4

}

public class PanelManager : MonoBehaviour
{
    public static PanelManager _instance;
    /// <summary>
    /// 当前的状态
    /// </summary>
    [HideInInspector]
    public CurrentPanel currentState;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;


    private void Awake()
    {
        _instance = this;  
    }

    private void Start()
    {
        currentState = CurrentPanel.panel1;
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }

    /// <summary>
    /// 设置当前的场景状态
    /// </summary>
    /// <param name="_currentState"></param>
    public void SetCurrentScene(CurrentPanel _currentState)
    {
       
        currentState = _currentState;
    }

    

    /// <summary>
    /// 点击按钮下一步
    /// </summary>
    public void OnClickNext()
    {


        //如果当前是第一个panel的状态下
        if (PanelManager._instance.currentState == CurrentPanel.panel1)
        {

            //检查数据是否全部填写
            if (PanelOne_C.Instance.CheakData())
            {
                panel1.SetActive(false);
                SetCurrentScene(CurrentPanel.panel2);
                panel2.SetActive(true);
            }
            else
            {
                TipManager.Instance.TipShow("资料没有全部填写完整");
            }
            return;
        }

        //如果当前是第二个panel的状态下
        if (PanelManager._instance.currentState == CurrentPanel.panel2)
        {

            panel2.SetActive(false);
            SetCurrentScene(CurrentPanel.panel3);
            panel3.SetActive(true);


        }

    }

    /// <summary>
    /// 点击设置按钮
    /// </summary>
    public void OnClickSetButton()
    {
        panel4.SetActive(true);
    }

    /// <summary>
    /// 点击在返回上一个界面
    /// </summary>
    public void OnClickReturnButton()
    {
        if (PanelManager._instance.currentState == CurrentPanel.panel1)
        {
            SceneManager.LoadScene("mainScene");
            SetCurrentScene(CurrentPanel.startscene);

        }
        if (PanelManager._instance.currentState == CurrentPanel.panel2)
        {
            SetCurrentScene(CurrentPanel.panel1);

            panel2.SetActive(false);
            panel1.SetActive(true);
        }
        if (PanelManager._instance.currentState == CurrentPanel.panel3)
        {
            SetCurrentScene(CurrentPanel.panel2);

            panel2.SetActive(true);
            panel3.SetActive(false);
        }
    }

    public void OnClickChooseScene()
    {
        SceneManager.LoadScene("selectScene2");
    }



}
