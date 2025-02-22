using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [SerializeField] private Slider suspiciousSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider staminaDrainSlider;

    [SerializeField] private Image staminaImage;

    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject InventoryContent;

    [SerializeField] private GameObject ScamWheel;


    public Slider GetStaminaSlider()
    {
        return staminaSlider;
    }
    public Slider GetDrainStaminaSlider()
    {
        return staminaDrainSlider;
    }
    public Image GetStaminaImage()
    {
        return staminaImage;
    }
    public Slider GetSuspicionSlider()
    {
        return suspiciousSlider;
    }
    public GameObject GetInventoryObject()
    {
        return Inventory;
    }
    public GameObject GetInventoryContent()
    {
        return InventoryContent;
    }
    public GameObject GetScamWheel()
    {
        return ScamWheel;
    }
    private void Start()
    {
        player.OnMoneyChanged += Player_OnMoneyChanged;

        Inventory.SetActive(false);
    }

    private void Player_OnMoneyChanged(object sender, Player.OnMoneyChangedEventArgs e)
    {
        moneyText.text = $"{e.money:N2}$";
    }
}
