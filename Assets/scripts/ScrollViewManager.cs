using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;
    private float[] pageArray = new float[] { 0, 1 };
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void UpdateSize(Transform trans)
    {
        if (trans.localScale == Vector3.one)
        {
            trans.localScale = Vector3.one * 1.5f;
        }
        else
        {
            trans.localScale = Vector3.one;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float posX = scrollRect.horizontalNormalizedPosition;
        int index = 0;
        float offset = Mathf.Abs(pageArray[index] - posX);
        for (int i = 1; i < pageArray.Length; i++)
        {
            float offsetTemp = Mathf.Abs(pageArray[i] - posX);
            if(offsetTemp < offset)
            {
                index = i;
                offset = offsetTemp;
            }
        }
        scrollRect.horizontalNormalizedPosition = pageArray[index];
    }
}
