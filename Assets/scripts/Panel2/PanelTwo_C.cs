using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 边框风格
/// </summary>
public enum BorderStyle
{
    /// <summary>
    /// 复古
    /// </summary>
    vintage,
    /// <summary>
    /// 彩色
    /// </summary>
    colorized,
    /// <summary>
    /// 黑白
    /// </summary>
    blackAndWhite
}
/// <summary>
/// 滤镜风格
/// </summary>
public enum FilterStyle
{

    vintage,
    colorized,
    blackAndWhite
}

public enum EditStyle
{
    vintage,
    colorized,
    blackAndWhite
}
/// <summary>
/// 第二个界面
/// </summary>
public class PanelTwo_C : MonoBehaviour
{

    public Dropdown borderStyle;
    public Dropdown filterStyle;
    public Dropdown editStyle;
    public Sprite[] styleImages;

    void Start()
    {
        ChooseStyle();
    }

    public void ChooseStyle()
    {
        if(borderStyle.value  == 0 && filterStyle.value == 0)
        {

        }
    }



    




    

    

   


}
