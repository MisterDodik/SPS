using UnityEngine;
using TMPro;

public class TextBubbleScript : MonoBehaviour
{
    public GameObject bubblePrefab;

    public static TextBubbleScript instance;

    Transform createdBubble;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void CreateBubble(Transform parent, Vector3 localPosition, string text)
    {
        if (parent == null)
            return;

        if (createdBubble == null)                      //minimizing instantiate calls to improve performance
            createdBubble = Instantiate(bubblePrefab).transform;
        else
        {
            createdBubble.gameObject.SetActive(true);
            createdBubble.parent = parent;
        }

        createdBubble.localPosition = localPosition;

        createdBubble.GetChild(0).GetComponent<TextMeshPro>().text=text;
    }


    public void DestroyBubble()
    {
        if (createdBubble != null)
            createdBubble.gameObject.SetActive(false);
    }
}
