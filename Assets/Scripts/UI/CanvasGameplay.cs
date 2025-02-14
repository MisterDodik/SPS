using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [SerializeField] private Slider suspiciousSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI moneyText;

    public Slider GetStaminaSlider()
    {
        return staminaSlider;
    }
    private void Start()
    {
        player.OnMoneyChanged += Player_OnMoneyChanged;
    }

    private void Player_OnMoneyChanged(object sender, Player.OnMoneyChangedEventArgs e)
    {
        moneyText.text = $"{e.money:N0}$";
    }
}
