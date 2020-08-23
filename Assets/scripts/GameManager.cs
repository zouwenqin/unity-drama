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
    private string borderStyle;
    private string editStyle;
    private string filterStyle;
    private string sceneName;
    private string musicName;
    private string[] actorName = { "" , "" ,"" };
    private Sprite[] actorImage = { null, null, null };

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

    public string GetborderStyle()
    {
        return null;
    }

    public string GetEditStyle()
    {
        return null;
    }

    public string GetFilterStyle()
    {
        return null;
    }
}
