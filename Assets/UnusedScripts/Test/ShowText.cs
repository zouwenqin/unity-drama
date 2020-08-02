using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public static ShowText Instance;

    private void Awake()
    {
        Instance = this; //对单例模式进行赋值
    }

    public void Show(string str)
    {
        GetComponent<Text>().text = str;
    }
}
