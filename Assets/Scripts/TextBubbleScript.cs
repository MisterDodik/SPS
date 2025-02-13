using UnityEngine;
using TMPro;

public class TextBubbleScript : MonoBehaviour
{
    public GameObject bubblePrefab;

    public static TextBubbleScript instance;

    Transform createdBubble;
    TextMeshPro textChild;

    [SerializeField] Transform player;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        createdBubble = Instantiate(bubblePrefab).transform;

        createdBubble.gameObject.SetActive(true);  
        textChild = createdBubble.GetChild(0).GetComponent<TextMeshPro>();
        textChild.ForceMeshUpdate();            // Prevents lag spike later on
        textChild.text = "";  
        createdBubble.gameObject.SetActive(false);
    }

    public void CreateBubble(Transform parent, Vector3 localPosition, string text)
    {
        if (parent == null || createdBubble.gameObject.activeSelf)
            return;

        if (createdBubble == null)                      // Minimizing instantiate calls to improve performance
            createdBubble = Instantiate(bubblePrefab).transform;
        else
        {
            createdBubble.gameObject.SetActive(true);
        }

        createdBubble.parent = parent;
        createdBubble.localPosition = localPosition;

        textChild.text=text;
    }

    public void DestroyBubble()
    {
        if (createdBubble != null && createdBubble.gameObject.activeSelf)     
            createdBubble.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (createdBubble.gameObject.activeSelf)
        {
            Vector3 direction = player.position - createdBubble.position;
            Quaternion rotation = Quaternion.LookRotation(-direction.normalized, Vector3.up);
            createdBubble.localRotation = rotation;
        }
    }
}
