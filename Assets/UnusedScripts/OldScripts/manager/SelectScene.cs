using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel5;
    public CurrentPanel currentState;
    private void Start()
    {
        panel2.SetActive(false);
        panel3.SetActive(false);
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
               
                //Debug.LogError("检查了");
                panel1.SetActive(false);
                PanelManager._instance.SetCurrentScene(CurrentPanel.panel2);
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
                PanelManager._instance.SetCurrentScene(CurrentPanel.panel3);
                panel3.SetActive(true);
            
           
        }
        
    }

    /// <summary>
    /// 点击设置按钮
    /// </summary>
    public void OnClickSetButton()
    {
        panel5.SetActive(true);
    }

    /// <summary>
    /// 点击在返回上一个界面
    /// </summary>
    public void OnClickReturnButton()
    {
        if (PanelManager._instance.currentState == CurrentPanel.panel1)
        {
            SceneManager.LoadScene("mainScene");
            PanelManager._instance.SetCurrentScene(CurrentPanel.startscene);

        }
        if (PanelManager._instance.currentState == CurrentPanel.panel2)
        {
            PanelManager._instance.SetCurrentScene(CurrentPanel.panel1);

            panel2.SetActive(false);
            panel1.SetActive(true);
        }
        if (PanelManager._instance.currentState == CurrentPanel.panel3)
        {
            PanelManager._instance.SetCurrentScene(CurrentPanel.panel2);

            panel2.SetActive(true);
            panel3.SetActive(false);
        }
    }
}
