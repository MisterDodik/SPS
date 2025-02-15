using UnityEngine;
using UnityEngine.UI;

public class CustomButtonShape : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.3f;
    }
}
