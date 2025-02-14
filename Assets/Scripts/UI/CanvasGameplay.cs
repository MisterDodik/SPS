using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [SerializeField] private Slider suspiciousSlider;
    [SerializeField] private Slider staminaSlider;

    public Slider GetStaminaSlider()
    {
        return staminaSlider;
    }
}
