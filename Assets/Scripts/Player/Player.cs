using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    public event EventHandler<OnMoneyChangedEventArgs> OnMoneyChanged;
    public class OnMoneyChangedEventArgs : EventArgs
    {
        public float money;
    }
    private float currentMoney;

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


}
