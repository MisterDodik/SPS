using System.Collections;
using System.Linq.Expressions;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    [SerializeField] private Image arrow;

    float interval = 0;

    bool leftToRight = false;
    Vector3 startPos = new Vector3(-150, -300);
    Vector3 endPos = new Vector3(150, -300);
    Quaternion leftRot = Quaternion.Euler(0, 0, 90);
    Quaternion rightRot = Quaternion.Euler(0, 0, -90);

    private void Update()
    {
        interval+=Time.deltaTime;
        Vector3 center = (startPos + endPos) / 2;

        // Move the center a bit downwards to make the arc vertical
        center -= new Vector3(0, 1, 0);

        Vector3 startRelCenter = startPos - center;
        Vector3 endRelCenter = endPos - center;

        if (leftToRight)
        {
            arrow.rectTransform.anchoredPosition = Vector3.Slerp(startRelCenter, endRelCenter, interval);
            arrow.rectTransform.anchoredPosition += (Vector2)center;

            arrow.rectTransform.rotation = Quaternion.Slerp(leftRot, rightRot, interval);
            if (Vector2.Distance(arrow.rectTransform.anchoredPosition, endPos) < 0.1f)
            {
                interval = 0;
                leftToRight = false;
            }
        }
        else
        {
            arrow.rectTransform.anchoredPosition = Vector3.Slerp(endRelCenter, startRelCenter, interval);
            arrow.rectTransform.anchoredPosition +=(Vector2) center;

            arrow.rectTransform.rotation = Quaternion.Slerp(rightRot, leftRot, interval);
            if (Vector2.Distance(arrow.rectTransform.anchoredPosition, startPos) < 0.1f)
            {
                interval = 0;
                leftToRight = true;
            }
        }
    }


    public void OnClick()
    {

    }

}
