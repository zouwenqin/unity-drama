using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public Button button;
    public InputField inputField;
    private void Awake()
    {
        button.onClick.AddListener(delegate { Debug.Log("点击了"); });
        inputField.onEndEdit.AddListener(delegate
        {
            Debug.Log("输入了");
        });
    }
}
