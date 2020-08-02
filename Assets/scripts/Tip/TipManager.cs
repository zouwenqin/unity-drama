using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{
    public static TipManager Instance;
    public Text text_tip;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void TipShow(string tipStr, float showTime = 1)
    {
        text_tip.gameObject.SetActive(true);
        text_tip.text = tipStr;
        Invoke("TipHide", showTime);
    }
    public void TipHide()
    {
        text_tip.gameObject.SetActive(false);
    }
}
