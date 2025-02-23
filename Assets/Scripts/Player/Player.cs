using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Singleton<Player>
{
    public event EventHandler<OnMoneyChangedEventArgs> OnMoneyChanged;
    public class OnMoneyChangedEventArgs : EventArgs
    {
        public float money;
    }
    private float currentMoney;



    [SerializeField] private Image selectedItemOrScam;
    [SerializeField] private TextMeshProUGUI selectedItemOrScamText;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeMoney(+500);
        }
    }
    public void ChangeMoney(float amount)
    {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(this, new OnMoneyChangedEventArgs()
        {
            money = currentMoney,
        });
    }

    public void updateSelectedItem(Sprite image, string text)
    {
        if (image)
            selectedItemOrScam.sprite = image;
        else
            selectedItemOrScamText.text = text;
    }
}
