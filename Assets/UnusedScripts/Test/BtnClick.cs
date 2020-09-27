using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //text.text = "欢迎大家"; //第一种显示文本方法
            //ShowText.Instance.Show("欢迎大家");
        });
    }
}
