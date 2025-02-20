using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILogics : MonoBehaviour, IPointerEnterHandler
{
    public ScamType scamType;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        ScamWheel.Instance.pickScam(scamType);
    }

}
