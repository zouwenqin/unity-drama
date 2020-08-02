using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public CurrentState currentState;
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
        if (GameLoop.Instance.currentState == CurrentState.panel1)
        {
            //Debug.LogError("panel1");
            //检查数据是否全部填写
            if (PanelOne_C.Instance.CheakData())
            {
               
                //Debug.LogError("检查了");
                panel1.SetActive(false);
                GameLoop.Instance.SetCurrentScene(CurrentState.panel2);
                panel2.SetActive(true);
            }
            else
            {
                TipManager.Instance.TipShow("资料没有全部填写完整");
            }
            return;
        }

        //如果当前是第二个panel的状态下
        if (GameLoop.Instance.currentState == CurrentState.panel2)
        {
            
                panel2.SetActive(false);
                GameLoop.Instance.SetCurrentScene(CurrentState.panel3);
                panel3.SetActive(true);
            
           
        }
        //--------------ToDO
    }

    /// <summary>
    /// 点击在返回上面
    /// </summary>
    public void OnClickReturn()
    {
        if (GameLoop.Instance.currentState == CurrentState.panel1)
        {
            SceneManager.LoadScene("mainScene");
            GameLoop.Instance.SetCurrentScene(CurrentState.startscene);

        }
        if (GameLoop.Instance.currentState == CurrentState.panel2)
        {
            GameLoop.Instance.SetCurrentScene(CurrentState.panel1);

            panel2.SetActive(false);
            panel1.SetActive(true);
        }
        if (GameLoop.Instance.currentState == CurrentState.panel3)
        {
            GameLoop.Instance.SetCurrentScene(CurrentState.panel2);

            panel2.SetActive(true);
            panel3.SetActive(false);
        }
    }
}
