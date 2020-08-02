using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PanelTwo_M
{
    /// <summary>
    /// 当前选择的边框风格
    /// </summary>
    public BorderStyle borderStyle = BorderStyle.vintage;
    /// <summary>
    /// 滤镜风格
    /// </summary>
    public FilterStyle filterStyle = FilterStyle.vintage;
    /// <summary>
    /// 编辑风格
    /// </summary>
    public EditStyle editStyle = EditStyle.vintage;
    /// <summary>
    /// 当前选择的音乐背景
    /// </summary>
    public AudioClip currentAudioClip;

    public void ClearData()
    {
        /// <summary>
        /// 当前选择的边框风格
        /// </summary>
        borderStyle = BorderStyle.vintage;
        /// <summary>
        /// 滤镜风格
        /// </summary>
        filterStyle = FilterStyle.vintage;
        /// <summary>
        /// 编辑风格
        /// </summary>
        editStyle = EditStyle.vintage;
        currentAudioClip = null;
    }

    public bool Cheak()
    {
        if (currentAudioClip == null)
        {
            return false;
        }
        return true;
    }
}
