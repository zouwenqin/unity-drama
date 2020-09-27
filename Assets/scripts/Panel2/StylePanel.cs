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
    /// 清新
    /// </summary>
    fresh
}

/// <summary>
/// 滤镜风格
/// </summary>
public enum FilterStyle
{
    vintage,
    soft,
    bright
}

public enum EditStyle
{
    vintage,
    colorized,
    blackAndWhite
}

/// <summary>
/// 风格选择界面
/// </summary>
public class StylePanel : MonoBehaviour
{
    public Dropdown borderStyle;
    public Dropdown filterStyle;
    public Dropdown editStyle;
    [HideInInspector]
    public Sprite borderImage;
    public Sprite vintageBorder;
    public Sprite colorizedBorder;
    public Sprite freshBorder;
    public Camera RecorderCamera;
    public Image sceneImage;

    void Start()
    {
        borderImage = vintageBorder;
        GameManager.Instance.SetFilterStyle(FilterStyle.vintage);       
        borderStyle.onValueChanged.AddListener(BorderDropDownListener);
        filterStyle.onValueChanged.AddListener(FilterDropDownListener);
    }

    public void OnEnable()
    {
        sceneImage.sprite = GameManager.Instance.GetSceneImage();
    }

    private void OnDisable()
    {
        GameManager.Instance.SetBorderStyle(borderImage);
    }

    public void BorderDropDownListener(int value)
    {
        if(value == 0)
        {
            borderImage = vintageBorder;
        }
        if(value == 1)
        {
            borderImage = colorizedBorder;
        }
        if(value ==2)
        {
            borderImage = freshBorder;
        }
        
    }

    public void FilterDropDownListener(int value)
    {
        if (value == 0)
        {
            GameManager.Instance.SetFilterStyle(FilterStyle.vintage);
        }
        else if (value == 1)
        {
            GameManager.Instance.SetFilterStyle(FilterStyle.soft);
        }
        else if (value == 2)
        {
            GameManager.Instance.SetFilterStyle(FilterStyle.bright);
        }

    }

   



    




    

    

   


}
