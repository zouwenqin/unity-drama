using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager 
{
    private  static GameManager _instance;

    private Sprite sceneImage;
    private string dramaName;
    private Sprite borderStyleImage;
    private Sprite editStyle;
    private string sceneName;
    private string musicName;
    private FilterStyle filterStyle;

    private string[] actorName = new String[3];
    private Sprite[] actorImage = new Sprite[3];

    public Dictionary<string, string> videoinfo = new Dictionary<string, string>();
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    
    public void SetScenarioName(string name)
    {
        dramaName = name;
    }
    public string GetScenarioName()
    {
        return dramaName;
    }

    public void SetSceneImage(Sprite sprite)
    {
        sceneImage = sprite;
    }

    public Sprite GetSceneImage()
    {
        return sceneImage;
    }

    public void SetActorName(int actorID,string name)
    {
        actorName[actorID] = name;
    }

    public void SetActorImage(int index, Sprite sprite)
    {
        
        actorImage[index] = sprite;
    }

    public Sprite GetActorImage(int index)
    {
        return actorImage[index];
    }

    
    public void SetBorderStyle(Sprite sprite)
    {
        borderStyleImage = sprite;
    }
    
    public Sprite GetborderStyle()
    {
        return borderStyleImage;
    }

    public void SetFilterStyle(FilterStyle filter)
    {
        filterStyle = filter;
    }
    public FilterStyle GetFilterStyle()
    {
        return filterStyle;
    }

    //TODO 
    public string GetEditStyle()
    {
        return null;
    }
}
