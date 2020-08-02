using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOne_M
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
    #endregion

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

    /// <summary>
    /// 检查数据是否全部填写
    /// </summary>
    public bool CheakData()
    {
        if (scenarioName == "" ||
        playerName1 == "" ||
        playerName2 == "" ||
        playerName3 == "" ||
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
}
