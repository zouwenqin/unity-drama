using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject luzhiobj;
    public void Closevideo()
    {
        luzhiobj.SetActive(false);
    }
}
