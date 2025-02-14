using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rectTransform.offsetMin;
            Vector2 rightTop = rectTransform.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            rectTransform.offsetMin = leftBottom;
            rectTransform.offsetMax = rightTop;
        }
    }
    public virtual void Setup()
    {

    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseImmediate), time);
    }
    public virtual void CloseImmediate()
    {
        gameObject.SetActive(false);
    }

}
