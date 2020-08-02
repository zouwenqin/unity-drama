using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CurrentState
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

public class GameLoop : MonoBehaviour
{
    public static GameLoop Instance;
    /// <summary>
    /// 当前的状态
    /// </summary>
    [HideInInspector]
    public CurrentState currentState;
    /// <summary>
    /// 设置当前的场景状态
    /// </summary>
    /// <param name="_currentState"></param>
    public void SetCurrentScene(CurrentState _currentState)
    {
       
        currentState = _currentState;
    }

    private void Awake()
    {
        //Debug.LogError("初始化了");
        Instance = this;
        currentState = CurrentState.panel1;
        //DontDestroyOnLoad(this.gameObject);
    }



}
